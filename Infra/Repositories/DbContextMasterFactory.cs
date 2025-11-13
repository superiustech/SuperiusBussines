using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class RuntimeDbContextMasterFactory
{
    private readonly IConfiguration _configuration;

    public RuntimeDbContextMasterFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContextMaster CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContextMaster>();
        var connectionString = _configuration.GetConnectionString("ConnectionMaster")
                               ?? throw new InvalidOperationException("ConnectionMaster não encontrada no appsettings.");

        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationDbContextMaster(optionsBuilder.Options);
    }
}