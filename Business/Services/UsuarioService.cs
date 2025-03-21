using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services
{
    public class UsuarioService : IUsuario
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Autenticar(CWUsuario oCWUsuario)
        {
            CWUsuario usuario = await _usuarioRepository.ObterPorLogin(oCWUsuario);
            usuario = oCWUsuario;
            if (usuario != null)
            {
                string token = string.Format(@"{0}_{1}",usuario.Email,Guid.NewGuid().ToString());
                _httpContextAccessor.HttpContext.Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,  
                    Secure = true,   
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(30)
                });
                return true;
            }
            return false;
        }
    }
}
