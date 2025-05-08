using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Interfaces;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.ViewModel;
using Domain.Entities.Uteis;
using Domain.ViewModel;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Helpers;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Crypto.Generators;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ILogger<AutenticacaoController> _logger;
        private readonly IAutenticacao _autenticacao;

        public AutenticacaoController( ILogger<AutenticacaoController> logger, IAutenticacao autenticacao)
        {
            _logger = logger;
            _autenticacao = autenticacao;
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] DTOToken oDTOToken)
        {
            try
            {
                return Ok(new { Token = await _autenticacao.GerarTokenJWT(oDTOToken) });
            }
            catch (ExceptionCustom ex)
            {
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
    }
}