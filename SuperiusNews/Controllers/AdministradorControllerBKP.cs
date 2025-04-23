//using Business.Services;
//using Domain.Entities;
//using Domain.Interfaces;
//using Infra.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System.Data;
//using static System.Net.Mime.MediaTypeNames;
//using Domain.Requests;
//using Infra;
//using Azure.Core;
//namespace WebApplication1.Controllers
//{
//    enum enumAcaoProduto
//    {
//        AbrirProduto = 1\
//    }
//    public class AdministradorControllerBKP : Controller
//    {
//        private readonly ILogger<AdministradorController> _logger;
//        private readonly IUsuario _usuariorepository;
//        private readonly IProduto _produto;
//        private readonly IEstoque _estoque;
//        private readonly ApplicationDbContext _context;

//        public AdministradorControllerBKP(ILogger<AdministradorController> logger , IUsuario usuario, IProduto produto, IEstoque estoque, ApplicationDbContext context)
//        {
//            _logger = logger;
//            _usuariorepository = usuario;
//            _produto = produto;
//            _estoque = estoque;
//            _context = context;
//        }

//        #region Adminsitrador
//        public IActionResult Index()
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                return View();
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }

//        public IActionResult Login()
//        {
//            return View(); 
//        }
//        [HttpPost]
//        public async Task<IActionResult> Autenticar(string email, string senha)
//        {
//            CWUsuario oCWUsuario = new CWUsuario() { Email = email, Senha = senha };
//            bool bFlAutenticado = await _usuariorepository.Autenticar(oCWUsuario);

//            if (bFlAutenticado)
//            {
//                return RedirectToAction("Index");
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return BadRequest(); // Tratamento de erro
//        }
//        #endregion

//        #region Produto

//        #region Tela Inicial
//        public IActionResult Produto()
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            { 
//                List<enumAcaoProduto> lstEnumAcaoProduto = new List<enumAcaoProduto>();
//                lstEnumAcaoProduto.Add(enumAcaoProduto.AbrirProduto);
//                return View(lstEnumAcaoProduto);
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }
//        [HttpGet("Administrador/ProdutoEditar/{nCdProduto}")]
//        public IActionResult ProdutoEditar(int nCdProduto)
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                CWProduto oCWProduto = _produto.ConsultarProduto(nCdProduto);
//                ViewData["Produto"] = oCWProduto;
//                return View();
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }      

//        [HttpGet]
//        public async Task<IActionResult> PesquisarProdutosComPaginacao([FromQuery] PaginacaoRequest oPaginacaoRequest)
//        {
//            try
//           {
//                CWProduto oCWProdutoFiltro = new CWProduto() { sNmProduto = oPaginacaoRequest.oFiltroRequest.sNmProduto, sDsProduto = oPaginacaoRequest.oFiltroRequest.sDsProduto };
//                var produtos = await _produto.PesquisarProdutos(oPaginacaoRequest.page, oPaginacaoRequest.pageSize, oCWProdutoFiltro);
//                var totalItens = await _produto.PesquisarQuantidadePaginas();
//                var totalPaginas = (int)Math.Ceiling(totalItens / (double)oPaginacaoRequest.pageSize);

//                var resposta = new
//                {
//                    Produtos = produtos,
//                    TotalPaginas = totalPaginas,
//                    PaginaAtual = oPaginacaoRequest.page
//                };

//                return Ok(resposta);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
//            }
//        }

//        #endregion

//        #region Cadastro
//        public IActionResult ProdutoCadastrar()
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                return View();
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }
//        [HttpPost]
//        public async Task<IActionResult> SalvarDados([FromBody] CWProduto dados)
//        {
//            try
//            {
//                if (dados == null) return BadRequest("Dados inválidos."); 

