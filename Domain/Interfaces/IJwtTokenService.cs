using Domain.Entities;
using Domain.ViewModel;
using System.Security.Claims;

namespace Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GerarTokenJWT(CWClienteUsuario usuario, List<CWFuncionalidade> funcionalidades);
        bool ValidarTokenJWT(string token);
        ClaimsPrincipal GetPrincipalFromToken(string token);
        string? GetClaimValue(string token, string claimType);
        IEnumerable<Claim>? GetTokenClaims(string token);
    }
}
