using Business.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevendedorController : ControllerBase
    {
        private readonly ILogger<RevendedorController> _logger;
        private readonly IRevendedor _revendedor;
        public RevendedorController( ILogger<RevendedorController> logger, IRevendedor revendedor)
        {
            _logger = logger;
            _revendedor = revendedor;
        }

        [HttpGet("TiposRevendedor")]
        public async Task<IActionResult> TiposRevendedor()
        {
            var lstRevendedorTipo = _revendedor.PesquisarTipos();
            return Ok(new { success = true, tipos = lstRevendedorTipo });
        }
        [HttpGet("Revendedor/{nCdRevendedor}")]
        public async Task<IActionResult> Revendedor(int nCdRevendedor)
        {
            var lstRevendedorTipo = _revendedor.Consultar(nCdRevendedor);
            return Ok(new { success = true, revendedor = lstRevendedorTipo });
        }
        [HttpPost("CadastrarRevendedor")]
        public async Task<IActionResult> CadastrarRevendedor([FromBody] CWRevendedor revendedor)
        {
            try
            {
                if (revendedor == null) return BadRequest("Dados inválidos.");
                int nCdRevendedor = await _revendedor.CadastrarRevendedor(revendedor);
                return Ok(new { success = true, message = "Dados salvos com sucesso.", codigoRevendedor = nCdRevendedor });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar estoque");
                return StatusCode(500, new { success = false, message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }
    }
}