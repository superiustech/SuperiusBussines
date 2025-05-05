using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Uteis;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Business.Services
{
    public class RevendedorService : IRevendedor
    {
        private readonly IRevendedorRepository _revendedorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RevendedorService(IRevendedorRepository revendedorRepository, IHttpContextAccessor httpContextAccessor)
        {
            _revendedorRepository = revendedorRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<DTORevendedor>> PesquisarRevendedores()
        {
            try
            {
                var revendedores = await _revendedorRepository.PesquisarRevendedores();
                return revendedores.Select(r => new DTORevendedor
                {
                    Codigo = r.nCdRevendedor,
                    Estoque = r.Estoque?.sNmEstoque ?? string.Empty,
                    TipoRevendedor = r.Tipo?.sNmTipo ?? string.Empty,
                    Nome = r.sNmRevendedor ?? string.Empty,
                    PercentualRevenda = r.dPcRevenda,
                    CpfCnpj = r.sNrCpfCnpj ?? string.Empty,
                    Telefone = r.sTelefone ?? string.Empty,
                    Email = r.sEmail ?? string.Empty,
                    Rua = r.sDsRua ?? string.Empty,
                    Complemento = r.sDsComplemento ?? string.Empty,
                    Numero = r.sNrNumero ?? string.Empty,
                    Cep = r.sCdCep ?? string.Empty
                }).ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarRevendedor(DTORevendedor oDTORevendedor)
        {
            try
            {
                CWRevendedor oCWRevendedor = new CWRevendedor()
                {
                    nCdRevendedor = oDTORevendedor.Codigo,
                    nCdEstoque = oDTORevendedor.CodigoEstoque,
                    nCdTipoRevendedor = oDTORevendedor.CodigoTipoRevendedor,
                    sNmRevendedor = oDTORevendedor.Nome ?? string.Empty,
                    dPcRevenda = oDTORevendedor.PercentualRevenda,
                    sNrCpfCnpj = oDTORevendedor.CpfCnpj ?? string.Empty,
                    sTelefone = oDTORevendedor.Telefone ?? string.Empty,
                    sEmail = oDTORevendedor.Email ?? string.Empty,
                    sDsRua = oDTORevendedor.Rua ?? string.Empty,
                    sDsComplemento = oDTORevendedor.Complemento ?? string.Empty,
                    sNrNumero = oDTORevendedor.Numero ?? string.Empty,
                    sCdCep = oDTORevendedor.Cep ?? string.Empty
                };

                await _revendedorRepository.CadastrarRevendedor(oCWRevendedor);
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
        public async Task<DTORevendedor> Consultar(int nCdRevendedor)
        {
            try
            {
                var revendedor = await _revendedorRepository.Consultar(nCdRevendedor) ?? throw new ExceptionCustom($"Revendedor cod. {nCdRevendedor} não localizado no sitema.");
                return new DTORevendedor()
                {
                    Codigo = revendedor.nCdRevendedor,
                    CodigoEstoque = revendedor.nCdEstoque,
                    CodigoTipoRevendedor = revendedor.nCdTipoRevendedor,
                    Nome = revendedor.sNmRevendedor ?? string.Empty,
                    PercentualRevenda = revendedor.dPcRevenda,
                    CpfCnpj = revendedor.sNrCpfCnpj ?? string.Empty,
                    Telefone = revendedor.sTelefone ?? string.Empty,
                    Email = revendedor.sEmail ?? string.Empty,
                    Rua = revendedor.sDsRua ?? string.Empty,
                    Complemento = revendedor.sDsComplemento ?? string.Empty,
                    Numero = revendedor.sNrNumero ?? string.Empty,
                    Cep = revendedor.sCdCep ?? string.Empty
                };
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOTipoRevendedor>> PesquisarTipos()
        {
            try
            {
                var tiposRevendedor = await _revendedorRepository.PesquisarTipos();

                return tiposRevendedor.Select(t => new DTOTipoRevendedor
                {
                    Codigo = t.nCdTipoRevendedor,
                    Descricao = t.sDsTipo ?? string.Empty,
                    Nome = t.sNmTipo ?? string.Empty,
                    Ativo = t.bFlAtivo == 1 
                }).ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> ExcluirRevendedores(string arrCodigosRevendedores)
        {
            try
            {
                if (string.IsNullOrEmpty(arrCodigosRevendedores)) throw new ExceptionCustom($"Favor, preencher quais revendedores devem ser removidos.");

                List<int> lstCodigosRevendedores = arrCodigosRevendedores.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();

                List<CWRevendedor> lstRevendedores = new List<CWRevendedor>();
                List<CWRevendedor> lstRevendedoresExistentes = await _revendedorRepository.PesquisarRevendedoresSimples();
                List<int> lstCodigosInvalidos = lstCodigosRevendedores.Except(lstRevendedoresExistentes.Select(x => x.nCdRevendedor)).ToList();

                foreach (CWRevendedor oCWRevendedor in lstRevendedoresExistentes)
                {
                    if (lstCodigosRevendedores.Contains(oCWRevendedor.nCdRevendedor)) lstRevendedores.Add(oCWRevendedor);
                }

                await _revendedorRepository.ExcluirRevendedores(lstRevendedores);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de revendedor não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
                }

                return new DTORetorno() { Mensagem = "Revendedor(es) excluido(s) com sucesso.", Status = enumSituacao.Sucesso };
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
