using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infra;
using Domain.Interfaces;

public class RuntimeDbContextFactory : IRuntimeDbContextFactory<ApplicationDbContext>
{
    private readonly IConfiguration _configuration;

    public RuntimeDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContext CreateDbContext(string tenantBase)
    {
        var connectionStringTemplate = _configuration.GetConnectionString("TenantBaseConnectionTemplate");
        var connectionString = connectionStringTemplate.Replace("{tenant}", tenantBase.ToUpper());
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        var context = new ApplicationDbContext(optionsBuilder.Options);

        if (!context.Database.CanConnect())
        {
            context.Database.Migrate(); // AJUSTAR ISSO EM UM PROJETO A PARTE E NÃO AQUI!

            context.UnidadeMedida.Add(new Domain.Entities.CWUnidadeMedida
            {
               sCdUnidadeMedida = "UN",
               sDsUnidadeMedida = "Unidade",
               sSgUnidadeMedida = "UN",
               bFlAtivo = 1
            });

            context.RevendedorTipo.Add(new Domain.Entities.CWRevendedorTipo
            {
                bFlAtivo = 1,
                sDsTipo = "Pessoa Jurídica",
                sNmTipo = "Pessoa Jurídica"
            });

            context.RevendedorTipo.Add(new Domain.Entities.CWRevendedorTipo
            {
                bFlAtivo = 1,
                sDsTipo = "Pessoa Física",
                sNmTipo = "Pessoa Física"
            });

            context.SaveChanges();
        }

        return context;
    }
}
