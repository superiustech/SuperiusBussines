using System.Security.Claims;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Business.Uteis
{
    public static class UsuarioClaimsExtensions
    {
        public static bool ValidarFuncionalidades(this ClaimsPrincipal user, params enumFuncionalidades[] funcionalidadesRequeridas)
        {
            if (user == null) return false;

            var funcionalidadesClaim = user.FindFirst("Funcionalidades")?.Value;

            if (string.IsNullOrWhiteSpace(funcionalidadesClaim)) return false;

            var funcionalidadesUsuario = funcionalidadesClaim.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(f => int.Parse(f)).ToHashSet();

            return funcionalidadesRequeridas.Any(f => funcionalidadesUsuario.Contains((int)f));
        }
    }
}