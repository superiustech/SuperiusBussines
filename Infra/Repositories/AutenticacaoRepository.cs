using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class AutenticacaoRepository : IAutenticacaoRepository
    {
        private readonly ApplicationDbContextMaster _dbContextMaster;
        public AutenticacaoRepository(ApplicationDbContextMaster dbContext)
        {
            _dbContextMaster = dbContext;
        }
        public async Task<CWClienteUsuario> ConsultarUsuarioEmpresa(string sCdUsuario)
        {
            return await _dbContextMaster.ClienteUsuario.Include(u => u.Cliente).FirstOrDefaultAsync(u => u.sCdUsuario == sCdUsuario);
        }
    }
}