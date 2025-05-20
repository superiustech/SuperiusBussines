using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Entities.Uteis;
using Domain.Requests;
namespace Business.Services
{
    public class PermissaoService : IPermissao
    {
        private readonly IPermissaoRepository _permissaoRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;

        public PermissaoService(IPermissaoRepository permissaoRepository, IEntidadeLeituraRepository entidadeLeituraRepository)
        {
            _permissaoRepository = permissaoRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
        }
        public async Task<List<DTOPermissao>> PesquisarPermissoes()
        {
            try
            {
                List<DTOPermissao> lstDTOPermissoes = new List<DTOPermissao>();
                List<CWPermissao> lstPermissoes = await _entidadeLeituraRepository.PesquisarTodos<CWPermissao>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Permissao.");

                foreach (CWPermissao cw in lstPermissoes)
                {
                    lstDTOPermissoes.Add(new DTOPermissao
                    {
                        CodigoPermissao = cw.nCdPermissao,
                        NomePermissao = cw.sNmPermissao,
                        DescricaoPermissao = cw.sDsPermissao,
                        Ativa = cw.bFlAtiva
                    });
                }

                return lstDTOPermissoes;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOFuncionalidade>> FuncionalidadesAssociadas(int codigoPermissao)
        {
            try
            {
                List<CWFuncionalidadePermissao> funcionalidadesPermissao = await _entidadeLeituraRepository.Pesquisar<CWFuncionalidadePermissao>(x => x.nCdPermissao == codigoPermissao)?? throw new ExceptionCustom("Não foi possível localizar as funcionalidades da permissão.");
                List<CWFuncionalidade> todasFuncionalidades = await _entidadeLeituraRepository.PesquisarTodos<CWFuncionalidade>()?? throw new ExceptionCustom("Não foi possível localizar as funcionalidades.");
                List<CWFuncionalidade> funcionalidadesAssociadas = todasFuncionalidades.Where(f => funcionalidadesPermissao.Any(fp => fp.nCdFuncionalidade == f.nCdFuncionalidade)).ToList();
                
                List<DTOFuncionalidade> lstDTOFuncionalidades = funcionalidadesAssociadas
                .Select(f => new DTOFuncionalidade
                {
                    CodigoFuncionalidade = f.nCdFuncionalidade,
                    NomeFuncionalidade = f.sNmFuncionalidade,
                    DescricaoFuncionalidade = f.sDsFuncionalidade,
                    Ativa = f.bFlAtiva
                })
                .ToList();

                return lstDTOFuncionalidades;
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarPermissao(DTOPermissao oDTOPermissao)
        {
            try
            {           
                CWPermissao cwPermissao = new CWPermissao()
                {
                   nCdPermissao = oDTOPermissao.CodigoPermissao, 
                   sNmPermissao = oDTOPermissao.NomePermissao,
                   sDsPermissao = oDTOPermissao.DescricaoPermissao,
                   bFlAtiva = oDTOPermissao.Ativa
                };

                CWPermissao cwPermissaoRetorno = await _permissaoRepository.CadastrarPermissao(cwPermissao);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Permissao cadastrada com sucesso", Id = cwPermissaoRetorno.nCdPermissao };
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
        public async Task<DTORetorno> EditarPermissao(DTOPermissao oDTOPermissao)
        {
            try
            {
                CWPermissao CWPermissao = await _entidadeLeituraRepository.Consultar<CWPermissao>(x => x.nCdPermissao.Equals(oDTOPermissao.CodigoPermissao)) 
                    ?? throw new ExceptionCustom($"Não foi possível localizar a Permissao {oDTOPermissao.CodigoPermissao} para atualização.");

                CWPermissao cwPermissao = new CWPermissao()
                {
                   nCdPermissao = oDTOPermissao.CodigoPermissao, 
                   sNmPermissao = oDTOPermissao.NomePermissao,
                   sDsPermissao = oDTOPermissao.DescricaoPermissao,
                   bFlAtiva = oDTOPermissao.Ativa
                };

                CWPermissao cwPermissaoRetorno = await _permissaoRepository.CadastrarPermissao(cwPermissao);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Permissao atualizada com sucesso", Id = cwPermissaoRetorno.nCdPermissao };
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
        public async Task<DTORetorno> InativarPermissoes(string arrCodigosPermissoes)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosPermissoes)) throw new ExceptionCustom($"Passe pelo menos um código de Permissao.");

                List<int> lstCodigosPermissoes = arrCodigosPermissoes.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWPermissao> lstPermissoes = new List<CWPermissao>();
                List<CWPermissao> lstPermissoesExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWPermissao>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Permissao.");
                List<int> lstCodigosInvalidos = lstCodigosPermissoes.Except(lstPermissoesExistentes.Select(x => x.nCdPermissao)).ToList();

                foreach (CWPermissao produto in lstPermissoesExistentes)
                {
                    if (lstCodigosPermissoes.Contains(produto.nCdPermissao)) lstPermissoes.Add(produto);
                }

                await _permissaoRepository.InativarPermissoes(lstPermissoes);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de permissões não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
        public async Task<DTORetorno> AtivarPermissoes(string arrCodigosPermissoes)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosPermissoes)) throw new ExceptionCustom($"Passe pelo menos um código de Permissao.");

                List<int> lstCodigosPermissoes = arrCodigosPermissoes.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWPermissao> lstPermissoes = new List<CWPermissao>();
                List<CWPermissao> lstPermissoesExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWPermissao>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Permissao.");
                List<int> lstCodigosInvalidos = lstCodigosPermissoes.Except(lstPermissoesExistentes.Select(x => x.nCdPermissao)).ToList();

                foreach (CWPermissao produto in lstPermissoesExistentes)
                {
                    if (lstCodigosPermissoes.Contains(produto.nCdPermissao)) lstPermissoes.Add(produto);
                }

                await _permissaoRepository.AtivarPermissoes(lstPermissoes);

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
        public async Task<DTORetorno> AssociarFuncionalidades(AssociacaoRequest associacaoRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(associacaoRequest.CodigosAssociacao)) throw new ExceptionCustom($"Passe pelo menos um código de funcionalidade.");

                List<int> lstCodigosFuncionalidades = associacaoRequest.CodigosAssociacao.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para associação.");
                }).ToList();                   
                
                List<CWFuncionalidade> lstFuncionalidades = new List<CWFuncionalidade>();
                List<CWFuncionalidade> lstFuncionalidadesExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWFuncionalidade>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Permissao.");
                List<int> lstCodigosInvalidos = lstCodigosFuncionalidades.Except(lstFuncionalidadesExistentes.Select(x => x.nCdFuncionalidade)).ToList();

                foreach (CWFuncionalidade funcionalidade in lstFuncionalidadesExistentes)
                {
                    if (lstCodigosFuncionalidades.Contains(funcionalidade.nCdFuncionalidade)) lstFuncionalidades.Add(funcionalidade);
                }

                await _permissaoRepository.AssociarFuncionalidades(associacaoRequest.Codigo, lstFuncionalidades);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de funcionalidades não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
        public async Task<DTORetorno> DesassociarFuncionalidades(AssociacaoRequest associacaoRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(associacaoRequest.CodigosAssociacao)) throw new ExceptionCustom($"Passe pelo menos um código de funcionalidade.");

                List<int> lstCodigosFuncionalidades = associacaoRequest.CodigosAssociacao.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para associação.");
                }).ToList();

                List<CWFuncionalidadePermissao> lstFuncionalidadePermissao = await _entidadeLeituraRepository.Pesquisar<CWFuncionalidadePermissao>(x => x.nCdPermissao == associacaoRequest.Codigo) ?? throw new ExceptionCustom($"Permissao {associacaoRequest.Codigo} não localizada no sistema.");
                List<CWFuncionalidadePermissao> lstFuncionalidadesPermissaoRemover = new List<CWFuncionalidadePermissao>();
                List<int> lstCodigosInvalidos = lstCodigosFuncionalidades.Except(lstFuncionalidadePermissao.Select(x => x.nCdFuncionalidade)).ToList();

                foreach (CWFuncionalidadePermissao funcionalidade in lstFuncionalidadePermissao)
                {
                    if (lstCodigosFuncionalidades.Contains(funcionalidade.nCdFuncionalidade)) lstFuncionalidadesPermissaoRemover.Add(funcionalidade);
                }

                await _permissaoRepository.DesassociarFuncionalidades(lstFuncionalidadesPermissaoRemover);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("As seguintes funcionalidades não existem nessa permissão: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
    }
}
