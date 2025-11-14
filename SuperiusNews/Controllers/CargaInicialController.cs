using Domain.Entities;
using Domain.Interfaces;
using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/master")]
public class CargaInicialController : ControllerBase
{
    private readonly ApplicationDbContextMaster _masterDb;
    private readonly IRuntimeDbContextFactory<ApplicationDbContext> _tenantDbFactory;

    public CargaInicialController(ApplicationDbContextMaster masterDb,
                                 IRuntimeDbContextFactory<ApplicationDbContext> tenantDbFactory)
    {
        _masterDb = masterDb;
        _tenantDbFactory = tenantDbFactory;
    }
    [AllowAnonymous]
    [HttpPost("criar-cliente")]
    public async Task<IActionResult> CriarCliente([FromBody] NovoClienteDTO dto)
    {
        // 🔹 Verifica se já existe na master
        if (await _masterDb.Cliente.AnyAsync(c => c.sNmCliente == dto.NomeCliente))
            return BadRequest("Cliente já existe.");

        // 🔹 Cria cliente na master
        var cliente = new CWCliente
        {
            sNmCliente = dto.NomeCliente
        };
        _masterDb.Cliente.Add(cliente);
        await _masterDb.SaveChangesAsync();

        // 🔹 Cria usuário admin master
        var usuario = new CWClienteUsuario
        {
            nCdCliente = cliente.nCdCliente,
            sCdUsuario = dto.AdminLogin
        };
        _masterDb.ClienteUsuario.Add(usuario);
        await _masterDb.SaveChangesAsync();

        // 🔹 Cria tenant e popula carga inicial
        using var tenantDb = _tenantDbFactory.CreateDbContext(cliente.sNmCliente);

        // Funcionalidades
        if (!tenantDb.Funcionalidade.Any())
        {
            tenantDb.Funcionalidade.AddRange(FuncoesPadrao.GetFuncionalidades());
        }

        // Permissões
        if (!tenantDb.Permissao.Any())
        {
            tenantDb.Permissao.AddRange(FuncoesPadrao.GetPermissoes());
        }

        // Perfis
        if (!tenantDb.Perfil.Any())
        {
            tenantDb.Perfil.AddRange(FuncoesPadrao.GetPerfis());
        }

        // Associações Funcionalidade <-> Permissão
        if (!tenantDb.FuncionalidadePermissao.Any())
        {
            tenantDb.FuncionalidadePermissao.AddRange(FuncoesPadrao.GetFuncionalidadePermissao());
        }

        // Associações Permissão <-> Perfil
        if (!tenantDb.PermissaoPerfil.Any())
        {
            tenantDb.PermissaoPerfil.AddRange(FuncoesPadrao.GetPermissaoPerfil());
        }

        // Usuário admin do tenant
        await tenantDb.SaveChangesAsync();

        // 🔹 Cria usuário admin no tenant
        if (!tenantDb.Usuario.Any(u => u.sCdUsuario == dto.AdminLogin))
        {
            tenantDb.Usuario.Add(new CWUsuario
            {
                sCdUsuario = dto.AdminLogin,
                sSenha = dto.AdminSenha, // aqui você pode hash se quiser segurança
                sNmUsuario = "Administrador", // ou dto.NomeCliente
                 sEmail = "teste@teste.com",
            });

            await tenantDb.SaveChangesAsync();
        }

        // 🔹 Associa o usuário admin ao perfil Admin (nCdPerfil = 1)
        var admin = await tenantDb.Usuario
            .FirstOrDefaultAsync(u => u.sCdUsuario == dto.AdminLogin);

        if (admin != null && !tenantDb.PerfilUsuario.Any(up => up.sCdUsuario == admin.sCdUsuario))
        {
            tenantDb.PerfilUsuario.Add(new CWPerfilUsuario
            {
                sCdUsuario = admin.sCdUsuario,
                nCdPerfil = 1
            });

            await tenantDb.SaveChangesAsync();
        }



        return Ok(new { Cliente = cliente.sNmCliente, Admin = dto.AdminLogin });
    }



    public class NovoClienteDTO
    {
        public string NomeCliente { get; set; }
        public string AdminLogin { get; set; }
        public string AdminSenha { get; set; }
    }

    public static class FuncoesPadrao
    {
        // 35 funcionalidades
    public static List<CWFuncionalidade> GetFuncionalidades() => new List<CWFuncionalidade>
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
        new() { nCdFuncionalidade = 34, sNmFuncionalidade = "Visualizar todos os estoques", sDsFuncionalidade = "Visualizar todos os estoques do sistema.", bFlAtiva = true },
        new() { nCdFuncionalidade = 35, sNmFuncionalidade = "Visualizar dashboard", sDsFuncionalidade = "Acessar a tela de dashboard padrão do sistema.", bFlAtiva = true }
    };


