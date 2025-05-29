using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infra;
using Domain.Interfaces;
using Domain.Entities;

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

                var funcionalidades = new List<CWFuncionalidade>
                {
                    new() { nCdFuncionalidade = 1, sNmFuncionalidade = "Estoque Abrir tela", sDsFuncionalidade = "Abrir a tela principal de estoque", bFlAtiva = true },
                    new() { nCdFuncionalidade = 2, sNmFuncionalidade = "Incluir/Editar estoques", sDsFuncionalidade = "Cadastrar ou modificar estoques", bFlAtiva = true },
                    new() { nCdFuncionalidade = 3, sNmFuncionalidade = "Excluir estoques", sDsFuncionalidade = "Remover estoques existentes", bFlAtiva = true },
                    new() { nCdFuncionalidade = 4, sNmFuncionalidade = "Abrir estoque detalhes", sDsFuncionalidade = "Visualizar os detalhes de um estoque", bFlAtiva = true },
                    new() { nCdFuncionalidade = 5, sNmFuncionalidade = "Dar entrada estoque", sDsFuncionalidade = "Registrar entrada de produtos no estoque", bFlAtiva = true },
                    new() { nCdFuncionalidade = 6, sNmFuncionalidade = "Dar saída no estoque", sDsFuncionalidade = "Registrar saída de produtos do estoque", bFlAtiva = true },
                    new() { nCdFuncionalidade = 7, sNmFuncionalidade = "Inativar produtos do estoque", sDsFuncionalidade = "Desativar produtos disponíveis no estoque", bFlAtiva = true },
                    new() { nCdFuncionalidade = 8, sNmFuncionalidade = "Visualizar todos os produtos", sDsFuncionalidade = "Listar todos os produtos cadastrados", bFlAtiva = true },
                    new() { nCdFuncionalidade = 9, sNmFuncionalidade = "Incluir/Editar produtos", sDsFuncionalidade = "Cadastrar ou alterar informações de produtos", bFlAtiva = true },
                    new() { nCdFuncionalidade = 10, sNmFuncionalidade = "Excluir produtos", sDsFuncionalidade = "Remover produtos cadastrados", bFlAtiva = true },
                    new() { nCdFuncionalidade = 11, sNmFuncionalidade = "Visualizar revendedores", sDsFuncionalidade = "Listar todos os revendedores cadastrados", bFlAtiva = true },
                    new() { nCdFuncionalidade = 12, sNmFuncionalidade = "Incluir/Editar revendedores", sDsFuncionalidade = "Cadastrar ou alterar dados de revendedores", bFlAtiva = true },
                    new() { nCdFuncionalidade = 13, sNmFuncionalidade = "Excluir revendedores", sDsFuncionalidade = "Remover revendedores cadastrados", bFlAtiva = true },
                    new() { nCdFuncionalidade = 14, sNmFuncionalidade = "Visualizar configurações", sDsFuncionalidade = "Acessar as configurações do sistema", bFlAtiva = true },
                    new() { nCdFuncionalidade = 15, sNmFuncionalidade = "Visualizar todas funcionalidades", sDsFuncionalidade = "Listar todas as funcionalidades do sistema", bFlAtiva = true },
                    new() { nCdFuncionalidade = 16, sNmFuncionalidade = "Editar funcionalidades", sDsFuncionalidade = "Modificar funcionalidades existentes", bFlAtiva = true },
                    new() { nCdFuncionalidade = 17, sNmFuncionalidade = "Ativar funcionalidades", sDsFuncionalidade = "Ativar funcionalidades previamente inativas", bFlAtiva = true },
                    new() { nCdFuncionalidade = 18, sNmFuncionalidade = "Inativar funcionalidades", sDsFuncionalidade = "Desativar funcionalidades do sistema", bFlAtiva = true },
                    new() { nCdFuncionalidade = 19, sNmFuncionalidade = "Visualizar todas permissões", sDsFuncionalidade = "Listar todas as permissões configuradas", bFlAtiva = true },
                    new() { nCdFuncionalidade = 20, sNmFuncionalidade = "Editar permissões", sDsFuncionalidade = "Modificar permissões existentes", bFlAtiva = true },
                    new() { nCdFuncionalidade = 21, sNmFuncionalidade = "Ativar permissões", sDsFuncionalidade = "Ativar permissões desativadas", bFlAtiva = true },
                    new() { nCdFuncionalidade = 22, sNmFuncionalidade = "Inativar permissões", sDsFuncionalidade = "Desativar permissões ativas", bFlAtiva = true },
                    new() { nCdFuncionalidade = 23, sNmFuncionalidade = "Associar funcionalidades a permissão", sDsFuncionalidade = "Vincular funcionalidades específicas a permissões", bFlAtiva = true },
                    new() { nCdFuncionalidade = 24, sNmFuncionalidade = "Visualizar todos os perfis", sDsFuncionalidade = "Listar todos os perfis de usuários", bFlAtiva = true },
                    new() { nCdFuncionalidade = 25, sNmFuncionalidade = "Ativar perfis", sDsFuncionalidade = "Ativar perfis de usuário inativos", bFlAtiva = true },
                    new() { nCdFuncionalidade = 26, sNmFuncionalidade = "Inativar perfis", sDsFuncionalidade = "Desativar perfis de usuário ativos", bFlAtiva = true },
                    new() { nCdFuncionalidade = 27, sNmFuncionalidade = "Associar permissões a perfis", sDsFuncionalidade = "Vincular permissões a perfis de usuário", bFlAtiva = true },
                    new() { nCdFuncionalidade = 28, sNmFuncionalidade = "Acessar todos os usuários", sDsFuncionalidade = "Listar todos os usuários do sistema", bFlAtiva = true },
                    new() { nCdFuncionalidade = 29, sNmFuncionalidade = "Incluir/Editar usuários", sDsFuncionalidade = "Cadastrar ou editar informações de usuários", bFlAtiva = true },
                    new() { nCdFuncionalidade = 30, sNmFuncionalidade = "Associar perfis a usuários", sDsFuncionalidade = "Vincular perfis a usuários específicos", bFlAtiva = true },
                    new() { nCdFuncionalidade = 31, sNmFuncionalidade = "Ativar usuários", sDsFuncionalidade = "Ativar contas de usuários", bFlAtiva = true },
                    new() { nCdFuncionalidade = 32, sNmFuncionalidade = "Inativar usuários", sDsFuncionalidade = "Desativar contas de usuários", bFlAtiva = true },
                    new() { nCdFuncionalidade = 33, sNmFuncionalidade = "Edição de usuarios revendedores", sDsFuncionalidade = "Associar ou desassociar usuarios revendedores", bFlAtiva = true },
                    new() { nCdFuncionalidade = 34, sNmFuncionalidade = "Visualizar todos os estoques", sDsFuncionalidade = "Visualizar todos os estoques do sistema.", bFlAtiva = true }
                };

                foreach (var func in funcionalidades)
                {
                    var exists = context.Funcionalidade.Any(f => f.sNmFuncionalidade == func.sNmFuncionalidade);
                    if (!exists) context.Funcionalidade.Add(func);
                }

                context.SaveChanges();
            }
        }

        return context;
    }
}
