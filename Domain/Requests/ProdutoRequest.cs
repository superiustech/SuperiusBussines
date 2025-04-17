using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests
{
    public class EditarVariacaoProdutoRequest
    {
        public int nCdProduto { get; set; }
        public List<CWVariacao> variacoes { get; set; }
    }
    public class ProdutoImagemRequest
    {
        public int nCdProduto { get; set; }
        public IFormFile Imagem { get; set; }
        public string Descricao { get; set; }
    }
    public class FiltroRequest
    {
        public string? sNmFiltro { get; set; }
        public string? sDsFiltro { get; set; }
    }
     public class PaginacaoRequest {
         public int page { get; set; } = 1;
         public int pageSize { get; set; } = 10;
         public FiltroRequest? oFiltroRequest { get; set; }
     }
}
