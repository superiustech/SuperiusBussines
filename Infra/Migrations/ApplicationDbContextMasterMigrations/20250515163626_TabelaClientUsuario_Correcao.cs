using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations.ApplicationDbContextMasterMigrations
{
    /// <inheritdoc />
    public partial class TabelaClientUsuario_Correcao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bFlAtivo",
                table: "CLIENTE_USUARIO");

            migrationBuilder.DropColumn(
                name: "sSenha",
                table: "CLIENTE_USUARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "bFlAtivo",
                table: "CLIENTE_USUARIO",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "sSenha",
                table: "CLIENTE_USUARIO",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
