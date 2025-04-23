using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class TesteInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESTOQUE",
                columns: table => new
                {
                    nCdEstoque = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmEstoque = table.Column<string>(type: "text", nullable: false),
                    sDsEstoque = table.Column<string>(type: "text", nullable: false),
                    sCdEstoque = table.Column<string>(type: "text", nullable: false),
                    sDsRua = table.Column<string>(type: "text", nullable: false),
                    sDsComplemento = table.Column<string>(type: "text", nullable: false),
                    sNrNumero = table.Column<string>(type: "text", nullable: false),
                    sCdCep = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTOQUE", x => x.nCdEstoque);
                });

            migrationBuilder.CreateTable(
                name: "REVENDEDOR_TIPO",
                columns: table => new
                {
                    nCdTipoRevendedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sDsTipo = table.Column<string>(type: "text", nullable: false),
                    sNmTipo = table.Column<string>(type: "text", nullable: false),
                    bFlAtivo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVENDEDOR_TIPO", x => x.nCdTipoRevendedor);
                });

            migrationBuilder.CreateTable(
                name: "UNIDADE_MEDIDA",
                columns: table => new
                {
                    nCdUnidadeMedida = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sCdUnidadeMedida = table.Column<string>(type: "text", nullable: false),
                    sDsUnidadeMedida = table.Column<string>(type: "text", nullable: false),
                    sSgUnidadeMedida = table.Column<string>(type: "text", nullable: false),
                    bFlAtivo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIDADE_MEDIDA", x => x.nCdUnidadeMedida);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Senha = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VARIACAO",
                columns: table => new
                {
                    nCdVariacao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmVariacao = table.Column<string>(type: "text", nullable: false),
                    sDsVariacao = table.Column<string>(type: "text", nullable: false),
                    bFlAtiva = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VARIACAO", x => x.nCdVariacao);
                });

            migrationBuilder.CreateTable(
                name: "VARIACAO_OPCAO",
                columns: table => new
                {
                    nCdVariacaoOpcao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmVariacaoOpcao = table.Column<string>(type: "text", nullable: false),
                    sDsVariacaoOpcao = table.Column<string>(type: "text", nullable: false),
                    bFlAtiva = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VARIACAO_OPCAO", x => x.nCdVariacaoOpcao);
                });

            migrationBuilder.CreateTable(
                name: "REVENDEDOR",
                columns: table => new
                {
                    nCdRevendedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nCdEstoque = table.Column<int>(type: "integer", nullable: false),
                    nCdTipoRevendedor = table.Column<int>(type: "integer", nullable: false),
                    sNmRevendedor = table.Column<string>(type: "text", nullable: false),
                    dPcRevenda = table.Column<decimal>(type: "numeric", nullable: false),
                    sNrCpfCnpj = table.Column<string>(type: "text", nullable: false),
                    sTelefone = table.Column<string>(type: "text", nullable: false),
                    sEmail = table.Column<string>(type: "text", nullable: false),
                    sDsRua = table.Column<string>(type: "text", nullable: false),
                    sDsComplemento = table.Column<string>(type: "text", nullable: false),
                    sNrNumero = table.Column<string>(type: "text", nullable: false),
                    sCdCep = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVENDEDOR", x => x.nCdRevendedor);
                    table.ForeignKey(
                        name: "FK_REVENDEDOR_ESTOQUE_nCdEstoque",
                        column: x => x.nCdEstoque,
                        principalTable: "ESTOQUE",
                        principalColumn: "nCdEstoque",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_REVENDEDOR_REVENDEDOR_TIPO_nCdTipoRevendedor",
                        column: x => x.nCdTipoRevendedor,
                        principalTable: "REVENDEDOR_TIPO",
                        principalColumn: "nCdTipoRevendedor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    nCdProduto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sNmProduto = table.Column<string>(type: "text", nullable: false),
                    sDsProduto = table.Column<string>(type: "text", nullable: false),
                    sCdProduto = table.Column<string>(type: "text", nullable: false),
                    sUrlVideo = table.Column<string>(type: "text", nullable: false),
                    sLargura = table.Column<string>(type: "text", nullable: false),
                    sComprimento = table.Column<string>(type: "text", nullable: false),
                    sAltura = table.Column<string>(type: "text", nullable: false),
                    sPeso = table.Column<string>(type: "text", nullable: false),
                    dVlVenda = table.Column<decimal>(type: "numeric", nullable: false),
                    dVlUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    nCdUnidadeMedida = table.Column<int>(type: "integer", nullable: false),
                    UnidadeMedidanCdUnidadeMedida = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.nCdProduto);
                    table.ForeignKey(
                        name: "FK_PRODUTO_UNIDADE_MEDIDA_UnidadeMedidanCdUnidadeMedida",
                        column: x => x.UnidadeMedidanCdUnidadeMedida,
                        principalTable: "UNIDADE_MEDIDA",
                        principalColumn: "nCdUnidadeMedida",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRODUTO_UNIDADE_MEDIDA_nCdUnidadeMedida",
                        column: x => x.nCdUnidadeMedida,
                        principalTable: "UNIDADE_MEDIDA",
                        principalColumn: "nCdUnidadeMedida",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VARIACAO_OPCAO_VARIACAO",
                columns: table => new
                {
                    nCdVariacao = table.Column<int>(type: "integer", nullable: false),
                    nCdVariacaoOpcao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VARIACAO_OPCAO_VARIACAO", x => new { x.nCdVariacao, x.nCdVariacaoOpcao });
                    table.ForeignKey(
                        name: "FK_VARIACAO_OPCAO_VARIACAO_VARIACAO_OPCAO_nCdVariacaoOpcao",
                        column: x => x.nCdVariacaoOpcao,
                        principalTable: "VARIACAO_OPCAO",
                        principalColumn: "nCdVariacaoOpcao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VARIACAO_OPCAO_VARIACAO_VARIACAO_nCdVariacao",
                        column: x => x.nCdVariacao,
                        principalTable: "VARIACAO",
                        principalColumn: "nCdVariacao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ESTOQUE_PRODUTO",
                columns: table => new
                {
                    nCdEstoque = table.Column<int>(type: "integer", nullable: false),
                    nCdProduto = table.Column<int>(type: "integer", nullable: false),
                    dQtMinima = table.Column<int>(type: "integer", nullable: false),
                    dQtEstoque = table.Column<int>(type: "integer", nullable: false),
                    dVlVenda = table.Column<decimal>(type: "numeric", nullable: false),
                    dVlCusto = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTOQUE_PRODUTO", x => new { x.nCdEstoque, x.nCdProduto });
                    table.ForeignKey(
                        name: "FK_ESTOQUE_PRODUTO_ESTOQUE_nCdEstoque",
                        column: x => x.nCdEstoque,
                        principalTable: "ESTOQUE",
                        principalColumn: "nCdEstoque",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ESTOQUE_PRODUTO_PRODUTO_nCdProduto",
                        column: x => x.nCdProduto,
                        principalTable: "PRODUTO",
                        principalColumn: "nCdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_IMAGEM",
                columns: table => new
                {
                    nCdImagem = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nCdProduto = table.Column<int>(type: "integer", nullable: false),
                    sDsImagem = table.Column<string>(type: "text", nullable: false),
                    sDsCaminho = table.Column<string>(type: "text", nullable: false),
                    CWProdutonCdProduto = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_IMAGEM", x => x.nCdImagem);
                    table.ForeignKey(
                        name: "FK_PRODUTO_IMAGEM_PRODUTO_CWProdutonCdProduto",
                        column: x => x.CWProdutonCdProduto,
                        principalTable: "PRODUTO",
                        principalColumn: "nCdProduto");
                    table.ForeignKey(
                        name: "FK_PRODUTO_IMAGEM_PRODUTO_nCdProduto",
                        column: x => x.nCdProduto,
                        principalTable: "PRODUTO",
                        principalColumn: "nCdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoOpcaoVariacao",
                columns: table => new
                {
                    nCdProduto = table.Column<int>(type: "integer", nullable: false),
                    nCdVariacaoOpcao = table.Column<int>(type: "integer", nullable: false),
                    nCdVariacao = table.Column<int>(type: "integer", nullable: false),
                    CWProdutonCdProduto = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoOpcaoVariacao", x => new { x.nCdProduto, x.nCdVariacaoOpcao, x.nCdVariacao });
                    table.ForeignKey(
                        name: "FK_ProdutoOpcaoVariacao_PRODUTO_CWProdutonCdProduto",
                        column: x => x.CWProdutonCdProduto,
                        principalTable: "PRODUTO",
                        principalColumn: "nCdProduto");
                    table.ForeignKey(
                        name: "FK_ProdutoOpcaoVariacao_PRODUTO_nCdProduto",
                        column: x => x.nCdProduto,
                        principalTable: "PRODUTO",
                        principalColumn: "nCdProduto");
                    table.ForeignKey(
                        name: "FK_ProdutoOpcaoVariacao_VARIACAO_OPCAO_nCdVariacaoOpcao",
                        column: x => x.nCdVariacaoOpcao,
                        principalTable: "VARIACAO_OPCAO",
                        principalColumn: "nCdVariacaoOpcao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoOpcaoVariacao_VARIACAO_nCdVariacao",
                        column: x => x.nCdVariacao,
                        principalTable: "VARIACAO",
                        principalColumn: "nCdVariacao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESTOQUE_PRODUTO_nCdProduto",
                table: "ESTOQUE_PRODUTO",
                column: "nCdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_nCdUnidadeMedida",
                table: "PRODUTO",
                column: "nCdUnidadeMedida");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_UnidadeMedidanCdUnidadeMedida",
                table: "PRODUTO",
                column: "UnidadeMedidanCdUnidadeMedida");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_IMAGEM_CWProdutonCdProduto",
                table: "PRODUTO_IMAGEM",
                column: "CWProdutonCdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_IMAGEM_nCdProduto",
                table: "PRODUTO_IMAGEM",
                column: "nCdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoOpcaoVariacao_CWProdutonCdProduto",
                table: "ProdutoOpcaoVariacao",
                column: "CWProdutonCdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoOpcaoVariacao_nCdVariacao",
                table: "ProdutoOpcaoVariacao",
                column: "nCdVariacao");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoOpcaoVariacao_nCdVariacaoOpcao",
                table: "ProdutoOpcaoVariacao",
                column: "nCdVariacaoOpcao");

            migrationBuilder.CreateIndex(
                name: "IX_REVENDEDOR_nCdEstoque",
                table: "REVENDEDOR",
                column: "nCdEstoque");

            migrationBuilder.CreateIndex(
                name: "IX_REVENDEDOR_nCdTipoRevendedor",
                table: "REVENDEDOR",
                column: "nCdTipoRevendedor");

            migrationBuilder.CreateIndex(
                name: "IX_VARIACAO_OPCAO_VARIACAO_nCdVariacaoOpcao",
                table: "VARIACAO_OPCAO_VARIACAO",
                column: "nCdVariacaoOpcao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESTOQUE_PRODUTO");

            migrationBuilder.DropTable(
                name: "PRODUTO_IMAGEM");

            migrationBuilder.DropTable(
                name: "ProdutoOpcaoVariacao");

            migrationBuilder.DropTable(
                name: "REVENDEDOR");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "VARIACAO_OPCAO_VARIACAO");

            migrationBuilder.DropTable(
                name: "PRODUTO");

            migrationBuilder.DropTable(
                name: "ESTOQUE");

            migrationBuilder.DropTable(
                name: "REVENDEDOR_TIPO");

            migrationBuilder.DropTable(
                name: "VARIACAO_OPCAO");

            migrationBuilder.DropTable(
                name: "VARIACAO");

            migrationBuilder.DropTable(
                name: "UNIDADE_MEDIDA");
        }
    }
}
