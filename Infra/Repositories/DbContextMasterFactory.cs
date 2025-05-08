using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infra
{
    public class ApplicationDbContextMasterFactory : IDesignTimeDbContextFactory<ApplicationDbContextMaster>
    {
        public ApplicationDbContextMaster CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContextMaster>();
            optionsBuilder.UseNpgsql("User ID=postgres;Password=usr_sup;Host=localhost;Port=5432;Database=SUPERIUS_MASTER;"); 
            return new ApplicationDbContextMaster(optionsBuilder.Options);
        }
    }
}