using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Entities.Uteis;
using Domain.Requests;
namespace Business.Services
{
    public class PerfilService : IPerfil
    {
        private readonly IPerfilRepository _perfilRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;

        public PerfilService(IPerfilRepository PerfilRepository, IEntidadeLeituraRepository entidadeLeituraRepository)
        {
            _perfilRepository = PerfilRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
        }
        public async Task<List<DTOPerfil>> PesquisarPerfis()
        {
            try
            {
                List<DTOPerfil> lstDTOPerfis = new List<DTOPerfil>();
                List<CWPerfil> lstPerfis = await _entidadeLeituraRepository.PesquisarTodos<CWPerfil>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhum Perfil.");

                foreach (CWPerfil cw in lstPerfis)
                {
                    lstDTOPerfis.Add(new DTOPerfil
                    {
                        CodigoPerfil = cw.nCdPerfil,
                        NomePerfil = cw.sNmPerfil,
                        DescricaoPerfil = cw.sDsPerfil,
                        Ativa = cw.bFlAtiva
                    });
                }

                return lstDTOPerfis;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOPermissao>> PermissoesAssociadas(int codigoPerfil)
        {
            try
            {
                List<CWPermissaoPerfil> PermissoesPermissao = await _entidadeLeituraRepository.Pesquisar<CWPermissaoPerfil>(x => x.nCdPerfil == codigoPerfil) ?? throw new ExceptionCustom("Não foi possível localizar as Permissoes da permissão.");
                List<CWPermissao> todasPermissoes = await _entidadeLeituraRepository.PesquisarTodos<CWPermissao>() ?? throw new ExceptionCustom("Não foi possível localizar as Permissoes.");
                List<CWPermissao> PermissoesAssociadas = todasPermissoes.Where(f => PermissoesPermissao.Any(fp => fp.nCdPermissao == f.nCdPermissao)).ToList();

                List<DTOPermissao> lstDTOPermissoes = PermissoesAssociadas
                .Select(f => new DTOPermissao
                {
                    CodigoPermissao = f.nCdPermissao,
                    NomePermissao = f.sNmPermissao,
                    DescricaoPermissao = f.sDsPermissao,
                    Ativa = f.bFlAtiva
                })
                .ToList();

                return lstDTOPermissoes;
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarPerfil(DTOPerfil oDTOPerfil)
        {
            try
            {           
                CWPerfil cwPerfil = new CWPerfil()
                {
                   nCdPerfil = oDTOPerfil.CodigoPerfil, 
                   sNmPerfil = oDTOPerfil.NomePerfil,
                   sDsPerfil = oDTOPerfil.DescricaoPerfil,
                   bFlAtiva = oDTOPerfil.Ativa
                };

                CWPerfil cwPerfilRetorno = await _perfilRepository.CadastrarPerfil(cwPerfil);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Perfil cadastrada com sucesso", Id = cwPerfilRetorno.nCdPerfil };
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
        public async Task<DTORetorno> EditarPerfil(DTOPerfil oDTOPerfil)
        {
            try
            {
                CWPerfil CWPerfil = await _entidadeLeituraRepository.Consultar<CWPerfil>(x => x.nCdPerfil.Equals(oDTOPerfil.CodigoPerfil)) 
                    ?? throw new ExceptionCustom($"Não foi possível localizar o Perfil {oDTOPerfil.CodigoPerfil} para atualização.");

                CWPerfil cwPerfil = new CWPerfil()
                {
                   nCdPerfil = oDTOPerfil.CodigoPerfil, 
                   sNmPerfil = oDTOPerfil.NomePerfil,
                   sDsPerfil = oDTOPerfil.DescricaoPerfil,
                   bFlAtiva = oDTOPerfil.Ativa
                };

                CWPerfil cwPerfilRetorno = await _perfilRepository.CadastrarPerfil(cwPerfil);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Perfil atualizado com sucesso", Id = cwPerfilRetorno.nCdPerfil };
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
        public async Task<DTORetorno> InativarPerfis(string arrCodigosPerfis)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosPerfis)) throw new ExceptionCustom($"Passe pelo menos um código de Perfil.");

                List<int> lstCodigosPerfis = arrCodigosPerfis.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWPerfil> lstPerfis = new List<CWPerfil>();
                List<CWPerfil> lstPerfisExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWPerfil>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Perfil.");
                List<int> lstCodigosInvalidos = lstCodigosPerfis.Except(lstPerfisExistentes.Select(x => x.nCdPerfil)).ToList();

                foreach (CWPerfil produto in lstPerfisExistentes)
                {
                    if (lstCodigosPerfis.Contains(produto.nCdPerfil)) lstPerfis.Add(produto);
                }

                await _perfilRepository.InativarPerfis(lstPerfis);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de perfis não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
        public async Task<DTORetorno> AtivarPerfis(string arrCodigosPerfis)
        {
            try
            {
                if(string.IsNullOrEmpty(arrCodigosPerfis)) throw new ExceptionCustom($"Passe pelo menos um código de Perfil.");

                List<int> lstCodigosPerfis = arrCodigosPerfis.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para remoção.");
                }).ToList();                   
                
                List<CWPerfil> lstPerfis = new List<CWPerfil>();
                List<CWPerfil> lstPerfisExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWPerfil>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Perfil.");
                List<int> lstCodigosInvalidos = lstCodigosPerfis.Except(lstPerfisExistentes.Select(x => x.nCdPerfil)).ToList();

                foreach (CWPerfil produto in lstPerfisExistentes)
                {
                    if (lstCodigosPerfis.Contains(produto.nCdPerfil)) lstPerfis.Add(produto);
                }

                await _perfilRepository.AtivarPerfis(lstPerfis);

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
        public async Task<DTORetorno> AssociarPermissoes(AssociacaoRequest associacaoRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(associacaoRequest.CodigosAssociacao)) throw new ExceptionCustom($"Passe pelo menos um código de Permissao.");

                List<int> lstCodigosPermissoes = associacaoRequest.CodigosAssociacao.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para associação.");
                }).ToList();                   
                
                List<CWPermissao> lstPermissoes = new List<CWPermissao>();
                List<CWPermissao> lstPermissoesExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWPermissao>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Permissao.");
                List<int> lstCodigosInvalidos = lstCodigosPermissoes.Except(lstPermissoesExistentes.Select(x => x.nCdPermissao)).ToList();

                foreach (CWPermissao Permissao in lstPermissoesExistentes)
                {
                    if (lstCodigosPermissoes.Contains(Permissao.nCdPermissao)) lstPermissoes.Add(Permissao);
                }

                await _perfilRepository.AssociarPermissoes(associacaoRequest.Codigo, lstPermissoes);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de permissoes não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
        public async Task<DTORetorno> DesassociarPermissoes(AssociacaoRequest associacaoRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(associacaoRequest.CodigosAssociacao)) throw new ExceptionCustom($"Passe pelo menos um código de Permissao.");

                List<int> lstCodigosPermissoes = associacaoRequest.CodigosAssociacao.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para associação.");
                }).ToList();

                List<CWPermissaoPerfil> lstPermissaoPermissao = await _entidadeLeituraRepository.Pesquisar<CWPermissaoPerfil>(x => x.nCdPermissao == associacaoRequest.Codigo) ?? throw new ExceptionCustom($"Permissao {associacaoRequest.Codigo} não localizada no sistema.");
                List<CWPermissaoPerfil> lstPermissoesPermissaoRemover = new List<CWPermissaoPerfil>();
                List<int> lstCodigosInvalidos = lstCodigosPermissoes.Except(lstPermissaoPermissao.Select(x => x.nCdPermissao)).ToList();

                foreach (CWPermissaoPerfil Permissao in lstPermissaoPermissao)
                {
                    if (lstCodigosPermissoes.Contains(Permissao.nCdPermissao)) lstPermissoesPermissaoRemover.Add(Permissao);
                }

                await _perfilRepository.DesassociarPermissoes(lstPermissoesPermissaoRemover);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("As seguintes permissões não existem nesse perfil: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
