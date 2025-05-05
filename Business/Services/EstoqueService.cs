using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Uteis;
using Domain.Entities.ViewModel;
using Domain.Interfaces;
using Domain.Requests;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Xml;

namespace Business.Services
{
    public class EstoqueService : IEstoque
    {
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EstoqueService(IEstoqueRepository estoqueRepository, IHttpContextAccessor httpContextAccessor)
        {
            _estoqueRepository = estoqueRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<DTOEstoque> Consultar(int nCdEstoque)
        {
            try
            {
                var estoque = await _estoqueRepository.Consultar(nCdEstoque) ?? throw new ExceptionCustom($"Estoque cod. {nCdEstoque} não localizado no sistema.");
                DTOEstoque oDTOEstoque = new DTOEstoque()
                {
                    Codigo = estoque.nCdEstoque,
                    CodigoIdentificacao = estoque.sCdEstoque,
                    Nome = estoque.sNmEstoque,
                    Descricao = estoque.sDsEstoque,
                    Cep = estoque.sCdCep,
                    Rua = estoque.sDsRua,
                    Numero = estoque.sNrNumero,
                    Complemento = estoque.sDsComplemento
                };

                return oDTOEstoque;
            }
            catch
            {
                throw;

            }
        }
        public async Task<List<DTOEstoque>> PesquisarEstoques(int? nCdRevendedor = null)
        {
            try
            {
                var estoques = await _estoqueRepository.PesquisarSemRevendedores(nCdRevendedor);
                List<DTOEstoque> lstEstoqueDTO = new List<DTOEstoque>();

                lstEstoqueDTO.AddRange(estoques.Select(x => new DTOEstoque()
                {
                    Codigo = x.nCdEstoque,
                    CodigoIdentificacao = x.sCdEstoque,
                    Nome = x.sNmEstoque,
                    Descricao = x.sDsEstoque,
                    Rua = x.sDsRua,
                    Complemento = x.sDsComplemento,
                    Numero = x.sNrNumero,
                    Cep = x.sCdCep
                }).ToList());

                return lstEstoqueDTO;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOEstoque>> PesquisarTodosEstoques()
        {
            try { 
                var estoques = await _estoqueRepository.PesquisarTodosEstoques();
                List<DTOEstoque> lstEstoqueDTO = new List<DTOEstoque>();

                lstEstoqueDTO.AddRange(estoques.Select(x => new DTOEstoque()
                {
                    Codigo = x.nCdEstoque,
                    CodigoIdentificacao = x.sCdEstoque,
                    Nome = x.sNmEstoque,
                    Descricao = x.sDsEstoque,
                    Rua = x.sDsRua,
                    Complemento = x.sDsComplemento,
                    Numero = x.sNrNumero,
                    Cep = x.sCdCep
                }).ToList());

                return lstEstoqueDTO;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOEstoqueProdutoHistorico>> ConsultarHistorico(int nCdEstoque)
        {
            try
            {
                List<DTOEstoqueProdutoHistorico> lstHistoricoEstoque = new List<DTOEstoqueProdutoHistorico>();
                var historico = await _estoqueRepository.ConsultarHistorico(nCdEstoque) ?? throw new ExceptionCustom($"Estoque cod. {nCdEstoque} não localizado no sistema.");
                lstHistoricoEstoque.AddRange(historico.Select(x => new DTOEstoqueProdutoHistorico()
                {
                    Codigo = x.nCdEstoqueProdutoHistorico,
                    CodigoProduto = x.nCdProduto,
                    CodigoEstoqueOrigem = x.nCdEstoque,
                    CodigoEstoqueDestino = x.nCdEstoqueDestino,
                    TipoMovimentacao = ObterStringTipoMovimentacao(x.nTipoMovimentacao),
                    DataMovimentacao = x.tDtMovimentacao?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                    QuantidadeMovimentada = x.dQtMovimentada,
                    Observacao = x.sDsObservacao ?? string.Empty,
                    EstoqueOrigem = x.EstoqueOrigem?.sNmEstoque ?? string.Empty,
                    EstoqueDestino = x.EstoqueDestino?.sNmEstoque ?? string.Empty,
                    Produto = x.Produto?.sNmProduto ?? string.Empty
                }));
                return lstHistoricoEstoque;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOEstoqueProduto>> PesquisarEstoqueProduto(int nCdEstoque)
        {
            try
            {
                List<DTOEstoqueProduto> lstEstoqueProdutoDTO = new List<DTOEstoqueProduto>();
                var estoque = await _estoqueRepository.Consultar(nCdEstoque) ?? throw new ExceptionCustom($"Estoque cod. {nCdEstoque} não localizado no sistema1.");

                lstEstoqueProdutoDTO.AddRange(estoque.Produtos.Select(x => new DTOEstoqueProduto()
                {
                    CodigoProduto = x.nCdProduto,
                    QuantidadeMinima = x.dQtMinima,
                    QuantidadeEstoque = x.dQtEstoque,
                    ValorVenda = x.dVlVenda,
                    ValorCusto = x.dVlCusto,
                    NomeProduto = x.Produto?.sNmProduto ?? string.Empty,
                    DescricaoProduto = x.Produto?.sDsProduto ?? string.Empty
                }).ToList());

                return lstEstoqueProdutoDTO;
            }
            catch
            {
                throw;

            }
        }
        public async Task<DTORetorno> CadastrarEstoque(DTOEstoque oDTOEstoque)
        {
            try
            {
                CWEstoque oCWEstoque = new CWEstoque()
                {
                    nCdEstoque = oDTOEstoque.Codigo,
                    sNmEstoque = oDTOEstoque.Nome,
                    sDsEstoque = oDTOEstoque.Descricao,
                    sCdEstoque = oDTOEstoque.CodigoIdentificacao,
                    sDsRua = oDTOEstoque.Rua,
                    sDsComplemento = oDTOEstoque.Complemento,
                    sNrNumero = oDTOEstoque.Numero,
                    sCdCep = oDTOEstoque.Cep
                };

                await _estoqueRepository.CadastrarEstoque(oCWEstoque);

                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso, Id = Convert.ToInt64(oCWEstoque.nCdEstoque) };
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
        public async Task<DTORetorno> MovimentarEntradaSaida(DTOEstoqueProdutoHistorico oDTOEstoqueProdutoHistorico)
        {
            try
            {
                CWEstoqueProdutoHistorico cwEstoqueProdutoHistorico = new CWEstoqueProdutoHistorico()
                {
                    nCdEstoque = oDTOEstoqueProdutoHistorico.CodigoEstoqueDestino,
                    nCdEstoqueDestino = oDTOEstoqueProdutoHistorico.CodigoEstoqueDestino,
                    nCdProduto = oDTOEstoqueProdutoHistorico.CodigoProduto,
                    dQtMovimentada = oDTOEstoqueProdutoHistorico.QuantidadeMovimentada,
                    nTipoMovimentacao = (nTipoMovimentacao) Convert.ToInt32(oDTOEstoqueProdutoHistorico.TipoMovimentacao)
                };

                CWEstoqueProduto cwEstoqueProduto = new CWEstoqueProduto();
                cwEstoqueProduto.nCdEstoque = oDTOEstoqueProdutoHistorico.CodigoEstoqueDestino;
                cwEstoqueProduto.nCdProduto = oDTOEstoqueProdutoHistorico.CodigoProduto;
                cwEstoqueProduto = await _estoqueRepository.CarregarDadosProdutoEEstoque(cwEstoqueProduto);

                _estoqueRepository.DefinirValorVenda(cwEstoqueProduto, await _estoqueRepository.ObterPercentualRevenda(cwEstoqueProdutoHistorico.nCdEstoque));

                MovimentarQuantidade(cwEstoqueProduto, cwEstoqueProdutoHistorico);
                await _estoqueRepository.MovimentarEntradaSaida(cwEstoqueProduto, cwEstoqueProdutoHistorico);

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
        public async Task<DTORetorno> RemoverEstoqueProduto(int codigoEstoque, string arrCodigosProdutos)
        {
            try
            {
                if (string.IsNullOrEmpty(arrCodigosProdutos)) throw new ExceptionCustom($"Favor, preencher quais estoques devem ser removidos.");

                List<int> lstCodigosProdutos = arrCodigosProdutos.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();

                CWEstoque cwEstoque = await _estoqueRepository.Consultar(codigoEstoque) ?? throw new ExceptionCustom($"Estoque cod. {codigoEstoque} não localizado no sistema1.");
                List<CWEstoqueProduto> lstEstoqueProdutos = new List<CWEstoqueProduto>();                
                List<CWEstoqueProduto> lstEstoquesExistentes = cwEstoque.Produtos.ToList();
                List<int> lstCodigosInvalidos = lstCodigosProdutos.Except(lstEstoquesExistentes.Select(x => x.nCdProduto)).ToList();

                foreach (CWEstoqueProduto estoque in lstEstoquesExistentes)
                {
                    if (lstCodigosProdutos.Contains(estoque.nCdProduto)) lstEstoqueProdutos.Add(estoque);
                }

                await _estoqueRepository.RemoverEstoqueProduto(lstEstoqueProdutos);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes produtos não existem no estoque: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
                }

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
        public async Task<DTORetorno> AdicionarEditarProdutoEstoque(DTOEstoqueProduto DTOEstoqueProduto)
        {
            try
            {
                if(DTOEstoqueProduto.CodigoEstoque <= 0) throw new ExceptionCustom($"Preencha o código do estoque.");

                CWEstoqueProduto cwEstoqueProduto = new CWEstoqueProduto()
                {
                    nCdEstoque = DTOEstoqueProduto.CodigoEstoque,
                    nCdProduto = DTOEstoqueProduto.CodigoProduto,
                    dQtMinima = DTOEstoqueProduto.QuantidadeMinima,
                    dQtEstoque = DTOEstoqueProduto.QuantidadeEstoque,
                    dVlVenda = DTOEstoqueProduto.ValorVenda,
                    dVlCusto = DTOEstoqueProduto.ValorCusto,
                    bFlAtivo = true
                };

                await _estoqueRepository.AdicionarEditarProdutoEstoque(cwEstoqueProduto);

                return new DTORetorno() { Mensagem = "Sucesso", Status = enumSituacao.Sucesso , Id = DTOEstoqueProduto.CodigoEstoque};
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
        public async Task<DTORetorno> ExcluirEstoques(string arrCodigosEstoques)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosEstoques)) throw new ExceptionCustom($"Favor, preencher quais estoques devem ser removidos.");

                List<int> lstCodigosEstoques = arrCodigosEstoques.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWEstoque> lstEstoques = new List<CWEstoque>();
                List<CWEstoque> lstEstoquesExistentes = await _estoqueRepository.PesquisarTodosEstoques();
                List<int> lstCodigosInvalidos = lstCodigosEstoques.Except(lstEstoquesExistentes.Select(x => x.nCdEstoque)).ToList();

                foreach (CWEstoque produto in lstEstoquesExistentes)
                {
                    if (lstCodigosEstoques.Contains(produto.nCdEstoque)) lstEstoques.Add(produto);
                }

                await _estoqueRepository.ExcluirEstoques(lstEstoques);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de estoque não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
                }

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
        private void MovimentarQuantidade(CWEstoqueProduto cwEstoqueProduto, CWEstoqueProdutoHistorico cwEstoqueProdutoHistorico)
        {
            if (cwEstoqueProdutoHistorico.nTipoMovimentacao == nTipoMovimentacao.Entrada)
            {
                cwEstoqueProduto.dQtEstoque += cwEstoqueProdutoHistorico.dQtMovimentada;
                cwEstoqueProdutoHistorico.sDsObservacao = "Entrada manual ao estoque";
            }
            else if (cwEstoqueProdutoHistorico.nTipoMovimentacao == nTipoMovimentacao.Saida)
            {
                if (cwEstoqueProdutoHistorico.dQtMovimentada > cwEstoqueProduto.dQtEstoque)
                {
                    throw new ExceptionCustom($"Quantidade insuficiente no estoque para realizar a saída.");
                }
                else
                {
                    cwEstoqueProduto.dQtEstoque -= cwEstoqueProdutoHistorico.dQtMovimentada;
                    cwEstoqueProdutoHistorico.sDsObservacao = "Saída manual do estoque";
                }
            }
        }
        public string ObterStringTipoMovimentacao(nTipoMovimentacao nTipo)
        {
            switch (nTipo)
            {
                case nTipoMovimentacao.Entrada:
                    return "Entrada";
                    break;
                case nTipoMovimentacao.Saida:
                    return "Saída";
                    break;
                case nTipoMovimentacao.Transferencia:
                    return "Transferencia";
                    break;
                case nTipoMovimentacao.Devolucao:
                    return "Devolução";
                    break;
                default:
                    return "N/A";
            }
        }

    }
    
}
