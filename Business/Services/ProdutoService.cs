using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Uteis;
using Domain.Entities.ViewModel;
using Domain.Interfaces;
using Domain.ViewModel;
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
        public async Task<DTOProduto> ConsultarProduto(int nCdProduto)
        {
            try
            {
                CWProduto produto = await _produtoRepository.ConsultarProduto(nCdProduto);
                DTOProduto oDTOProduto = new DTOProduto()
                {
                    Codigo = produto.nCdProduto,
                    CodigoProduto = produto.sCdProduto,
                    Nome = produto.sNmProduto,
                    Descricao = produto.sDsProduto,
                    CodigoUnidadeMedida = produto.nCdUnidadeMedida,
                    ValorUnitario = produto.dVlUnitario,
                    ValorVenda = produto.dVlVenda,
                    UrlVideo = produto.sUrlVideo,
                    Altura = produto.sAltura,
                    Largura = produto.sLargura,
                    Comprimento = produto.sComprimento,
                    Peso = produto.sPeso
                };

                return oDTOProduto;
            }
            catch 
            {
                throw;
            }
        }
        public async Task<List<DTOProduto>> PesquisarTodosProdutos()
        {
            try
            {
                List<CWProduto> produtos = await _produtoRepository.PesquisarTodosProdutos();
                List<DTOProduto> lstProdutosDTO = new List<DTOProduto>();

                lstProdutosDTO.AddRange(produtos.Select(x => new DTOProduto()
                {
                    Codigo = x.nCdProduto,
                    CodigoProduto = x.sCdProduto,
                    Nome = x.sNmProduto,
                    Descricao = x.sDsProduto,
                    ValorUnitario = x.dVlUnitario,
                    ValorVenda = x.dVlVenda
                }).ToList());

                return lstProdutosDTO;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOVariacao>> ObterVariacoesAtivas(){
           return await ObterVariacoesAtivas(0);
        }
        public async Task<List<DTOVariacao>> ObterVariacoesAtivas(int? tipo) 
        {
            try
            {
                List<CWVariacao> lstProdutoVariacao = await _produtoRepository.PesquisarVariacoes();
                List<DTOVariacao> lstDTOVariacoes = new List<DTOVariacao>();

                if (tipo > 0) lstProdutoVariacao.FirstOrDefault(x => x.nCdVariacao == tipo);

                lstDTOVariacoes = lstProdutoVariacao.Select(v => new DTOVariacao()
                {
                    Codigo = v.nCdVariacao,
                    Nome = v.sNmVariacao ?? string.Empty,
                    Descricao = v.sDsVariacao ?? string.Empty,
                    Ativa = v.bFlAtiva,
                    Opcoes = v.VariacaoOpcoes?
                    .Select(vo => new DTOVariacaoOpcao
                    {
                        Codigo = vo.nCdVariacaoOpcao,
                        Nome = vo.sNmVariacaoOpcao ?? string.Empty,
                        Descricao = vo.sDsVariacaoOpcao ?? string.Empty,
                        Ativa = vo.bFlAtiva
                    })
                    .ToList() ?? new List<DTOVariacaoOpcao>()
                })
                .ToList();

                return lstDTOVariacoes;
            }
            catch 
            {
                throw;
            }
        }
        public async Task<List<DTOUnidadeMedida>> ObterUnidadesAtivas()
        {
            try
            {
                List<CWUnidadeMedida> lstUnidadeMedida = await _produtoRepository.PesquisarUnidadeMedidas();
                List<DTOUnidadeMedida> lstDTOUnidadeMedia = new List<DTOUnidadeMedida>();

                lstDTOUnidadeMedia.AddRange(lstUnidadeMedida
                .Where(u => u.bFlAtivo == 1)
                .Select(u => new DTOUnidadeMedida
                {
                    Codigo = u.nCdUnidadeMedida,
                    CodigoIdentificacao = u.sCdUnidadeMedida ?? string.Empty,
                    Descricao = u.sDsUnidadeMedida ?? string.Empty,
                    Sigla = u.sSgUnidadeMedida ?? string.Empty,
                    Ativo = u.bFlAtivo == 1
                })
                .ToList());

                return lstDTOUnidadeMedia;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOProdutoImagem>> ObterImagensProduto(int nCdProduto)
        {
            try
            {
                List<CWProdutoImagem> lstProdutoImagem = await _produtoRepository.PesquisarProdutoImagens(nCdProduto);
                return lstProdutoImagem.Select(imagem => new DTOProdutoImagem
                {
                    CodigoImagem = imagem.nCdImagem,
                    CodigoProduto = imagem.nCdProduto,
                    Descricao = imagem.sDsImagem ?? string.Empty,
                    Caminho = imagem.sDsCaminho ?? string.Empty
                }).ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOVariacao>> ConsultarProdutoVariacao(int nCdProduto)
        {
            try
            {
                var lstProdutoOpcaoVariacao = await _produtoRepository.ConsultarProdutoVariacao(nCdProduto);
                return lstProdutoOpcaoVariacao.Select(v => new DTOVariacao()
                {
                    Codigo = v.nCdVariacao,
                    Nome = v.sNmVariacao ?? string.Empty,
                    Descricao = v.sDsVariacao ?? string.Empty,
                    Ativa = v.bFlAtiva,
                    Opcoes = v.VariacaoOpcoes?
                    .Select(vo => new DTOVariacaoOpcao
                    {
                        Codigo = vo.nCdVariacaoOpcao,
                        Nome = vo.sNmVariacaoOpcao ?? string.Empty,
                        Descricao = vo.sDsVariacaoOpcao ?? string.Empty,
                        Atrelado = vo.bFlAtrelado
                    })
                    .ToList() ?? new List<DTOVariacaoOpcao>()
                }).ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> AdicionarImagem(int nCdProduto, IFormFile Imagem, string Descricao)
        {
            try
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

                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso };
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> AtualizarProduto(CWProduto oCWProduto)
        {
            try
            {
                await _produtoRepository.AtualizarProduto(oCWProduto);
                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                #if DEBUG
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
                #endif
                return new DTORetorno() { Mensagem = "Houve um erro não previsto ao processar sua solicitação", Status = enumSituacao.Erro };
            }
        }
        public async Task<DTORetorno> ExcluirImagem(int nCdImagem)
        {
            try
            {
                await _produtoRepository.ExcluirImagem(nCdImagem);
                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                #if DEBUG
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
                #endif
                return new DTORetorno() { Mensagem = "Houve um erro não previsto ao processar sua solicitação", Status = enumSituacao.Erro };
            }
        }
        public async Task<DTORetorno> CadastrarProduto(DTOProduto oDTOProduto)
        {
            try
            {
                CWProduto cwProduto = new CWProduto()
                {
                    nCdProduto = oDTOProduto.Codigo,
                    sNmProduto = oDTOProduto.Nome,
                    sDsProduto = oDTOProduto.Descricao,
                    dVlUnitario = oDTOProduto.ValorUnitario,
                    dVlVenda = oDTOProduto.ValorVenda,
                    nCdUnidadeMedida = oDTOProduto.CodigoUnidadeMedida,
                    sCdProduto = oDTOProduto.CodigoProduto,
                    sAltura = oDTOProduto.Altura,
                    sComprimento = oDTOProduto.Comprimento,
                    sPeso = oDTOProduto.Peso,
                    sLargura = oDTOProduto.Largura,
                    sUrlVideo = oDTOProduto.UrlVideo
                };

                int nCdProduto = await _produtoRepository.CadastrarProduto(cwProduto);
                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso , Id = nCdProduto };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                #if DEBUG
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
                #endif
                return new DTORetorno() { Mensagem = "Houve um erro não previsto ao processar sua solicitação", Status = enumSituacao.Erro };
            }
        }
        public async Task<DTORetorno> EditarVariacaoProduto(int nCdProduto, List<DTOVariacao> variacoes)
        {
            try
            {   
                List<CWProdutoOpcaoVariacao> lstProdutoOpcaoVariacao = new List<CWProdutoOpcaoVariacao>();
                foreach (var variacao in variacoes)
                {
                    foreach (var opcaoVariacao in variacao.Opcoes)
                    {
                        lstProdutoOpcaoVariacao.Add(new CWProdutoOpcaoVariacao()
                        {
                            nCdProduto = nCdProduto,
                            nCdVariacao = variacao.Codigo,
                            nCdVariacaoOpcao = opcaoVariacao?.Codigo ?? 0
                        });
                    }
                }
                await _produtoRepository.EditarVariacaoProduto(nCdProduto, lstProdutoOpcaoVariacao);
                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso, Id = nCdProduto };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                #if DEBUG
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
                #endif
                return new DTORetorno() { Mensagem = "Houve um erro não previsto ao processar sua solicitação", Status = enumSituacao.Erro };
            }
        }
        
        public async Task<DTORetorno> ExcluirProdutos(string arrCodigosProdutos)
        {
            try
            {
                List<string> lstCodigosProdutos = arrCodigosProdutos.Split(",").ToList();
                List<CWProduto> lstProdutos = new List<CWProduto>();
                List<CWProduto> lstProdutosExistentes = await _produtoRepository.PesquisarTodosProdutos();
                foreach (CWProduto produto in lstProdutosExistentes)
                {
                    if (lstCodigosProdutos.Contains(produto.nCdProduto.ToString()))
                        lstProdutos.Add(produto);
                }
                await _produtoRepository.ExcluirProdutos(lstProdutos);
                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                #if DEBUG
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
                #endif
                return new DTORetorno() { Mensagem = "Houve um erro não previsto ao processar sua solicitação", Status = enumSituacao.Erro };
            }
        }
    }
}
