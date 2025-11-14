using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infra;
using Domain.Interfaces;
using Domain.Entities;
using Npgsql;

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
            // Extrai o nome do banco da connection string
            var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
            var databaseName = builder.Database;
            builder.Database = "postgres"; // conecta no banco padrão para criar

            using var masterConn = new Npgsql.NpgsqlConnection(builder.ConnectionString);
            masterConn.Open();

            using var cmd = new Npgsql.NpgsqlCommand($"CREATE DATABASE \"{databaseName}\"", masterConn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (PostgresException ex) when (ex.SqlState == "42P04")
            {
                // 42P04 = banco já existe, ignora
            }

            masterConn.Close();

            // Agora sim aplica migrações
            context.Database.Migrate();

            // Popula dados iniciais
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

            if (!context.Variacao.Any())
            {
                var cor = new CWVariacao
                {
                    sNmVariacao = "Cor",
                    sDsVariacao = "Variação de cor",
                    bFlAtiva = true,
                    VariacaoOpcoes = new List<CWVariacaoOpcao>
                    {
                        new CWVariacaoOpcao { sNmVariacaoOpcao = "Vermelho", sDsVariacaoOpcao = "Cor Vermelha", bFlAtiva = true },
                        new CWVariacaoOpcao { sNmVariacaoOpcao = "Azul", sDsVariacaoOpcao = "Cor Azul", bFlAtiva = true },
                        new CWVariacaoOpcao { sNmVariacaoOpcao = "Preto", sDsVariacaoOpcao = "Cor Preta", bFlAtiva = true }
                    }
                };

                var tamanho = new CWVariacao
                {
                    sNmVariacao = "Tamanho",
                    sDsVariacao = "Variação de tamanho",
                    bFlAtiva = true,
                    VariacaoOpcoes = new List<CWVariacaoOpcao>
                    {
                        new CWVariacaoOpcao { sNmVariacaoOpcao = "P", sDsVariacaoOpcao = "Pequeno", bFlAtiva = true },
                        new CWVariacaoOpcao { sNmVariacaoOpcao = "M", sDsVariacaoOpcao = "Médio", bFlAtiva = true },
                        new CWVariacaoOpcao { sNmVariacaoOpcao = "G", sDsVariacaoOpcao = "Grande", bFlAtiva = true }
                    }
                };

                context.Variacao.AddRange(cor, tamanho);
            }
        }

        return context;
    }
}
