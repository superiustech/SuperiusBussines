using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class TabelaRevendedorUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "REVENDEDOR_USUARIO",
                columns: table => new
                {
                    nCdRevendedor = table.Column<int>(type: "integer", nullable: false),
                    sCdUsuario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVENDEDOR_USUARIO", x => new { x.nCdRevendedor, x.sCdUsuario });
                    table.ForeignKey(
                        name: "FK_REVENDEDOR_USUARIO_REVENDEDOR_nCdRevendedor",
                        column: x => x.nCdRevendedor,
                        principalTable: "REVENDEDOR",
                        principalColumn: "nCdRevendedor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_REVENDEDOR_USUARIO_USUARIO_sCdUsuario",
                        column: x => x.sCdUsuario,
                        principalTable: "USUARIO",
                        principalColumn: "sCdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REVENDEDOR_USUARIO_sCdUsuario",
                table: "REVENDEDOR_USUARIO",
                column: "sCdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REVENDEDOR_USUARIO");
        }
    }
}
