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
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProdutoRepositorySQL _produtoRepositorySQL;
        public ProdutoRepository(ApplicationDbContext dbContext, IProdutoRepositorySQL produtoRepositorySQL)
        {
            _context = dbContext;
            _produtoRepositorySQL = produtoRepositorySQL;
        }
        public async Task<List<CWProduto>> PesquisarTodos(int page = 1, int pageSize = 10, CWProduto? oCWProdutoFiltro = null)
        {
            var query = _context.Produto.AsNoTracking().AsQueryable();
            query = oCWProdutoFiltro == null ? query : AplicarFiltros(query, oCWProdutoFiltro);
            return await query.OrderBy(p => p.nCdProduto).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        private IQueryable<CWProduto> AplicarFiltros(IQueryable<CWProduto> query, CWProduto filtro)
        {
            query = !string.IsNullOrEmpty(filtro.sNmProduto) ? query.Where(p => EF.Functions.Like(p.sNmProduto, $"%{filtro.sNmProduto}%")) : query;
            query = !string.IsNullOrEmpty(filtro.sDsProduto) ? query.Where(p => EF.Functions.Like(p.sDsProduto, $"%{filtro.sDsProduto}%")) : query;
            return query;
        }

        public async Task<int> PesquisarQuantidadePaginas(CWProduto? cwProdutoFiltro = null)
        {
            var query = _context.Produto.AsNoTracking().AsQueryable();
            query = cwProdutoFiltro == null ? query : AplicarFiltros(query, cwProdutoFiltro);
            return await query.CountAsync();
        }
        public CWProduto ConsultarProduto(int nCdProduto)
        {
            return _context.Produto.FirstOrDefault(x => x.nCdProduto == nCdProduto);
        }
        public async Task<List<CWUnidadeMedida>> PesquisarUnidadeMedidas()
        {
            return await _context.UnidadeMedida.Where(x => x.bFlAtivo == 1).ToListAsync();
        }
        public async Task<List<CWProdutoImagem>> PesquisarProdutoImagens(int nCdProduto)
        {
            return await _context.ProdutoImagem.Where(x => x.nCdProduto == nCdProduto).ToListAsync();
        }

        public async Task<List<CWVariacao>> PesquisarVariacoes()
        {
            var variacoes = await _context.Set<CWVariacao>()
                .Where(v => v.bFlAtiva)
                .Select(v => new CWVariacao
                {
                    nCdVariacao = v.nCdVariacao,
                    sNmVariacao = v.sNmVariacao,
                    sDsVariacao = v.sDsVariacao,
                    VariacaoOpcoes = v.VariacaoOpcoes
                        .Where(vo => vo.bFlAtiva) 
                        .Select(vo => new CWVariacaoOpcao
                        {
                            nCdVariacaoOpcao = vo.nCdVariacaoOpcao,
                            sNmVariacaoOpcao = vo.sNmVariacaoOpcao,
                            sDsVariacaoOpcao = vo.sDsVariacaoOpcao
                        }).ToList()
                })
                .ToListAsync();

            return variacoes;
        }
        public async Task<int> CadastrarProduto(CWProduto cwProduto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var produtoExistente = await _context.Produto.FirstOrDefaultAsync(p => p.nCdProduto == cwProduto.nCdProduto);
                int nCdProduto;
                if (produtoExistente == null)
                {
                    await _context.Produto.AddAsync(cwProduto);
                    await _context.SaveChangesAsync();
                    nCdProduto = cwProduto.nCdProduto;
                }
                else
                {
                    produtoExistente.sNmProduto = cwProduto.sNmProduto;
                    produtoExistente.sDsProduto = cwProduto.sDsProduto;
                    produtoExistente.sCdProduto = cwProduto.sCdProduto;
                    produtoExistente.sPeso = cwProduto.sPeso;
                    produtoExistente.sAltura = cwProduto.sAltura;
                    produtoExistente.sComprimento = cwProduto.sComprimento;
                    produtoExistente.sUrlVideo = cwProduto.sUrlVideo;
                    produtoExistente.dVlVenda = cwProduto.dVlVenda;
                    produtoExistente.dVlUnitario = cwProduto.dVlUnitario;
                    produtoExistente.nCdUnidadeMedida = cwProduto.nCdUnidadeMedida;

                    _context.Entry(produtoExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    nCdProduto = produtoExistente.nCdProduto;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return nCdProduto;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task AdicionarImagem(CWProdutoImagem oCWProdutoImagem)
        {
            _context.ProdutoImagem.Add(oCWProdutoImagem);
            await _context.SaveChangesAsync();
        }
        public async Task EditarVariacaoProduto(int nCdProduto, List<CWProdutoOpcaoVariacao> variacoes)
        {
            var variacoesAtuais = await _context.ProdutoOpcaoVariacao.Where(pov => pov.nCdProduto == nCdProduto).ToListAsync();
            var variacoesParaAdicionar = new List<CWProdutoOpcaoVariacao>();
            var variacoesParaRemover = new List<CWProdutoOpcaoVariacao>();
            foreach (var variacao in variacoes)
            {
                var variacaoExistente = variacoesAtuais.FirstOrDefault(pov => pov.nCdVariacaoOpcao == variacao.nCdVariacaoOpcao && pov.nCdVariacao == variacao.nCdVariacao);

                if (variacaoExistente == null)
                {
                    variacoesParaAdicionar.Add(new CWProdutoOpcaoVariacao
                    {
                        nCdProduto = nCdProduto,
                        nCdVariacao = variacao.nCdVariacao,
                        nCdVariacaoOpcao = variacao.nCdVariacaoOpcao
                    });
                }
            }

            foreach (var variacaoAtual in variacoesAtuais)
            {
                var variacaoEnviada = variacoes.FirstOrDefault(v => v.nCdVariacaoOpcao == variacaoAtual.nCdVariacaoOpcao && v.nCdVariacao == variacaoAtual.nCdVariacao);
                if (variacaoEnviada == null) variacoesParaRemover.Add(variacaoAtual);
            }

            if (variacoesParaAdicionar.Any())
            {
                await _context.ProdutoOpcaoVariacao.AddRangeAsync(variacoesParaAdicionar);
            }

            if (variacoesParaRemover.Any())
            {
                _context.ProdutoOpcaoVariacao.RemoveRange(variacoesParaRemover);
            }

            await _context.SaveChangesAsync();
        }
        public async Task ExcluirImagem(int nCdImagem)
        {
            var produtoImagem = await _context.ProdutoImagem.FirstOrDefaultAsync(pov => pov.nCdImagem == nCdImagem);
            if(produtoImagem != null) _context.ProdutoImagem.Remove(produtoImagem);
            await _context.SaveChangesAsync();

        }
        public async Task AtualizarProduto(CWProduto produto)
        {
            await _context.Database.ExecuteSqlRawAsync(_produtoRepositorySQL.AtualizarProdutoSQL(),
                new SqlParameter("@sNmProduto", produto.sNmProduto),
                new SqlParameter("@sDsProduto", produto.sDsProduto ?? (object)DBNull.Value),
                new SqlParameter("@sCdProduto", produto.sCdProduto ?? (object)DBNull.Value),
                new SqlParameter("@sUrlVideo", produto.sUrlVideo ?? (object)DBNull.Value),
                new SqlParameter("@sLargura", produto.sLargura ?? (object)DBNull.Value),
                new SqlParameter("@sComprimento", produto.sComprimento ?? (object)DBNull.Value),
                new SqlParameter("@sAltura", produto.sAltura ?? (object)DBNull.Value),
                new SqlParameter("@sPeso", produto.sPeso ?? (object)DBNull.Value), 
                new SqlParameter("@dVlVenda", produto.dVlVenda),
                new SqlParameter("@dVlUnitario", produto.dVlUnitario),
                new SqlParameter("@nCdUnidadeMedida", produto.nCdUnidadeMedida),
                new SqlParameter("@nCdProduto", produto.nCdProduto)
            );
            await _context.SaveChangesAsync();
        }
        public async Task<List<CWVariacao>> ConsultarProdutoVariacao(int nCdProduto)
        {
            return await _context.Set<CWVariacao>()
                .Where(v => v.bFlAtiva)
                .Select(v => new CWVariacao
                {
                    nCdVariacao = v.nCdVariacao,
                    sNmVariacao = v.sNmVariacao,
                    sDsVariacao = v.sDsVariacao,
                    VariacaoOpcoes = v.VariacaoOpcoes
                        .Where(vo => vo.bFlAtiva)
                        .Select(vo => new CWVariacaoOpcao
                        {
                            nCdVariacaoOpcao = vo.nCdVariacaoOpcao,
                            sNmVariacaoOpcao = vo.sNmVariacaoOpcao,
                            sDsVariacaoOpcao = vo.sDsVariacaoOpcao,
                            bFlAtrelado = _context.ProdutoOpcaoVariacao.Any(pov => pov.nCdProduto == nCdProduto && pov.nCdVariacaoOpcao == vo.nCdVariacaoOpcao)
                        }).ToList()
                }).Where(v => v.VariacaoOpcoes.Any(vo => vo.bFlAtrelado)).ToListAsync();

        }
    }
}
