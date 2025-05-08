using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations.ApplicationDbContextMasterMigrations
{
    /// <inheritdoc />
    public partial class InitialCreate_MasterContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLIENTE",
                columns: table => new
                {
                    nCdCliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmCliente = table.Column<string>(type: "text", nullable: true),
                    bFlAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE", x => x.nCdCliente);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTE_USUARIO",
                columns: table => new
                {
                    sCdUsuario = table.Column<string>(type: "text", nullable: false),
                    sSenha = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    bFlAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    nCdCliente = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE_USUARIO", x => x.sCdUsuario);
                    table.ForeignKey(
                        name: "FK_CLIENTE_USUARIO_CLIENTE_nCdCliente",
                        column: x => x.nCdCliente,
                        principalTable: "CLIENTE",
                        principalColumn: "nCdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_USUARIO_nCdCliente",
                table: "CLIENTE_USUARIO",
                column: "nCdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLIENTE_USUARIO");

            migrationBuilder.DropTable(
                name: "CLIENTE");
        }
    }
}