//                int nCdProduto = await _produto.CadastrarProduto(dados, new List<CWVariacao>());
//                return Json(new { success = true, message = "Dados salvos com sucesso.", codigoProduto = nCdProduto });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erro ao salvar produto");
//                return StatusCode(500, new { success = false, message = "Ocorreu um erro ao processar sua solicitação." });
//            }
//        }
//        [HttpPut("EditarVariacaoProduto")]
//        public async Task<IActionResult> EditarVariacaoProduto([FromBody] EditarVariacaoProdutoRequest request)
//        {
//            try
//            {
//                await _produto.EditarVariacaoProduto(request.nCdProduto, request.variacoes);
//                return Json(new { success = true, codigoProduto = request.nCdProduto , message = "Produto atualizado com sucesso." });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao atualizar variações: {ex.Message}");
//            }
//        }
//        [HttpPut]
//        public async Task<IActionResult> AtualizarProduto([FromBody] CWProduto oCWProduto)
//        {
//            try
//            {
//                await _produto.AtualizarProduto(oCWProduto);
//                return Json(new { success = true, message = "Produto atualizado com sucesso." });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        [HttpGet("Administrador/ProdutoVariacoesEditar/{nCdProduto}")]
//        public IActionResult ProdutoVariacoesEditar(int nCdProduto)
//        {
//            try
//            {
//                return View(nCdProduto);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        [HttpGet]
//        public async Task<IActionResult> ConsultarVariacoesProduto(int nCdProduto)
//        {   try
//            {
//                var variacoes = await _produto.ConsultarProdutoVariacao(nCdProduto);
//                return Ok(variacoes);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
//            }
//        }
//        [HttpGet]
//        public async Task<IActionResult> GetTipoVariacao()
//        {
//            try
//            {
//                var variacoes = await _produto.ObterVariacoesAtivas();
//                return Ok(variacoes);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
//            }
//        }
//        [HttpGet]
//        public async Task<IActionResult> UnidadeDeMedida()
//        {
//            try
//            {
//                var unidadeMedidas = await _produto.ObterUnidadesAtivas();
//                return Ok(unidadeMedidas);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter unidade de medida: {ex.Message}");
//            }
//        }
//        [HttpGet]
//        public async Task<IActionResult> GetOpcoesVariacao(int tipo)
//        {
//            try
//            {
//                var variacoes = await _produto.ObterVariacoesAtivas();
//                return Json(variacoes.Where(x => x.nCdVariacao == tipo));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
//            }
//        }
//        #endregion

//        #region Imagem
//        [HttpGet("Administrador/ProdutoImagem/{codigoProduto}")]
//        public IActionResult ProdutoImagem(int codigoProduto)
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                return View(codigoProduto);
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }
//        [HttpGet("Administrador/ObterImagensProduto/{codigoProduto}")]
//        public async Task<IActionResult> ObterImagensProduto(int codigoProduto)
//        {
//            try
//            {
//                var imagensProduto = await _produto.ObterImagensProduto(codigoProduto);
//                return Ok(imagensProduto);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter unidade de medida: {ex.Message}");
//            }
//        }
//        [HttpPost]
//        public async Task<IActionResult> AdicionarImagem(ProdutoImagemRequest oProdutoImagemRequest)
//        {
//            try
//            {
//                if (oProdutoImagemRequest.Imagem == null || oProdutoImagemRequest.Imagem.Length == 0)
//                {
//                    return Json(new { success = false, message = "Nenhuma imagem foi enviada." });
//                }
//                CWProdutoImagem oCWProdutoImagem = await _produto.AdicionarImagem(oProdutoImagemRequest.nCdProduto, oProdutoImagemRequest.Imagem, oProdutoImagemRequest.Descricao);
//                return Json(new { success = true , produtoImagem = oCWProdutoImagem });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        [HttpDelete("Administrador/ExcluirImagem/{codigoImagem}")]
//        public async Task<IActionResult> ExcluirImagem(int codigoImagem)
//        {
//            try
//            {
//                if (codigoImagem == 0) return Json(new { success = false, message = "Nenhuma imagem foi enviada." });
//                await _produto.ExcluirImagem(codigoImagem);
//                return Json(new { success = true});
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        #endregion

//        #endregion

//        #region Estoque 

//        #region Tela inicial 
//        public IActionResult CadastrarEstoque()
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                return View();
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }
//        #endregion

//        #region Operações
//        [HttpPost]
//        public async Task<IActionResult> CadastrarEstoque([FromBody] CWEstoque estoque)
//        {
//            try
//            {
//                if (estoque == null) return BadRequest("Dados inválidos.");

