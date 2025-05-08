using Microsoft.EntityFrameworkCore;
using Domain.Entities; 

namespace Infra
{
    public class ApplicationDbContextMaster : DbContext
    {
        public ApplicationDbContextMaster(DbContextOptions<ApplicationDbContextMaster> options) : base(options) { }
        public DbSet<CWCliente> Cliente { get; set; }
        public DbSet<CWClienteUsuario> ClienteUsuario { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region CLIENTE 
            modelBuilder.Entity<CWCliente>().HasKey(c => c.nCdCliente);
            modelBuilder.Entity<CWCliente>().HasMany(c => c.Usuarios).WithOne(u => u.Cliente).HasForeignKey(u => u.nCdCliente).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CWCliente>().ToTable("CLIENTE");
            modelBuilder.Entity<CWClienteUsuario>().ToTable("CLIENTE_USUARIO");
            #endregion
        }
    }
}
