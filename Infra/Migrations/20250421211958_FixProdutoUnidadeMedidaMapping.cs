using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class FixProdutoUnidadeMedidaMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUTO_UNIDADE_MEDIDA_UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO");

            migrationBuilder.DropForeignKey(
                name: "FK_PRODUTO_UNIDADE_MEDIDA_nCdUnidadeMedida",
                table: "PRODUTO");

            migrationBuilder.DropIndex(
                name: "IX_PRODUTO_UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO");

            migrationBuilder.DropColumn(
                name: "UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUTO_UNIDADE_MEDIDA_nCdUnidadeMedida",
                table: "PRODUTO",
                column: "nCdUnidadeMedida",
                principalTable: "UNIDADE_MEDIDA",
                principalColumn: "nCdUnidadeMedida",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUTO_UNIDADE_MEDIDA_nCdUnidadeMedida",
                table: "PRODUTO");

            migrationBuilder.AddColumn<int>(
                name: "UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO",
                column: "UnidadeMedidanCdUnidadeMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUTO_UNIDADE_MEDIDA_UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO",
                column: "UnidadeMedidanCdUnidadeMedida",
                principalTable: "UNIDADE_MEDIDA",
                principalColumn: "nCdUnidadeMedida",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUTO_UNIDADE_MEDIDA_nCdUnidadeMedida",
                table: "PRODUTO",
                column: "nCdUnidadeMedida",
                principalTable: "UNIDADE_MEDIDA",
                principalColumn: "nCdUnidadeMedida",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
