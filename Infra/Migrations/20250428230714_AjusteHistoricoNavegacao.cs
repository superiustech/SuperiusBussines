using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class AjusteHistoricoNavegacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ESTOQUE_PRODUTO_HISTORICO_nCdEstoqueDestino",
                table: "ESTOQUE_PRODUTO_HISTORICO",
                column: "nCdEstoqueDestino");

            migrationBuilder.CreateIndex(
                name: "IX_ESTOQUE_PRODUTO_HISTORICO_nCdProduto",
                table: "ESTOQUE_PRODUTO_HISTORICO",
                column: "nCdProduto");

            migrationBuilder.AddForeignKey(
                name: "FK_ESTOQUE_PRODUTO_HISTORICO_ESTOQUE_nCdEstoque",
                table: "ESTOQUE_PRODUTO_HISTORICO",
                column: "nCdEstoque",
                principalTable: "ESTOQUE",
                principalColumn: "nCdEstoque",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ESTOQUE_PRODUTO_HISTORICO_ESTOQUE_nCdEstoqueDestino",
                table: "ESTOQUE_PRODUTO_HISTORICO",
                column: "nCdEstoqueDestino",
                principalTable: "ESTOQUE",
                principalColumn: "nCdEstoque",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ESTOQUE_PRODUTO_HISTORICO_PRODUTO_nCdProduto",
                table: "ESTOQUE_PRODUTO_HISTORICO",
                column: "nCdProduto",
                principalTable: "PRODUTO",
                principalColumn: "nCdProduto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ESTOQUE_PRODUTO_HISTORICO_ESTOQUE_nCdEstoque",
                table: "ESTOQUE_PRODUTO_HISTORICO");

            migrationBuilder.DropForeignKey(
                name: "FK_ESTOQUE_PRODUTO_HISTORICO_ESTOQUE_nCdEstoqueDestino",
                table: "ESTOQUE_PRODUTO_HISTORICO");

            migrationBuilder.DropForeignKey(
                name: "FK_ESTOQUE_PRODUTO_HISTORICO_PRODUTO_nCdProduto",
                table: "ESTOQUE_PRODUTO_HISTORICO");

            migrationBuilder.DropIndex(
                name: "IX_ESTOQUE_PRODUTO_HISTORICO_nCdEstoqueDestino",
                table: "ESTOQUE_PRODUTO_HISTORICO");

            migrationBuilder.DropIndex(
                name: "IX_ESTOQUE_PRODUTO_HISTORICO_nCdProduto",
                table: "ESTOQUE_PRODUTO_HISTORICO");
        }
    }
}
