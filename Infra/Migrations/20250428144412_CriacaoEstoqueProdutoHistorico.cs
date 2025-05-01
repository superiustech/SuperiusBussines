using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoEstoqueProdutoHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "bFlAtivo",
                table: "ESTOQUE_PRODUTO",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ESTOQUE_PRODUTO_HISTORICO",
                columns: table => new
                {
                    nCdEstoqueProdutoHistorico = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nCdEstoque = table.Column<int>(type: "integer", nullable: false),
                    nCdProduto = table.Column<int>(type: "integer", nullable: false),
                    tDtMovimentacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    dQtMovimentada = table.Column<int>(type: "integer", nullable: false),
                    nTipoMovimentacao = table.Column<int>(type: "integer", nullable: false),
                    sDsObservacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    nCdEstoqueDestino = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTOQUE_PRODUTO_HISTORICO", x => x.nCdEstoqueProdutoHistorico);
                    table.ForeignKey(
                        name: "FK_ESTOQUE_PRODUTO_HISTORICO_ESTOQUE_PRODUTO_nCdEstoque_nCdPro~",
                        columns: x => new { x.nCdEstoque, x.nCdProduto },
                        principalTable: "ESTOQUE_PRODUTO",
                        principalColumns: new[] { "nCdEstoque", "nCdProduto" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESTOQUE_PRODUTO_HISTORICO_nCdEstoque_nCdProduto",
                table: "ESTOQUE_PRODUTO_HISTORICO",
                columns: new[] { "nCdEstoque", "nCdProduto" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESTOQUE_PRODUTO_HISTORICO");

            migrationBuilder.DropColumn(
                name: "bFlAtivo",
                table: "ESTOQUE_PRODUTO");
        }
    }
}
