using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Entities.ViewModel;
using Domain.Entities.Uteis;
using Domain.Interfaces;
using Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Business.Uteis;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class AutenticacaoService : IAutenticacao
    {
        private readonly IAutenticacaoRepository _autenticacaoRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRuntimeDbContextFactory<ApplicationDbContext> _runtimeDbContextFactory;

        public AutenticacaoService( IAutenticacaoRepository autenticacaoRepository, IJwtTokenService jwtTokenService, IEntidadeLeituraRepository entidadeLeituraRepository, IRuntimeDbContextFactory<ApplicationDbContext> runtimeDbContextFactory)
        {
            _autenticacaoRepository = autenticacaoRepository;
            _jwtTokenService = jwtTokenService;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _runtimeDbContextFactory = runtimeDbContextFactory;
        }

        public async Task<string> GerarTokenJWT(DTOToken oDTOToken)
        {
            try
            {
                if (oDTOToken == null) throw new ArgumentNullException(nameof(oDTOToken));
                if (string.IsNullOrWhiteSpace(oDTOToken.Login)) throw new ArgumentException("Login não pode ser vazio", nameof(oDTOToken.Login));
                if (string.IsNullOrWhiteSpace(oDTOToken.Senha)) throw new ArgumentException("Senha não pode ser vazia", nameof(oDTOToken.Senha));

                var usuarioMaster = await _autenticacaoRepository.ConsultarUsuarioEmpresa(oDTOToken.Login) ?? throw new ExceptionCustom($"Usuário {oDTOToken.Login} não localizado no sistema.");

                var tenantBase = usuarioMaster.Cliente?.sNmCliente?.ToUpper();

                if (string.IsNullOrEmpty(tenantBase)) throw new ExceptionCustom("Base de dados do cliente não encontrada.");

                using var contextoCliente = _runtimeDbContextFactory.CreateDbContext(tenantBase);
                var usuario = await contextoCliente.Usuario.FirstOrDefaultAsync(x => x.sCdUsuario == usuarioMaster.sCdUsuario) ?? throw new ExceptionCustom($"Usuário {oDTOToken.Login} não localizado no sistema do cliente.");

                if (usuario.sSenha != oDTOToken.Senha) throw new ExceptionCustom("Credenciais inválidas.");

                return _jwtTokenService.GerarTokenJWT(usuarioMaster);
            }
            catch
            {
                throw;
            }
        }

    }
}