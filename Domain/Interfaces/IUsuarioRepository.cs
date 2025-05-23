using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<CWUsuario> CadastrarUsuario(CWUsuario cwUsuario);
        Task AssociarPerfis(string codigoUsuario, List<CWPerfil> lstPerfis);
        Task DesassociarPerfis(List<CWPerfilUsuario> lstPerfis);
        Task AssociarDesassociarPerfis(string codigoUsuario, List<CWPerfil> lstPerfis);
    }
}