//                int nCdEstoque = await _estoque.CadastrarEstoque(estoque, new List<CWProduto>());
//                return Json(new { success = true, message = "Dados salvos com sucesso.", codigoEstoque = nCdEstoque });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erro ao salvar produto");
//                return StatusCode(500, new { success = false, message = "Ocorreu um erro ao processar sua solicitação." });
//            }
//        }
//        [HttpGet("Administrador/EstoqueProduto/{codigoEstoque}")]
//        public async Task<IActionResult> EstoqueProduto(int codigoEstoque)
//        {
//            var token = HttpContext.Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                CWEstoque estoque = await _estoque.Consultar(codigoEstoque);
//                List<CWProduto> produtos = await _produto.PesquisarPorEstoque(codigoEstoque);
//                List<CWEstoqueProduto> estoqueProdutos = await _estoque.PesquisarPorEstoqueProduto(codigoEstoque);
//                List<CWProduto> todosProdutos = await _produto.PesquisarProdutos();

//                var resultado = ( from ep in estoqueProdutos join p in produtos on ep.nCdProduto equals p.nCdProduto where ep.nCdEstoque == codigoEstoque
//                select new ProdutoEstoqueDTO {
//                    Produto = p,
//                    EstoqueProduto = ep
//                }).ToList();

//                var model = new EstoqueProdutoViewModel
//                {
//                    Estoque = estoque,
//                    Produtos = resultado,
//                    TodosProdutos = todosProdutos
//                };
//                return View(model);
//            }
//            else
//            {
//                return RedirectToAction("Login");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> AdicionarEstoqueProduto([FromBody] CWEstoqueProduto oCWEstoqueProduto)
//        {
//            try
//            {
//                if (oCWEstoqueProduto.nCdEstoque == 0 || oCWEstoqueProduto.nCdProduto == 0)
//                {
//                    return Json(new { success = false, message = "Nenhum estoque/produto foi enviado." });
//                }

//                CWEstoqueProduto oEstoqueProduto = new CWEstoqueProduto()
//                {
//                    nCdEstoque = oCWEstoqueProduto.nCdEstoque,
//                    nCdProduto = oCWEstoqueProduto.nCdProduto,
//                    dQtEstoque = oCWEstoqueProduto?.dQtEstoque ?? 0,
//                    dQtMinima  = oCWEstoqueProduto?.dQtMinima  ?? 0,
//                    dVlCusto   = oCWEstoqueProduto?.dVlCusto   ?? 0,
//                    dVlVenda   = oCWEstoqueProduto?.dVlVenda   ?? 0

//                };

//                bool bFLProdutoAtrelado = await _estoque.AdicionarEstoqueProduto(oEstoqueProduto);
//                return Json(new { success = true, produtoAtrelado = bFLProdutoAtrelado });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        [HttpDelete]
//        public async Task<IActionResult> RemoverEstoqueProduto([FromBody] EstoqueRequest request)
//        {
//            try
//            {
//                if (request.nCdEstoque == 0 || request.nCdProduto == 0) return Json(new { success = false, message = "Nenhum estoque/produto foi enviado." });
//                await _estoque.RemoverEstoqueProduto(request.nCdEstoque, request.nCdProduto);
//                return Json(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }
//        [HttpPut]
//        public async Task<IActionResult> EditarProdutoEstoque([FromBody] CWEstoqueProduto request)
//        {
//            try
//            {
//                if (request.nCdEstoque == 0 || request.nCdProduto == 0) return Json(new { success = false, message = "Nenhum estoque/produto foi enviado." });
//                await _estoque.AdicionarEditarProdutoEstoque(request);
//                return Json(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        #endregion

//        #endregion
//    }
//}

//BKP 2 


//using Business.Services;
//using Domain.Entities;
//using Domain.Interfaces;
//using Domain.Requests;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace WebApplication1.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AdministradorController : ControllerBase
//    {
//        private readonly ILogger<AdministradorController> _logger;
//        private readonly IUsuario _usuariorepository;
//        private readonly IProduto _produto;
//        private readonly IEstoque _estoque;

