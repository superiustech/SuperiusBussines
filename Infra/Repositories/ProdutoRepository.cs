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

        public async Task<int> PesquisarQuantidadePaginas()
        {
            return await _context.Produto.CountAsync();
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
        public async Task<int> CadastrarProduto(CWProduto cwProduto, List<CWProdutoOpcaoVariacaoBase> variacoes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Produto.AddAsync(cwProduto);
                await _context.SaveChangesAsync();

                int nCdProduto = cwProduto.nCdProduto;
                List<CWProdutoOpcaoVariacaoBase> lstProdutoOpcaoVariacao = new List<CWProdutoOpcaoVariacaoBase>();
                foreach (var variacao in variacoes)
                {
                    lstProdutoOpcaoVariacao.Add(new CWProdutoOpcaoVariacaoBase
                    {
                        nCdProduto = nCdProduto,
                        nCdVariacao = variacao.nCdVariacao,
                        nCdVariacaoOpcao = variacao.nCdVariacaoOpcao
                    });
                }

                await _context.ProdutoOpcaoVariacao.AddRangeAsync(lstProdutoOpcaoVariacao);
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
        public async Task EditarVariacaoProduto(int nCdProduto, List<CWProdutoOpcaoVariacaoBase> variacoes)
        {
            var variacoesAtuais = await _context.ProdutoOpcaoVariacao.Where(pov => pov.nCdProduto == nCdProduto).ToListAsync();
            var variacoesParaAdicionar = new List<CWProdutoOpcaoVariacaoBase>();
            var variacoesParaRemover = new List<CWProdutoOpcaoVariacaoBase>();
            foreach (var variacao in variacoes)
            {
                var variacaoExistente = variacoesAtuais.FirstOrDefault(pov => pov.nCdVariacaoOpcao == variacao.nCdVariacaoOpcao && pov.nCdVariacao == variacao.nCdVariacao);

                if (variacaoExistente == null)
                {
                    variacoesParaAdicionar.Add(new CWProdutoOpcaoVariacaoBase
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
        public async Task<List<dynamic>> ConsultarProdutoVariacao(int nCdProduto)
        {
            var result = new List<dynamic>();
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = _produtoRepositorySQL.ConsultarProdutoVariacaoSQL();
                    command.CommandType = CommandType.Text;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@nCdProduto";
                    parameter.Value = nCdProduto;
                    command.Parameters.Add(parameter);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new
                            {
                                nCdVariacaoOpcao = reader["nCdVariacaoOpcao"],
                                sNmVariacaoOpcao = reader["sNmVariacaoOpcao"],
                                nCdVariacao = reader["nCdVariacao"],
                                bFlAtrelado = reader["bFlAtrelado"]
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}
