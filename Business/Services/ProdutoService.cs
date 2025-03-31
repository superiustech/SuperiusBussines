using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Business.Services
{
    public class ProdutoService : IProduto
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProdutoService(IProdutoRepository produtoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _produtoRepository = produtoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CWProduto>> PesquisarProdutos(int page, int pageSize, CWProduto? oCWProdutoFiltro = null)
        {
            return await _produtoRepository.PesquisarTodos(page, pageSize, oCWProdutoFiltro);
        }
        public async Task<int> PesquisarQuantidadePaginas()
        {
            return await _produtoRepository.PesquisarQuantidadePaginas();
        }
        public CWProduto ConsultarProduto(int nCdProduto)
        {
            return _produtoRepository.ConsultarProduto(nCdProduto);
        }
        public async Task<List<CWVariacao>> ObterVariacoesAtivas() 
        {
            List<CWVariacao> lstProdutoVariacao = new List<CWVariacao>();
            lstProdutoVariacao = await _produtoRepository.PesquisarVariacoes();
            return lstProdutoVariacao;
        }
        public async Task AtualizarProduto(CWProduto oCWProduto)
        {
            try
            {
                await _produtoRepository.AtualizarProduto(oCWProduto);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao atualizar o produto.", ex);
            }
        }
        public async Task ExcluirImagem(int nCdImagem)
        {
            try
            {
                await _produtoRepository.ExcluirImagem(nCdImagem);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao excluir a imagem.", ex);
            }
        }
        public async Task<int> CadastrarProduto(CWProduto cwProduto, List<CWVariacao> variacoes)
        {
            try
            {
                var lstProdutoOpcaoVariacao = variacoes
                    .SelectMany(v => v.VariacaoOpcoes
                        .Select(opcao => new CWProdutoOpcaoVariacaoBase
                        {
                            nCdProduto = cwProduto.nCdProduto,
                            nCdVariacao = v.nCdVariacao,
                            nCdVariacaoOpcao = opcao?.nCdVariacaoOpcao ?? 0
                        }))
                    .ToList();

                int nCdProduto = await _produtoRepository.CadastrarProduto(cwProduto, lstProdutoOpcaoVariacao);
                return nCdProduto;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar o produto.", ex);
            }
        }
        public async Task EditarVariacaoProduto(int nCdProduto, List<CWVariacao> variacoes)
        {
            try
            {   
                List<CWProdutoOpcaoVariacaoBase> lstProdutoOpcaoVariacao = new List<CWProdutoOpcaoVariacaoBase>();
                foreach (var cwVaricacao in variacoes)
                {
                    foreach (var cwVariacaoOpcao in cwVaricacao.VariacaoOpcoes)
                    {
                        lstProdutoOpcaoVariacao.Add(new CWProdutoOpcaoVariacaoBase()
                        {
                            nCdProduto = nCdProduto,
                            nCdVariacao = cwVaricacao.nCdVariacao,
                            nCdVariacaoOpcao = cwVariacaoOpcao?.nCdVariacaoOpcao ?? 0
                        });
                    }
                }
                await _produtoRepository.EditarVariacaoProduto(nCdProduto, lstProdutoOpcaoVariacao);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao editar a variação do produto.", ex);

            }
        }
        public async Task<List<CWUnidadeMedida>> ObterUnidadesAtivas()
        {
            List<CWUnidadeMedida> lstUnidadeMedida = new List<CWUnidadeMedida>();
            lstUnidadeMedida = await _produtoRepository.PesquisarUnidadeMedidas();
            return lstUnidadeMedida;
        }
        public async Task<List<CWProdutoImagem>> ObterImagensProduto(int nCdProduto)
        {
            List<CWProdutoImagem> lstProdutoImagem = new List<CWProdutoImagem>();
            lstProdutoImagem = await _produtoRepository.PesquisarProdutoImagens(nCdProduto);
            return lstProdutoImagem;
        }
        public async Task<List<dynamic>> ConsultarProdutoVariacao(int nCdProduto)
        {
            var lstProdutoOpcaoVariacao = await _produtoRepository.ConsultarProdutoVariacao(nCdProduto);
            return lstProdutoOpcaoVariacao;
        }
        public async Task<CWProdutoImagem> AdicionarImagem(int nCdProduto, IFormFile Imagem, string Descricao)
        {
            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(Imagem.FileName);
            var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await Imagem.CopyToAsync(stream);
            }

            var produtoImagem = new CWProdutoImagem
            {
                nCdProduto = nCdProduto,
                sDsImagem = Descricao,
                sDsCaminho = Path.Combine("images", nomeArquivo)
            };
            await _produtoRepository.AdicionarImagem(produtoImagem);

            return produtoImagem;
        }
        public async Task<List<CWProduto>> PesquisarPorEstoque(int nCdEstoque)
        {
            try
            {
                if (nCdEstoque > 0)
                {
                    return await _produtoRepository.PesquisarPorEstoque(nCdEstoque);
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao pesquisar produtos por estoque. Código inválido.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao pesquisar produtos por estoque.", ex);

            }
        }
    }
}