//        public AdministradorController(
//            ILogger<AdministradorController> logger,
//            IUsuario usuario,
//            IProduto produto,
//            IEstoque estoque)
//        {
//            _logger = logger;
//            _usuariorepository = usuario;
//            _produto = produto;
//            _estoque = estoque;
//        }

//        [HttpPost("Autenticar")]
//        public async Task<IActionResult> Autenticar(string email, string senha)
//        {
//            CWUsuario oCWUsuario = new CWUsuario() { Email = email, Senha = senha };
//            bool bFlAutenticado = await _usuariorepository.Autenticar(oCWUsuario);

//            if (bFlAutenticado)
//            {
//                return Ok();
//            }

//            return Unauthorized();
//        }

//        #region Produto
//        [HttpGet("ConsultarProduto/{nCdProduto}")]
//        public IActionResult ConsultarProduto(int nCdProduto)
//        {
//            CWProduto oCWProduto = _produto.ConsultarProduto(nCdProduto);
//            return Ok(new
//            {
//                success = true,
//                produto = oCWProduto
//            });
//        }

//        [HttpGet("PesquisarProdutosComPaginacao")]
//        public async Task<IActionResult> PesquisarProdutosComPaginacao([FromQuery] PaginacaoRequest oPaginacaoRequest)
//        {
//            try
//            {
//                CWProduto oCWProdutoFiltro = new CWProduto()
//                {
//                    sNmProduto = oPaginacaoRequest.oFiltroRequest?.sNmFiltro ?? string.Empty,
//                    sDsProduto = oPaginacaoRequest.oFiltroRequest?.sDsFiltro ?? string.Empty
//                };

//                var produtos = await _produto.PesquisarProdutos(oPaginacaoRequest.page, oPaginacaoRequest.pageSize, oCWProdutoFiltro);
//                var totalItens = await _produto.PesquisarQuantidadePaginas(oCWProdutoFiltro);
//                var totalPaginas = (int)Math.Ceiling(totalItens / (double)oPaginacaoRequest.pageSize);

//                return Ok(new
//                {
//                    Produtos = produtos,
//                    TotalPaginas = totalPaginas,
//                    PaginaAtual = oPaginacaoRequest.page
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
//            }
//        }
//        [HttpGet("PesquisarEstoquesComPaginacao")]
//        public async Task<IActionResult> PesquisarEstoquesComPaginacao([FromQuery] PaginacaoRequest oPaginacaoRequest)
//        {
//            try
//            {
//                CWEstoque oCWEstoqueFiltro = new CWEstoque()
//                {
//                    sNmEstoque = oPaginacaoRequest.oFiltroRequest?.sNmFiltro ?? string.Empty,
//                    sDsEstoque = oPaginacaoRequest.oFiltroRequest?.sDsFiltro ?? string.Empty
//                };

//                var estoques = await _estoque.PesquisarEstoques(oPaginacaoRequest.page, oPaginacaoRequest.pageSize, oCWEstoqueFiltro);
//                var totalItens = await _estoque.PesquisarQuantidadePaginas(oCWEstoqueFiltro);
//                var totalPaginas = (int)Math.Ceiling(totalItens / (double)oPaginacaoRequest.pageSize);

//                return Ok(new
//                {
//                    Estoques = estoques,
//                    TotalPaginas = totalPaginas,
//                    PaginaAtual = oPaginacaoRequest.page
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter produtos: {ex.Message}");
//            }
//        }
//        [HttpGet("Estoque/{nCdEstoque}")]
//        public async Task<IActionResult> Estoque(int nCdEstoque)
//        {
//            try
//            {
//                CWEstoque oCWEstoque = await _estoque.Consultar(nCdEstoque);
//                return Ok(new { success = true, estoque = oCWEstoque });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter estoque: {ex.Message}");
//            }
//        }

//        [HttpPost("SalvarDados")]
//        public async Task<IActionResult> SalvarDados([FromBody] CWProduto dados)
//        {
//            try
//            {
//                if (dados == null) return BadRequest("Dados inválidos.");

