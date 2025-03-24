using Business.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using Domain.Requests;
namespace WebApplication1.Controllers
{
    enum enumAcaoProduto
    {
        AbrirProduto = 1
    }
    public class AdministradorController : Controller
    {
        private readonly ILogger<AdministradorController> _logger;
        private readonly IUsuario _usuariorepository;
        private readonly IProduto _produto;
        public AdministradorController(ILogger<AdministradorController> logger , IUsuario usuario, IProduto produto)
        {
            _logger = logger;
            _usuariorepository = usuario;
            _produto = produto; 
        }

        #region Adminsitrador
        public IActionResult Index()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Autenticar(string email, string senha)
        {
            CWUsuario oCWUsuario = new CWUsuario() { Email = email, Senha = senha };
            bool bFlAutenticado = await _usuariorepository.Autenticar(oCWUsuario);

            if (bFlAutenticado)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return BadRequest(); // Tratamento de erro
        }
        #endregion

        #region Produto

        #region Tela Inicial
        public IActionResult Produto()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            { 
                List<enumAcaoProduto> lstEnumAcaoProduto = new List<enumAcaoProduto>();
                lstEnumAcaoProduto.Add(enumAcaoProduto.AbrirProduto);
                return View(lstEnumAcaoProduto);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet("Administrador/ProdutoEditar/{nCdProduto}")]
        public IActionResult ProdutoEditar(int nCdProduto)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                CWProduto oCWProduto = _produto.ConsultarProduto(nCdProduto);
                ViewData["Produto"] = oCWProduto;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }      

        [HttpGet]
        public async Task<IActionResult> PesquisarProdutosComPaginacao([FromQuery] PaginacaoRequest oPaginacaoRequest)
        {
            try
           {
                CWProduto oCWProdutoFiltro = new CWProduto() { sNmProduto = oPaginacaoRequest.oFiltroRequest.sNmProduto, sDsProduto = oPaginacaoRequest.oFiltroRequest.sDsProduto };
                var produtos = await _produto.PesquisarProdutos(oPaginacaoRequest.page, oPaginacaoRequest.pageSize, oCWProdutoFiltro);
                var totalItens = await _produto.PesquisarQuantidadePaginas();
                var totalPaginas = (int)Math.Ceiling(totalItens / (double)oPaginacaoRequest.pageSize);

                var resposta = new
                {
                    Produtos = produtos,
                    TotalPaginas = totalPaginas,
                    PaginaAtual = oPaginacaoRequest.page
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
            }
        }

        #endregion

        #region Cadastro
        public IActionResult ProdutoCadastrar()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public IActionResult SalvarDados([FromBody] CWProduto dados)
        {
            if (dados == null)
            {
                return BadRequest("Dados inválidos.");
            }
            HttpContext.Session.SetString("DadosProduto", JsonConvert.SerializeObject(dados));
            return Json(new { success = true, message = "Dados salvos com sucesso." });
        }
        [HttpPost]
        public async Task<IActionResult> CadastrarProduto([FromBody] List<CWVariacao> variacoes)
        {
            var dadosSalvos = HttpContext.Session.GetString("DadosProduto");
            if (string.IsNullOrEmpty(dadosSalvos))
            {
                return RedirectToAction("ProdutoCadastrar");
            }

            var dados = JsonConvert.DeserializeObject<CWProduto>(dadosSalvos);

            if (variacoes == null || variacoes.Any(v => v.nCdVariacao == 0 || string.IsNullOrEmpty(v.sNmVariacao)))
            {
                return BadRequest("Dados inválidos ou ausentes.");
            }

            await _produto.CadastrarProduto(dados, variacoes);

            return Json(new { success = true, message = "Produto cadastrado com sucesso." , codigoProduto = dados.nCdProduto});
        }
        [HttpPut("EditarVariacaoProduto")]
        public async Task<IActionResult> EditarVariacaoProduto([FromBody] EditarVariacaoProdutoRequest request)
        {
            try
            {
                await _produto.EditarVariacaoProduto(request.nCdProduto, request.variacoes);
                return Json(new { success = true, codigoProduto = request.nCdProduto , message = "Produto atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar variações: {ex.Message}");
            }
        }
        public IActionResult ProximoPasso()
        {
            var dadosSalvos = HttpContext.Session.GetString("DadosProduto");
            if (string.IsNullOrEmpty(dadosSalvos))
            {
                return RedirectToAction("ProdutoCadastrar");
            }

            var dados = JsonConvert.DeserializeObject<CWProduto>(dadosSalvos);
            return PartialView("ProdutoVariacoes", dados);
        }
        [HttpPut]
        public async Task<IActionResult> AtualizarProduto([FromBody] CWProduto oCWProduto)
        {
            try
            {
                await _produto.AtualizarProduto(oCWProduto);
                return Json(new { success = true, message = "Produto atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult ProximoPassoEdicao(int nCdProduto)
        {
            try
            {
                return PartialView("ProdutoVariacoesEditar", nCdProduto);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ConsultarVariacoesProduto(int nCdProduto)
        {   try
            {
                var variacoes = await _produto.ConsultarProdutoVariacao(nCdProduto);
                return Ok(variacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTipoVariacao()
        {
            try
            {
                var variacoes = await _produto.ObterVariacoesAtivas();
                return Ok(variacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> UnidadeDeMedida()
        {
            try
            {
                var unidadeMedidas = await _produto.ObterUnidadesAtivas();
                return Ok(unidadeMedidas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter unidade de medida: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetOpcoesVariacao(int tipo)
        {
            try
            {
                var variacoes = await _produto.ObterVariacoesAtivas();
                return Json(variacoes.Where(x => x.nCdVariacao == tipo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
            }
        }
        #endregion

        #region Imagem
        [HttpGet("Administrador/ProdutoImagem/{codigoProduto}")]
        public IActionResult ProdutoImagem(int codigoProduto)
        {
            var token = HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                return View(codigoProduto);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet("Administrador/ObterImagensProduto/{codigoProduto}")]
        public async Task<IActionResult> ObterImagensProduto(int codigoProduto)
        {
            try
            {
                var imagensProduto = await _produto.ObterImagensProduto(codigoProduto);
                return Ok(imagensProduto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter unidade de medida: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AdicionarImagem(ProdutoImagemRequest oProdutoImagemRequest)
        {
            try
            {
                if (oProdutoImagemRequest.Imagem == null || oProdutoImagemRequest.Imagem.Length == 0)
                {
                    return Json(new { success = false, message = "Nenhuma imagem foi enviada." });
                }
                CWProdutoImagem oCWProdutoImagem = await _produto.AdicionarImagem(oProdutoImagemRequest.nCdProduto, oProdutoImagemRequest.Imagem, oProdutoImagemRequest.Descricao);
                return Json(new { success = true , produtoImagem = oCWProdutoImagem });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("Administrador/ExcluirImagem/{codigoImagem}")]
        public async Task<IActionResult> ExcluirImagem(int codigoImagem)
        {
            try
            {
                if (codigoImagem == 0) return Json(new { success = false, message = "Nenhuma imagem foi enviada." });
                await _produto.ExcluirImagem(codigoImagem);
                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #endregion
    }
}
