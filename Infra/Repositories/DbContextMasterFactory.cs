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
            optionsBuilder.UseNpgsql("Host=localhost;Database=SUPERIUS_MASTER;Username=postgres;Password=1234");
            return new ApplicationDbContextMaster(optionsBuilder.Options);
        }
    }
}