//                int nCdProduto = await _produto.CadastrarProduto(dados);
//                return Ok(new { success = true, message = "Dados salvos com sucesso.", codigoProduto = nCdProduto });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erro ao salvar produto");
//                return StatusCode(500, new { success = false, message = "Ocorreu um erro ao processar sua solicitação." });
//            }
//        }

//        [HttpPut("EditarVariacaoProduto")]
//        public async Task<IActionResult> EditarVariacaoProduto([FromBody] EditarVariacaoProdutoRequest request)
//        {
//            try
//            {
//                await _produto.EditarVariacaoProduto(request.nCdProduto, request.variacoes);
//                return Ok(new { success = true, codigoProduto = request.nCdProduto, message = "Produto atualizado com sucesso." });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao atualizar variações: {ex.Message}");
//            }
//        }

//        [HttpPut("AtualizarProduto")]
//        public async Task<IActionResult> AtualizarProduto([FromBody] CWProduto oCWProduto)
//        {
//            try
//            {
//                await _produto.AtualizarProduto(oCWProduto);
//                return Ok(new { success = true, message = "Produto atualizado com sucesso." });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        [HttpGet("ConsultarVariacoesProduto/{nCdProduto}")]
//        public async Task<IActionResult> ConsultarVariacoesProduto(int nCdProduto)
//        {
//            try
//            {
//                var variacoes = await _produto.ConsultarProdutoVariacao(nCdProduto);
//                return Ok(variacoes);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
//            }
//        }

//        [HttpGet("GetTipoVariacao")]
//        public async Task<IActionResult> GetTipoVariacao()
//        {
//            try
//            {
//                var variacoes = await _produto.ObterVariacoesAtivas();
//                return Ok(variacoes);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
//            }
//        }

//        [HttpGet("UnidadeDeMedida")]
//        public async Task<IActionResult> UnidadeDeMedida()
//        {
//            try
//            {
//                var unidadeMedidas = await _produto.ObterUnidadesAtivas();
//                return Ok(new
//                {
//                    success = true,
//                    unidade = unidadeMedidas
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter unidade de medida: {ex.Message}");
//            }
//        }

//        [HttpGet("GetOpcoesVariacao/{tipo}")]
//        public async Task<IActionResult> GetOpcoesVariacao(int tipo)
//        {
//            try
//            {
//                var variacoes = await _produto.ObterVariacoesAtivas();
//                return Ok(variacoes.Where(x => x.nCdVariacao == tipo));
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter variações: {ex.Message}");
//            }
//        }

//        [HttpGet("ObterImagensProduto/{codigoProduto}")]
//        public async Task<IActionResult> ObterImagensProduto(int codigoProduto)
//        {
//            try
//            {
//                var imagensProduto = await _produto.ObterImagensProduto(codigoProduto);
//                return Ok(imagensProduto);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao obter imagens: {ex.Message}");
//            }
//        }

//        [HttpPost("AdicionarImagem")]
//        public async Task<IActionResult> AdicionarImagem([FromForm] ProdutoImagemRequest oProdutoImagemRequest)
//        {
//            try
//            {
//                if (oProdutoImagemRequest.Imagem == null || oProdutoImagemRequest.Imagem.Length == 0)
//                {
//                    return BadRequest(new { success = false, message = "Nenhuma imagem foi enviada." });
//                }

//                CWProdutoImagem oCWProdutoImagem = await _produto.AdicionarImagem(
//                    oProdutoImagemRequest.nCdProduto,
//                    oProdutoImagemRequest.Imagem,
//                    oProdutoImagemRequest.Descricao);

//                return Ok(new { success = true, produtoImagem = oCWProdutoImagem });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }

//        [HttpDelete("ExcluirImagem/{codigoImagem}")]
//        public async Task<IActionResult> ExcluirImagem(int codigoImagem)
//        {
//            try
//            {
//                if (codigoImagem == 0) return BadRequest(new { success = false, message = "Código inválido." });

//                await _produto.ExcluirImagem(codigoImagem);
//                return Ok(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message });
//            }
//        }
//        #endregion

