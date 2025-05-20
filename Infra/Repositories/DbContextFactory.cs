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
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        string connectionString;

        if (string.IsNullOrWhiteSpace(tenantBase))
        {
            connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection não encontrada no appsettings.");
        }
        else
        {
            var template = _configuration.GetConnectionString("TenantBaseConnectionTemplate") ?? throw new InvalidOperationException("TenantBaseConnectionTemplate não encontrada no appsettings.");
            connectionString = template.Replace("{tenant}", tenantBase.ToUpper());
        }

        optionsBuilder.UseNpgsql(connectionString);
        var context = new ApplicationDbContext(optionsBuilder.Options);

        if (!context.Database.CanConnect())
        {
            // TODO: Essa lógica deve ser extraída futuramente para um serviço separado!

            context.Database.Migrate();

            if (!context.UnidadeMedida.Any())
            {
                context.UnidadeMedida.Add(new Domain.Entities.CWUnidadeMedida
                {
                    sCdUnidadeMedida = "UN",
                    sDsUnidadeMedida = "Unidade",
                    sSgUnidadeMedida = "UN",
                    bFlAtivo = 1
                });

                context.RevendedorTipo.AddRange(
                    new Domain.Entities.CWRevendedorTipo
                    {
                        bFlAtivo = 1,
                        sDsTipo = "Pessoa Jurídica",
                        sNmTipo = "Pessoa Jurídica"
                    },
                    new Domain.Entities.CWRevendedorTipo
                    {
                        bFlAtivo = 1,
                        sDsTipo = "Pessoa Física",
                        sNmTipo = "Pessoa Física"
                    }
                );

                context.SaveChanges();
            }
        }

        return context;
    }
}
