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
}