//        #region Estoque
//        [HttpPost("CadastrarEstoque")]
//        public async Task<IActionResult> CadastrarEstoque([FromBody] CWEstoque estoque)
//                if (estoque == null) return BadRequest("Dados inválidos.");
//            try
//                int nCdEstoque = await _estoque.CadastrarEstoque(estoque, new List<CWProduto>());
//                return Ok(new { success = true, message = "Dados salvos com sucesso.", codigoEstoque = nCdEstoque
//    });

//                int nCdEstoque = await _estoque.CadastrarEstoque(estoque, new List<CWProduto>());
//                return Ok(new { success = true, message = "Dados salvos com sucesso.", codigoEstoque = nCdEstoque
//});
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Erro ao salvar estoque");

//[HttpGet("EstoqueProduto/{codigoEstoque}")]
//            }
//        }

//        [HttpGet("EstoqueProduto/{codigoEstoque}")]
//public async Task<IActionResult> EstoqueProduto(int codigoEstoque)
//{
//    try
//    {
//        CWEstoque estoque = await _estoque.Consultar(codigoEstoque);
//        List<CWProduto> produtos = await _produto.PesquisarPorEstoque(codigoEstoque);
//        List<CWEstoqueProduto> estoqueProdutos = await _estoque.PesquisarPorEstoqueProduto(codigoEstoque);
//        List<CWProduto> todosProdutos = await _produto.PesquisarProdutos();

//        var resultado = (from ep in estoqueProdutos
//                         join p in produtos on ep.nCdProduto equals p.nCdProduto
//                         where ep.nCdEstoque == codigoEstoque
//                         select new
//                         {
//                             Produto = p,
//                             EstoqueProduto = ep
//                         }).ToList();

//        return Ok(new
//        {
//            Estoque = estoque,
//            Produtos = resultado,
//            TodosProdutos = todosProdutos
//        });
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, $"Erro ao obter estoque: {ex.Message}");
//    }
//}

//[HttpPost("AdicionarEstoqueProduto")]
//public async Task<IActionResult> AdicionarEstoqueProduto([FromBody] CWEstoqueProduto oCWEstoqueProduto)
//{
//    try
//    {
//        if (oCWEstoqueProduto.nCdEstoque == 0 || oCWEstoqueProduto.nCdProduto == 0)
//        {
//            return BadRequest(new { success = false, message = "Dados inválidos." });
//        }

//        var oEstoqueProduto = new CWEstoqueProduto()
//        {
//            nCdEstoque = oCWEstoqueProduto.nCdEstoque,
//            nCdProduto = oCWEstoqueProduto.nCdProduto,
//            dQtEstoque = oCWEstoqueProduto?.dQtEstoque ?? 0,
//            dQtMinima = oCWEstoqueProduto?.dQtMinima ?? 0,
//            dVlCusto = oCWEstoqueProduto?.dVlCusto ?? 0,
//            dVlVenda = oCWEstoqueProduto?.dVlVenda ?? 0
//        };

//        bool bFLProdutoAtrelado = await _estoque.AdicionarEstoqueProduto(oEstoqueProduto);
//        return Ok(new { success = true, produtoAtrelado = bFLProdutoAtrelado });
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new { success = false, message = ex.Message });
//    }
//}

//[HttpDelete("RemoverEstoqueProduto")]
//public async Task<IActionResult> RemoverEstoqueProduto([FromBody] EstoqueRequest request)
//{
//    try
//    {
//        if (request.nCdEstoque == 0 || request.nCdProduto == 0)
//            return BadRequest(new { success = false, message = "Dados inválidos." });

//        await _estoque.RemoverEstoqueProduto(request.nCdEstoque, request.nCdProduto);
//        return Ok(new { success = true });
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new { success = false, message = ex.Message });
//    }
//}

//[HttpPut("EditarProdutoEstoque")]
//public async Task<IActionResult> EditarProdutoEstoque([FromBody] CWEstoqueProduto request)
//{
//    try
//    {
//        if (request.nCdEstoque == 0 || request.nCdProduto == 0)
//            return BadRequest(new { success = false, message = "Dados inválidos." });

//        await _estoque.AdicionarEditarProdutoEstoque(request);
//        return Ok(new { success = true });
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new { success = false, message = ex.Message });
//    }
//}
//        #endregion
//    }
//}
