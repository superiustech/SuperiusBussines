using Domain.Entities;
using Domain.Interfaces;
using Domain.Uteis;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Business.Middlewares
{
    public class FuncionalidadeMiddleware
    {
        private readonly RequestDelegate _next;

        public FuncionalidadeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sEndpoint = context.Request.Path.Value;
            var sFuncionalidades = context.User.FindFirst("Funcionalidades")?.Value;
            var lstFuncionalidadesUsuario = sFuncionalidades?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            if (string.IsNullOrEmpty(sEndpoint))
            {
                await _next(context);
                return;
            }

            bool bFlEncontrouEndpoint = false;

            foreach (var kvp in FuncionalidadeMapper.EndpointParaFuncionalidade)
            {
                var regexPattern = "^" + Regex.Replace(kvp.Key, @"\{[^/]+\}", @"[^/]+") + "$";

                if (Regex.IsMatch(sEndpoint, regexPattern, RegexOptions.IgnoreCase))
                {
                    bFlEncontrouEndpoint = true;

                    var funcionalidadesRequeridas = kvp.Value;
                    bool bFlTemPermissao = lstFuncionalidadesUsuario?.Intersect(funcionalidadesRequeridas).Any() == true;

                    if (!bFlTemPermissao)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Acesso negado");
                        return;
                    }

                    break;
                }
            }

            await _next(context);
        }
    }
}