        // ============================================================
        // 🔐 PERMISSÕES PADRÃO
        // ============================================================
        public static List<CWPermissao> GetPermissoes() => new List<CWPermissao>
    {
        new() { nCdPermissao = 1, sNmPermissao = "Permissão Admin", sDsPermissao =  "Permissão Admin", bFlAtiva = true },
        new() { nCdPermissao = 2, sNmPermissao = "Gestão de Estoque",sDsPermissao = "Gestão de Estoque", bFlAtiva = true },
        new() { nCdPermissao = 3, sNmPermissao = "Gestão de Revendedores",sDsPermissao = "Gestão de Revendedores", bFlAtiva = true },
        new() { nCdPermissao = 4, sNmPermissao = "Gestão de Usuários e Perfis",sDsPermissao = "Gestão de Usuários e Perfis", bFlAtiva = true },
        new() { nCdPermissao = 5, sNmPermissao = "Visualização Geral",sDsPermissao = "Visualização Geral", bFlAtiva = true }
    };

        // ============================================================
        // 👥 PERFIS PADRÃO
        // ============================================================
        public static List<CWPerfil> GetPerfis() => new List<CWPerfil>
    {
        new() { nCdPerfil = 1, sNmPerfil = "Admin", sDsPerfil = "Admin"},
        new() { nCdPerfil = 2, sNmPerfil = "Estoque", sDsPerfil = "Estoque"},
        new() { nCdPerfil = 3, sNmPerfil = "Revendedor", sDsPerfil = "Revendedor" },
        new() { nCdPerfil = 4, sNmPerfil = "Visualizador", sDsPerfil = "Visualizador"}
    };

        // ============================================================
        // ⚙️ FUNCIONALIDADE → PERMISSÃO
        // ============================================================
        public static List<CWFuncionalidadePermissao> GetFuncionalidadePermissao()
        {
            var lista = new List<CWFuncionalidadePermissao>();

            // --- Permissão 2: Gestão de Estoque ---
            int[] estoqueFuncs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 34, 35 };
            foreach (var f in estoqueFuncs)
                lista.Add(new CWFuncionalidadePermissao { nCdFuncionalidade = f, nCdPermissao = 2 });

            // --- Permissão 3: Gestão de Revendedores ---
            int[] revFuncs = { 11, 12, 13, 33 };
            foreach (var f in revFuncs)
                lista.Add(new CWFuncionalidadePermissao { nCdFuncionalidade = f, nCdPermissao = 3 });

            // --- Permissão 4: Gestão de Usuários e Perfis ---
            int[] userFuncs = { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23,
                            24, 25, 26, 27, 28, 29, 30, 31, 32 };
            foreach (var f in userFuncs)
                lista.Add(new CWFuncionalidadePermissao { nCdFuncionalidade = f, nCdPermissao = 4 });

            // --- Permissão 5: Visualização Geral ---
            int[] visualFuncs = { 4, 8, 11, 14, 19, 24, 28, 34, 35 };
            foreach (var f in visualFuncs)
                lista.Add(new CWFuncionalidadePermissao { nCdFuncionalidade = f, nCdPermissao = 5 });

            // --- Permissão 1: Admin (tudo) ---
            for (int i = 1; i <= 35; i++)
                lista.Add(new CWFuncionalidadePermissao { nCdFuncionalidade = i, nCdPermissao = 1 });

            return lista;
        }

        // ============================================================
        // 🔗 PERMISSÃO → PERFIL
        // ============================================================
        public static List<CWPermissaoPerfil> GetPermissaoPerfil() => new List<CWPermissaoPerfil>
    {
        // Admin: vê tudo
        new() { nCdPermissao = 1, nCdPerfil = 1 },
        new() { nCdPermissao = 2, nCdPerfil = 1 },
        new() { nCdPermissao = 3, nCdPerfil = 1 },
        new() { nCdPermissao = 4, nCdPerfil = 1 },
        new() { nCdPermissao = 5, nCdPerfil = 1 },

        // Estoquista
        new() { nCdPermissao = 2, nCdPerfil = 2 },

        // Revendedor
        new() { nCdPermissao = 3, nCdPerfil = 3 },

        // Visualizador
        new() { nCdPermissao = 5, nCdPerfil = 4 }
    };
    }
}
