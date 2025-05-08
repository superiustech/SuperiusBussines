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

namespace Business.Services
{
    public class AutenticacaoService : IAutenticacao
    {
        private readonly IAutenticacaoRepository _autenticacaoRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AutenticacaoService( IAutenticacaoRepository autenticacaoRepository, IJwtTokenService jwtTokenService)
        {
            _autenticacaoRepository = autenticacaoRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> GerarTokenJWT(DTOToken oDTOToken)
        {
            try 
            { 
                if (oDTOToken == null) throw new ArgumentNullException(nameof(oDTOToken));
                if (string.IsNullOrWhiteSpace(oDTOToken.Login)) throw new ArgumentException("Login não pode ser vazio", nameof(oDTOToken.Login));
                if (string.IsNullOrWhiteSpace(oDTOToken.Senha)) throw new ArgumentException("Senha não pode ser vazia", nameof(oDTOToken.Senha));

                var usuario = await _autenticacaoRepository.ConsultarUsuarioEmpresa(oDTOToken.Login) ?? throw new ExceptionCustom($"Usuário {oDTOToken.Login} não localizado no sistema.");
                if (usuario.sSenha != oDTOToken.Senha) throw new ExceptionCustom("Credenciais inválidas.");

                return _jwtTokenService.GerarTokenJWT(usuario);
            }
            catch
            {
                throw;
            }
        }
    }
}