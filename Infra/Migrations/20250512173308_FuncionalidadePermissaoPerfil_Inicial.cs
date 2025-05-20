using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class FuncionalidadePermissaoPerfil_Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Usuario",
                newName: "sEmail");

            migrationBuilder.AddColumn<string>(
                name: "sCdUsuario",
                table: "Usuario",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<string>(
                name: "sNmUsuario",
                table: "Usuario",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sSenha",
                table: "Usuario",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "sCdUsuario");

            migrationBuilder.CreateTable(
                name: "FUNCIONALIDADE",
                columns: table => new
                {
                    nCdFuncionalidade = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmFuncionalidade = table.Column<string>(type: "text", nullable: false),
                    sDsFuncionalidade = table.Column<string>(type: "text", nullable: false),
                    bFlAtiva = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCIONALIDADE", x => x.nCdFuncionalidade);
                });

            migrationBuilder.CreateTable(
                name: "PERFIL",
                columns: table => new
                {
                    nCdPerfil = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmPerfil = table.Column<string>(type: "text", nullable: false),
                    sDsPerfil = table.Column<string>(type: "text", nullable: false),
                    bFlAtiva = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIL", x => x.nCdPerfil);
                });

            migrationBuilder.CreateTable(
                name: "PERFIL_USUARIO",
                columns: table => new
                {
                    nCdPerfil = table.Column<int>(type: "integer", nullable: false),
                    sCdUsuario = table.Column<string>(type: "text", nullable: false),
                    CWUsuariosCdUsuario = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFIL_USUARIO", x => new { x.nCdPerfil, x.sCdUsuario });
                    table.ForeignKey(
                        name: "FK_PERFIL_USUARIO_PERFIL_nCdPerfil",
                        column: x => x.nCdPerfil,
                        principalTable: "PERFIL",
                        principalColumn: "nCdPerfil",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERFIL_USUARIO_Usuario_CWUsuariosCdUsuario",
                        column: x => x.CWUsuariosCdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "sCdUsuario");
                    table.ForeignKey(
                        name: "FK_PERFIL_USUARIO_Usuario_sCdUsuario",
                        column: x => x.sCdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "sCdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PERMISSAO",
                columns: table => new
                {
                    nCdPermissao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmPermissao = table.Column<string>(type: "text", nullable: false),
                    sDsPermissao = table.Column<string>(type: "text", nullable: false),
                    bFlAtiva = table.Column<bool>(type: "boolean", nullable: false),
                    CWPerfilnCdPerfil = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSAO", x => x.nCdPermissao);
                    table.ForeignKey(
                        name: "FK_PERMISSAO_PERFIL_CWPerfilnCdPerfil",
                        column: x => x.CWPerfilnCdPerfil,
                        principalTable: "PERFIL",
                        principalColumn: "nCdPerfil");
                });

            migrationBuilder.CreateTable(
                name: "FUNCIONALIDADE_PERMISSAO",
                columns: table => new
                {
                    nCdFuncionalidade = table.Column<int>(type: "integer", nullable: false),
                    nCdPermissao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCIONALIDADE_PERMISSAO", x => new { x.nCdFuncionalidade, x.nCdPermissao });
                    table.ForeignKey(
                        name: "FK_FUNCIONALIDADE_PERMISSAO_FUNCIONALIDADE_nCdFuncionalidade",
                        column: x => x.nCdFuncionalidade,
                        principalTable: "FUNCIONALIDADE",
                        principalColumn: "nCdFuncionalidade",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FUNCIONALIDADE_PERMISSAO_PERMISSAO_nCdPermissao",
                        column: x => x.nCdPermissao,
                        principalTable: "PERMISSAO",
                        principalColumn: "nCdPermissao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PERMISSAO_PERFIL",
                columns: table => new
                {
                    nCdPermissao = table.Column<int>(type: "integer", nullable: false),
                    nCdPerfil = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSAO_PERFIL", x => new { x.nCdPermissao, x.nCdPerfil });
                    table.ForeignKey(
                        name: "FK_PERMISSAO_PERFIL_PERFIL_nCdPerfil",
                        column: x => x.nCdPerfil,
                        principalTable: "PERFIL",
                        principalColumn: "nCdPerfil",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PERMISSAO_PERFIL_PERMISSAO_nCdPermissao",
                        column: x => x.nCdPermissao,
                        principalTable: "PERMISSAO",
                        principalColumn: "nCdPermissao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FUNCIONALIDADE_PERMISSAO_nCdPermissao",
                table: "FUNCIONALIDADE_PERMISSAO",
                column: "nCdPermissao");

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_USUARIO_CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO",
                column: "CWUsuariosCdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_USUARIO_sCdUsuario",
                table: "PERFIL_USUARIO",
                column: "sCdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSAO_CWPerfilnCdPerfil",
                table: "PERMISSAO",
                column: "CWPerfilnCdPerfil");

            migrationBuilder.CreateIndex(
                name: "IX_PERMISSAO_PERFIL_nCdPerfil",
                table: "PERMISSAO_PERFIL",
                column: "nCdPerfil");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FUNCIONALIDADE_PERMISSAO");

            migrationBuilder.DropTable(
                name: "PERFIL_USUARIO");

            migrationBuilder.DropTable(
                name: "PERMISSAO_PERFIL");

            migrationBuilder.DropTable(
                name: "FUNCIONALIDADE");

            migrationBuilder.DropTable(
                name: "PERMISSAO");

            migrationBuilder.DropTable(
                name: "PERFIL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "sCdUsuario",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "sNmUsuario",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "sSenha",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "sEmail",
                table: "Usuario",
                newName: "Senha");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuario",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Usuario",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");
        }
    }
}
