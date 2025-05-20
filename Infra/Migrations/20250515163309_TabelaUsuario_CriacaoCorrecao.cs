using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class TabelaUsuario_CriacaoCorrecao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PERFIL_USUARIO_Usuario_CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO");

            migrationBuilder.DropForeignKey(
                name: "FK_PERFIL_USUARIO_Usuario_sCdUsuario",
                table: "PERFIL_USUARIO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_PERFIL_USUARIO_CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO");

            migrationBuilder.DropColumn(
                name: "CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "USUARIO");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USUARIO",
                table: "USUARIO",
                column: "sCdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_PERFIL_USUARIO_USUARIO_sCdUsuario",
                table: "PERFIL_USUARIO",
                column: "sCdUsuario",
                principalTable: "USUARIO",
                principalColumn: "sCdUsuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PERFIL_USUARIO_USUARIO_sCdUsuario",
                table: "PERFIL_USUARIO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USUARIO",
                table: "USUARIO");

            migrationBuilder.RenameTable(
                name: "USUARIO",
                newName: "Usuario");

            migrationBuilder.AddColumn<string>(
                name: "CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "sCdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PERFIL_USUARIO_CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO",
                column: "CWUsuariosCdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_PERFIL_USUARIO_Usuario_CWUsuariosCdUsuario",
                table: "PERFIL_USUARIO",
                column: "CWUsuariosCdUsuario",
                principalTable: "Usuario",
                principalColumn: "sCdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_PERFIL_USUARIO_Usuario_sCdUsuario",
                table: "PERFIL_USUARIO",
                column: "sCdUsuario",
                principalTable: "Usuario",
                principalColumn: "sCdUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
