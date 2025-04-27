using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlterarFKRevendedorParaSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoOpcaoVariacao_PRODUTO_nCdProduto",
                table: "ProdutoOpcaoVariacao");

            migrationBuilder.DropForeignKey(
                name: "FK_REVENDEDOR_ESTOQUE_nCdEstoque",
                table: "REVENDEDOR");

            migrationBuilder.DropForeignKey(
                name: "FK_REVENDEDOR_REVENDEDOR_TIPO_nCdTipoRevendedor",
                table: "REVENDEDOR");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoOpcaoVariacao_PRODUTO_nCdProduto",
                table: "ProdutoOpcaoVariacao",
                column: "nCdProduto",
                principalTable: "PRODUTO",
                principalColumn: "nCdProduto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_REVENDEDOR_ESTOQUE_nCdEstoque",
                table: "REVENDEDOR",
                column: "nCdEstoque",
                principalTable: "ESTOQUE",
                principalColumn: "nCdEstoque",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_REVENDEDOR_REVENDEDOR_TIPO_nCdTipoRevendedor",
                table: "REVENDEDOR",
                column: "nCdTipoRevendedor",
                principalTable: "REVENDEDOR_TIPO",
                principalColumn: "nCdTipoRevendedor",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoOpcaoVariacao_PRODUTO_nCdProduto",
                table: "ProdutoOpcaoVariacao");

            migrationBuilder.DropForeignKey(
                name: "FK_REVENDEDOR_ESTOQUE_nCdEstoque",
                table: "REVENDEDOR");

            migrationBuilder.DropForeignKey(
                name: "FK_REVENDEDOR_REVENDEDOR_TIPO_nCdTipoRevendedor",
                table: "REVENDEDOR");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoOpcaoVariacao_PRODUTO_nCdProduto",
                table: "ProdutoOpcaoVariacao",
                column: "nCdProduto",
                principalTable: "PRODUTO",
                principalColumn: "nCdProduto");

            migrationBuilder.AddForeignKey(
                name: "FK_REVENDEDOR_ESTOQUE_nCdEstoque",
                table: "REVENDEDOR",
                column: "nCdEstoque",
                principalTable: "ESTOQUE",
                principalColumn: "nCdEstoque",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_REVENDEDOR_REVENDEDOR_TIPO_nCdTipoRevendedor",
                table: "REVENDEDOR",
                column: "nCdTipoRevendedor",
                principalTable: "REVENDEDOR_TIPO",
                principalColumn: "nCdTipoRevendedor",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
