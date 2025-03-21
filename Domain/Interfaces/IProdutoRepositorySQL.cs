using Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IProdutoRepositorySQL
    {
        string AtualizarProdutoSQL();
        string ConsultarProdutoVariacaoSQL();

    }
}
