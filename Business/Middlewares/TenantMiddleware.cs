using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Business.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITenantProvider tenantProvider)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var claimsIdentity = context.User.Identity as ClaimsIdentity;
                var tenantId = claimsIdentity?.FindFirst("TenantId")?.Value;
                var tenantBase = claimsIdentity?.FindFirst("TenantBase")?.Value;

                if (!string.IsNullOrEmpty(tenantId))
                {
                    tenantProvider.SetarTenantID(tenantId);
                    tenantProvider.SetarNomeBase(tenantBase);
                    context.Items["TenantId"] = tenantId; 
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Tenant ID não identificado.");
                    return;
                }
            }
            await _next(context);
        }
    }
}
