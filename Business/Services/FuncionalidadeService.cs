using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Entities.Uteis;
using Infra.Repositories;

namespace Business.Services
{
    public class FuncionalidadeService : IFuncionalidade
    {
        private readonly IFuncionalidadeRepository _funcionalidadeRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;

        public FuncionalidadeService(IFuncionalidadeRepository funcionalidadeRepository, IEntidadeLeituraRepository entidadeLeituraRepository)
        {
            _funcionalidadeRepository = funcionalidadeRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
        }
        public async Task<List<DTOFuncionalidade>> PesquisarFuncionalidades()
        {
            try
            {
                List<DTOFuncionalidade> lstDTOFuncionalidades = new List<DTOFuncionalidade>();
                List<CWFuncionalidade> lstFuncionalidades = await _entidadeLeituraRepository.PesquisarTodos<CWFuncionalidade>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma funcionalidade.");

                foreach (CWFuncionalidade cw in lstFuncionalidades)
                {
                    lstDTOFuncionalidades.Add(new DTOFuncionalidade
                    {
                        CodigoFuncionalidade = cw.nCdFuncionalidade,
                        NomeFuncionalidade = cw.sNmFuncionalidade,
                        DescricaoFuncionalidade = cw.sDsFuncionalidade,
                        Ativa = cw.bFlAtiva
                    });
                }

                lstDTOFuncionalidades.OrderBy(x => x.CodigoFuncionalidade);

                return lstDTOFuncionalidades;
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarFuncionalidade(DTOFuncionalidade oDTOFuncionalidade)
        {
            try
            {           
                CWFuncionalidade cwFuncionalidade = new CWFuncionalidade()
                {
                   nCdFuncionalidade = oDTOFuncionalidade.CodigoFuncionalidade, 
                   sNmFuncionalidade = oDTOFuncionalidade.NomeFuncionalidade,
                   sDsFuncionalidade = oDTOFuncionalidade.DescricaoFuncionalidade,
                   bFlAtiva = oDTOFuncionalidade.Ativa
                };

                CWFuncionalidade cwFuncionalidadeRetorno = await _funcionalidadeRepository.CadastrarFuncionalidade(cwFuncionalidade);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Funcionalidade cadastrada com sucesso", Id = cwFuncionalidadeRetorno.nCdFuncionalidade };
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
        public async Task<DTORetorno> EditarFuncionalidade(DTOFuncionalidade oDTOFuncionalidade)
        {
            try
            {
                CWFuncionalidade CWFuncionalidade = await _entidadeLeituraRepository.Consultar<CWFuncionalidade>(x => x.nCdFuncionalidade.Equals(oDTOFuncionalidade.CodigoFuncionalidade)) 
                    ?? throw new ExceptionCustom($"Não foi possível localizar a funcionalidade {oDTOFuncionalidade.CodigoFuncionalidade} para atualização.");

                CWFuncionalidade cwFuncionalidade = new CWFuncionalidade()
                {
                   nCdFuncionalidade = oDTOFuncionalidade.CodigoFuncionalidade, 
                   sNmFuncionalidade = oDTOFuncionalidade.NomeFuncionalidade,
                   sDsFuncionalidade = oDTOFuncionalidade.DescricaoFuncionalidade,
                   bFlAtiva = oDTOFuncionalidade.Ativa
                };

                CWFuncionalidade cwFuncionalidadeRetorno = await _funcionalidadeRepository.CadastrarFuncionalidade(cwFuncionalidade);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Funcionalidade atualizada com sucesso", Id = cwFuncionalidadeRetorno.nCdFuncionalidade };
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
        public async Task<DTORetorno> InativarFuncionalidades(string arrCodigosFuncionalidades)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosFuncionalidades)) throw new ExceptionCustom($"Passe pelo menos um código de funcionalidade.");

                List<int> lstCodigosFuncionalidades = arrCodigosFuncionalidades.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWFuncionalidade> lstFuncionalidades = new List<CWFuncionalidade>();
                List<CWFuncionalidade> lstFuncionalidadesExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWFuncionalidade>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma funcionalidade.");
                List<int> lstCodigosInvalidos = lstCodigosFuncionalidades.Except(lstFuncionalidadesExistentes.Select(x => x.nCdFuncionalidade)).ToList();

                foreach (CWFuncionalidade produto in lstFuncionalidadesExistentes)
                {
                    if (lstCodigosFuncionalidades.Contains(produto.nCdFuncionalidade)) lstFuncionalidades.Add(produto);
                }

                await _funcionalidadeRepository.InativarFuncionalidades(lstFuncionalidades);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de estoque não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
                }

                return new DTORetorno() { Mensagem = "Funcionalidades inativadas com sucesso!", Status = enumSituacao.Sucesso };
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
        public async Task<DTORetorno> AtivarFuncionalidades(string arrCodigosFuncionalidades)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosFuncionalidades)) throw new ExceptionCustom($"Passe pelo menos um código de funcionalidade.");

                List<int> lstCodigosFuncionalidades = arrCodigosFuncionalidades.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWFuncionalidade> lstFuncionalidades = new List<CWFuncionalidade>();
                List<CWFuncionalidade> lstFuncionalidadesExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWFuncionalidade>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma funcionalidade.");
                List<int> lstCodigosInvalidos = lstCodigosFuncionalidades.Except(lstFuncionalidadesExistentes.Select(x => x.nCdFuncionalidade)).ToList();

                foreach (CWFuncionalidade produto in lstFuncionalidadesExistentes)
                {
                    if (lstCodigosFuncionalidades.Contains(produto.nCdFuncionalidade)) lstFuncionalidades.Add(produto);
                }

                await _funcionalidadeRepository.AtivarFuncionalidades(lstFuncionalidades);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de estoque não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
                }

                return new DTORetorno() { Mensagem = "Funcionalidades ativadas com sucesso!", Status = enumSituacao.Sucesso };
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
