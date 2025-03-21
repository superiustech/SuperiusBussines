using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<CWUsuario> ObterPorLogin(CWUsuario oCWUsuario);
    }
}
