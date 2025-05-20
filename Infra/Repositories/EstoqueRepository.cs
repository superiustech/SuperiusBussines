using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infra.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly ApplicationDbContext _context;
        public EstoqueRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<CWEstoque> Consultar(int nCdEstoque)
        {
            return await _context.Estoque
                .AsNoTracking()
                .Include(e => e.Produtos.Where(x => x.bFlAtivo))
                .ThenInclude(ep => ep.Produto)
                .FirstOrDefaultAsync(x => x.nCdEstoque == nCdEstoque);
        }
        public async Task<List<CWEstoqueProdutoHistorico>> ConsultarHistorico(int nCdEstoque)
        {
            return await _context.EstoqueProdutoHistorico
                .AsNoTracking()
                .OrderByDescending(x => x.nCdEstoqueProdutoHistorico)
                .Where(h => h.nCdEstoque == nCdEstoque)
                .Include(h => h.Produto)
                .Include(h => h.EstoqueOrigem)
                .Include(h => h.EstoqueDestino)
                .ToListAsync();
        }
        
        public async Task<List<CWEstoque>> PesquisarTodos(int page = 0, int pageSize = 0, CWEstoque? oCWEstoqueFiltro = null)
        {
            if (page == 0 || pageSize == 0)
            {
                return await _context.Estoque.AsNoTracking().OrderBy(p => p.nCdEstoque).ToListAsync();
            }
            else
            {
                var query = _context.Estoque.AsNoTracking().AsQueryable();
                query = oCWEstoqueFiltro == null ? query : AplicarFiltros(query, oCWEstoqueFiltro);
                return await query.OrderBy(p => p.nCdEstoque).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }
        }
        public async Task<List<CWEstoque>> PesquisarSemRevendedores(int? nCdRevendedor = null)
        {
            int? estoqueDoRevendedor = null;

            if (nCdRevendedor.HasValue)
            {
                estoqueDoRevendedor = await _context.Revendedor
                    .Where(r => r.nCdRevendedor == nCdRevendedor)
                    .Select(r => r.nCdEstoque)
                    .FirstOrDefaultAsync();
            }

            return await _context.Estoque
                .Where(e =>
                    !_context.Revendedor.Any(r => r.nCdEstoque == e.nCdEstoque)
                    || (estoqueDoRevendedor.HasValue && e.nCdEstoque == estoqueDoRevendedor.Value))
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<CWEstoque>> PesquisarTodosEstoques()
        {
            return await _context.Estoque.AsNoTracking().OrderBy(p => p.nCdEstoque).ToListAsync();
        }
        public IQueryable<CWEstoque> AplicarFiltros(IQueryable<CWEstoque> query, CWEstoque filtro)
        {
            query = !string.IsNullOrEmpty(filtro.sNmEstoque) ? query.Where(p => EF.Functions.Like(p.sNmEstoque, $"%{filtro.sNmEstoque}%")) : query;
            query = !string.IsNullOrEmpty(filtro.sDsEstoque) ? query.Where(p => EF.Functions.Like(p.sDsEstoque, $"%{filtro.sDsEstoque}%")) : query;
            return query;
        }
        public async Task<int> PesquisarQuantidadePaginas(CWEstoque? cwEstoqueFiltro = null)
        {
            var query = _context.Estoque.AsNoTracking().AsQueryable();
            query = cwEstoqueFiltro == null ? query : AplicarFiltros(query, cwEstoqueFiltro);
            return await query.CountAsync();
        }
        public async Task CadastrarEstoque(CWEstoque oCWEstoque)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var estoqueExistente = await _context.Estoque.FirstOrDefaultAsync(p => p.nCdEstoque == oCWEstoque.nCdEstoque);
                int nCdEstoque = 0;
                if (estoqueExistente == null)
                {
                    await _context.Estoque.AddAsync(oCWEstoque);
                    await _context.SaveChangesAsync();
                    nCdEstoque = oCWEstoque.nCdEstoque;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    estoqueExistente.sNmEstoque = oCWEstoque.sNmEstoque;
                    estoqueExistente.sDsEstoque = oCWEstoque.sDsEstoque;
                    estoqueExistente.sCdEstoque = oCWEstoque.sCdEstoque;
                    estoqueExistente.sDsRua = oCWEstoque.sDsRua;
                    estoqueExistente.sDsComplemento = oCWEstoque.sDsComplemento;
                    estoqueExistente.sCdCep = oCWEstoque.sCdCep;
                    estoqueExistente.sNrNumero = oCWEstoque.sNrNumero;

                    _context.Entry(estoqueExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    nCdEstoque = estoqueExistente.nCdEstoque;
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<CWEstoqueProduto>> PesquisarPorEstoqueProduto(int nCdEstoque)
        {
            var produtosIds = await _context.EstoqueProduto.Where(ep => ep.nCdEstoque == nCdEstoque).Select(ep => ep.nCdProduto).Distinct().ToListAsync();
            return await _context.EstoqueProduto.Where(p => produtosIds.Contains(p.nCdProduto) && p.bFlAtivo).ToListAsync();
        }
        public async Task<bool> AdicionarEstoqueProduto(CWEstoqueProduto cwEstoqueProduto)
        {
            var estoqueExistente = await ObterEstoqueExistente(cwEstoqueProduto);

            if (estoqueExistente != null)
            {
                AtualizarEstoqueExistenteEntrada(estoqueExistente, cwEstoqueProduto);
            }
            else
            {
                _context.EstoqueProduto.Add(cwEstoqueProduto);
            }

            AdicionarHistoricoMovimentacaoEntrada(cwEstoqueProduto, "Entrada Manual", nTipoMovimentacao.Entrada);

            await _context.SaveChangesAsync();

            return estoqueExistente != null;
        }
        public async Task MovimentarEntradaSaida(CWEstoqueProduto cwEstoqueProduto, CWEstoqueProdutoHistorico cwEstoqueProdutoHistorico)
        {
            var estoqueExistente = await ObterEstoqueExistente(cwEstoqueProduto);

            if (estoqueExistente != null)
            {
                estoqueExistente.dQtMinima = cwEstoqueProduto.dQtMinima;
                estoqueExistente.dQtEstoque = cwEstoqueProduto.dQtEstoque;
                estoqueExistente.dVlVenda = cwEstoqueProduto.dVlVenda;
                estoqueExistente.dVlCusto = cwEstoqueProduto.dVlCusto;
                estoqueExistente.bFlAtivo = true;

                _context.Entry(estoqueExistente).State = EntityState.Modified;
            }
            else
            {
                _context.EstoqueProduto.Add(cwEstoqueProduto);
            }

            _context.EstoqueProdutoHistorico.Add(cwEstoqueProdutoHistorico);

            await _context.SaveChangesAsync();
        }

        public async Task<CWEstoqueProduto> ObterEstoqueExistente(CWEstoqueProduto cwEstoqueProduto)
        {
            return await _context.EstoqueProduto.FirstOrDefaultAsync(ep => ep.nCdProduto == cwEstoqueProduto.nCdProduto && ep.nCdEstoque == cwEstoqueProduto.nCdEstoque);
        }

        public async Task<decimal> ObterPercentualRevenda(int nCdEstoque)
        {
            var revendedor = await _context.Revendedor.FirstOrDefaultAsync(r => r.nCdEstoque == nCdEstoque);
            return revendedor != null && revendedor.dPcRevenda > 0 ? revendedor.dPcRevenda / 100m : 0m;
        }

        public async Task<CWEstoqueProduto> CarregarDadosProdutoEEstoque(CWEstoqueProduto cwEstoqueProduto)
        {
            var cwEstoqueProdutoRetorno = await _context.EstoqueProduto.Where(x => x.nCdEstoque == cwEstoqueProduto.nCdEstoque && x.nCdProduto == cwEstoqueProduto.nCdProduto).FirstOrDefaultAsync() ?? new CWEstoqueProduto();
            cwEstoqueProdutoRetorno.Estoque = await _context.Estoque.FindAsync(cwEstoqueProduto.nCdEstoque);
            cwEstoqueProdutoRetorno.Produto = await _context.Produto.FindAsync(cwEstoqueProduto.nCdProduto);
            return cwEstoqueProdutoRetorno;
        }

        public void DefinirValorVenda(CWEstoqueProduto cwEstoqueProduto, decimal percentualRevenda)
        {
            cwEstoqueProduto.dVlVenda = cwEstoqueProduto.Produto.dVlVenda * (1 + percentualRevenda);
        }

        public void AtualizarEstoqueExistenteEntrada(CWEstoqueProduto estoqueExistente, CWEstoqueProduto novoEstoque)
        {
            estoqueExistente.dQtMinima = novoEstoque.dQtMinima;
            estoqueExistente.dQtEstoque += novoEstoque.dQtEstoque;
            estoqueExistente.dVlVenda = novoEstoque.dVlVenda;
            estoqueExistente.dVlCusto = novoEstoque.dVlCusto;
            estoqueExistente.bFlAtivo = true;

            _context.Entry(estoqueExistente).State = EntityState.Modified;
        }

        public void AdicionarHistoricoMovimentacaoEntrada(CWEstoqueProduto cwEstoqueProduto, string sDsObservacao, nTipoMovimentacao onTipoMovimentacao)
        {
            var historico = new CWEstoqueProdutoHistorico
            {
                nCdEstoqueProdutoHistorico = 0,
                nCdEstoque = cwEstoqueProduto.nCdEstoque,
                nCdProduto = cwEstoqueProduto.nCdProduto,
                nCdEstoqueDestino = cwEstoqueProduto.nCdEstoque,
                dQtMovimentada = cwEstoqueProduto.dQtEstoque,
                tDtMovimentacao = default,
                sDsObservacao = sDsObservacao,
                nTipoMovimentacao = onTipoMovimentacao
            };

            _context.EstoqueProdutoHistorico.Add(historico);
        } 
        public async Task RemoverEstoqueProduto(List<CWEstoqueProduto> lstEstoqueProduto)
        {
            var historicos = new List<CWEstoqueProdutoHistorico>();
            var chaves = lstEstoqueProduto.Select(x => new { x.nCdEstoque, x.nCdProduto }).ToList();
            var estoquesExistentes = await _context.EstoqueProduto.Where(ep => chaves.Select(c => c.nCdEstoque).Contains(ep.nCdEstoque) && chaves.Select(c => c.nCdProduto).Contains(ep.nCdProduto)).ToListAsync();

            foreach (var estoque in estoquesExistentes)
            {
                historicos.Add(new CWEstoqueProdutoHistorico
                {
                    nCdEstoqueProdutoHistorico = 0,
                    nCdEstoque = estoque.nCdEstoque,
                    nCdProduto = estoque.nCdProduto,
                    nCdEstoqueDestino = estoque.nCdEstoque,
                    dQtMovimentada = estoque.dQtEstoque,
                    tDtMovimentacao = default,
                    sDsObservacao = "Inativação do produto ao estoque",
                    nTipoMovimentacao = nTipoMovimentacao.Saida
                });

                estoque.bFlAtivo = false;
                estoque.dQtEstoque = 0;

                _context.Entry(estoque).State = EntityState.Modified;
            }

            _context.EstoqueProdutoHistorico.AddRange(historicos);
            await _context.SaveChangesAsync();
        }
        public async Task AdicionarEditarProdutoEstoque(CWEstoqueProduto cwEstoqueProduto)
        {
            var estoqueExistente = await _context.EstoqueProduto.FirstOrDefaultAsync(ep => ep.nCdProduto == cwEstoqueProduto.nCdProduto && ep.nCdEstoque == cwEstoqueProduto.nCdEstoque);
            if (estoqueExistente != null)
            {
                estoqueExistente.dQtMinima = cwEstoqueProduto.dQtMinima;
                estoqueExistente.dQtEstoque = cwEstoqueProduto.dQtEstoque;
                estoqueExistente.dVlVenda = cwEstoqueProduto.dVlVenda;
                estoqueExistente.dVlCusto = cwEstoqueProduto.dVlCusto;
                _context.Entry(estoqueExistente).State = EntityState.Modified;
            }
            else{
                _context.EstoqueProduto.Add(cwEstoqueProduto);
            }

            await _context.SaveChangesAsync();
        }
        public async Task ExcluirEstoques(List<CWEstoque> lstEstoques)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lstCdEstoques = lstEstoques.Select(p => p.nCdEstoque).ToList();
                    var estoquesProdutos = _context.EstoqueProduto.Where(pov => lstCdEstoques.Contains(Convert.ToInt32(pov.nCdEstoque)));

                   _context.EstoqueProduto.RemoveRange(estoquesProdutos);
                   _context.Estoque.RemoveRange(lstEstoques);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
