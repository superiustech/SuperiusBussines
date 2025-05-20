using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Entities.Uteis;
using Domain.Requests;
namespace Business.Services
{
    public class UsuarioService : IUsuario
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IFuncionalidadeRepository _funcionalidadeRepository;

        public UsuarioService(IUsuarioRepository UsuarioRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IFuncionalidadeRepository funcionalidadeRepository)
        {
            _usuarioRepository = UsuarioRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _funcionalidadeRepository = funcionalidadeRepository;
        }
        public async Task<List<DTOUsuario>> PesquisarUsuarios()
        {
            try
            {
                List<DTOUsuario> lstDTOUsuarios = new List<DTOUsuario>();
                List<CWUsuario> lstUsuarios = await _entidadeLeituraRepository.PesquisarTodos<CWUsuario>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhum Usuario.");

                foreach (CWUsuario cw in lstUsuarios)
                {
                    lstDTOUsuarios.Add(new DTOUsuario
                    {
                        Usuario = cw.sCdUsuario,
                        NomeUsuario = cw.sNmUsuario,
                        Senha = cw.sSenha,
                        Email = cw.sEmail
                    });
                }

                return lstDTOUsuarios;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOFuncionalidade>> FuncionalidadesUsuario(string sCdUsuario)
        {
            try
            {
                List<DTOFuncionalidade> lstDTOFuncionalidades = new List<DTOFuncionalidade>();
                List<CWFuncionalidade> lstFuncionalidades = await _funcionalidadeRepository.FuncionalidadesUsuario(sCdUsuario) ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma funcionalidade para o usuário.");

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

                return lstDTOFuncionalidades;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOPerfil>> PerfisAssociados(string codigoUsuario)
        {
            try
            {
                List<CWPerfilUsuario> PerfisPerfil = await _entidadeLeituraRepository.Pesquisar<CWPerfilUsuario>(x => x.sCdUsuario == codigoUsuario) ?? throw new ExceptionCustom("Não foi possível localizar as Perfis da permissão.");
                List<CWPerfil> todasPerfis = await _entidadeLeituraRepository.PesquisarTodos<CWPerfil>() ?? throw new ExceptionCustom("Não foi possível localizar as Perfis.");
                List<CWPerfil> PerfisAssociadas = todasPerfis.Where(f => PerfisPerfil.Any(fp => fp.nCdPerfil == f.nCdPerfil)).ToList();

                List<DTOPerfil> lstDTOPerfis = PerfisAssociadas
                .Select(f => new DTOPerfil
                {
                    CodigoPerfil = f.nCdPerfil,
                    NomePerfil = f.sNmPerfil,
                    DescricaoPerfil = f.sDsPerfil,
                    Ativa = f.bFlAtiva
                })
                .ToList();

                return lstDTOPerfis;
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarUsuario(DTOUsuario oDTOUsuario)
        {
            try
            {           
                CWUsuario cwUsuario = new CWUsuario()
                {
                   sCdUsuario = oDTOUsuario.Usuario, 
                   sNmUsuario = oDTOUsuario.NomeUsuario,
                   sSenha = oDTOUsuario.Senha,
                   sEmail = oDTOUsuario.Email
                };

                CWUsuario cwUsuarioRetorno = await _usuarioRepository.CadastrarUsuario(cwUsuario);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Usuario cadastrada com sucesso", Id = cwUsuarioRetorno.sCdUsuario };
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
        public async Task<DTORetorno> EditarUsuario(DTOUsuario oDTOUsuario)
        {
            try
            {
                CWUsuario CWUsuario = await _entidadeLeituraRepository.Consultar<CWUsuario>(x => x.sCdUsuario.Equals(oDTOUsuario.Usuario)) 
                    ?? throw new ExceptionCustom($"Não foi possível localizar o Usuario {oDTOUsuario.Usuario} para atualização.");

                CWUsuario cwUsuario = new CWUsuario()
                {
                   sCdUsuario = oDTOUsuario.Usuario, 
                   sNmUsuario = oDTOUsuario.NomeUsuario,
                   sSenha = oDTOUsuario.Senha,
                   sEmail = oDTOUsuario.Email
                };

                CWUsuario cwUsuarioRetorno = await _usuarioRepository.CadastrarUsuario(cwUsuario);
                return new DTORetorno { Status = enumSituacao.Sucesso, Mensagem = "Usuario atualizado com sucesso", Id = cwUsuarioRetorno.sCdUsuario };
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
        public async Task<DTORetorno> AssociarPerfis(AssociacaoUsuarioRequest associacaoRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(associacaoRequest.CodigosAssociacao)) throw new ExceptionCustom($"Passe pelo menos um código de Permissao.");

                List<int> lstCodigosPerfis = associacaoRequest.CodigosAssociacao.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para associação.");
                }).ToList();                   
                
                List<CWPerfil> lstPerfis = new List<CWPerfil>();
                List<CWPerfil> lstPerfisExistentes =  await _entidadeLeituraRepository.PesquisarTodos<CWPerfil>() ?? throw new ExceptionCustom($"Não foi possível localizar nenhuma Permissao.");
                List<int> lstCodigosInvalidos = lstCodigosPerfis.Except(lstPerfisExistentes.Select(x => x.nCdPerfil)).ToList();

                foreach (CWPerfil Permissao in lstPerfisExistentes)
                {
                    if (lstCodigosPerfis.Contains(Permissao.nCdPerfil)) lstPerfis.Add(Permissao);
                }

                await _usuarioRepository.AssociarPerfis(associacaoRequest.Codigo, lstPerfis);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes códigos de Perfis não existem: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
        public async Task<DTORetorno> DesassociarPerfis(AssociacaoUsuarioRequest associacaoRequest)
        {
            try
            {
                if(string.IsNullOrEmpty(associacaoRequest.CodigosAssociacao)) throw new ExceptionCustom($"Passe pelo menos um código de Permissao.");

                List<int> lstCodigosPerfis = associacaoRequest.CodigosAssociacao.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(valor =>
                {
                    if (int.TryParse(valor.Trim(), out int numero)) return numero;
                    else throw new ExceptionCustom("Passe somente números como parâmetro para associação.");
                }).ToList();

                List<CWPerfilUsuario> lstPermissaoPermissao = await _entidadeLeituraRepository.Pesquisar<CWPerfilUsuario>(x => x.sCdUsuario == associacaoRequest.Codigo) ?? throw new ExceptionCustom($"Perfil {associacaoRequest.Codigo} não localizado no sistema.");
                List<CWPerfilUsuario> lstPerfisPermissaoRemover = new List<CWPerfilUsuario>();
                List<int> lstCodigosInvalidos = lstCodigosPerfis.Except(lstPermissaoPermissao.Select(x => x.nCdPerfil)).ToList();

                foreach (CWPerfilUsuario permissao in lstPermissaoPermissao)
                {
                    if (lstCodigosPerfis.Contains(permissao.nCdPerfil)) lstPerfisPermissaoRemover.Add(permissao);
                }

                await _usuarioRepository.DesassociarPerfis(lstPerfisPermissaoRemover);

                if (lstCodigosInvalidos.Any())
                {
                    return new DTORetorno() { Mensagem = string.Format("Os seguintes perfis não existem para este usuario: '{0}'", string.Join(", ", lstCodigosInvalidos)), Status = enumSituacao.Aviso };
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
