using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IUsuario
    {
        Task<bool> Autenticar(CWUsuario oCWUsuario);
    }
}
