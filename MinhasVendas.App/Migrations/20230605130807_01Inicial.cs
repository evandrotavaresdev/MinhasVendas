using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    public partial class _01Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecoDeLista = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdemDeVendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    StatusOrdemDeVenda = table.Column<int>(type: "int", nullable: false),
                    FormaDePagamento = table.Column<int>(type: "int", nullable: false),
                    DataDePagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeVenda = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemDeVendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdemDeVendas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdemDeCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FornecedorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemDeCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdemDeCompras_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalheDeVendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdemDeVendaId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalheDeVendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalheDeVendas_OrdemDeVendas_OrdemDeVendaId",
                        column: x => x.OrdemDeVendaId,
                        principalTable: "OrdemDeVendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalheDeVendas_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalheDeCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    OrdemDeCompraId = table.Column<int>(type: "int", nullable: false),
                    TransacaoDeEstoqueId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    CustoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalheDeCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalheDeCompras_OrdemDeCompras_OrdemDeCompraId",
                        column: x => x.OrdemDeCompraId,
                        principalTable: "OrdemDeCompras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalheDeCompras_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransacaoDeEstoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    OrdemDeCompraId = table.Column<int>(type: "int", nullable: true),
                    OrdemDeVendaId = table.Column<int>(type: "int", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    TipoDransacaoDeEstoque = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacaoDeEstoques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransacaoDeEstoques_OrdemDeCompras_OrdemDeCompraId",
                        column: x => x.OrdemDeCompraId,
                        principalTable: "OrdemDeCompras",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransacaoDeEstoques_OrdemDeVendas_OrdemDeVendaId",
                        column: x => x.OrdemDeVendaId,
                        principalTable: "OrdemDeVendas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransacaoDeEstoques_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalheDeCompras_OrdemDeCompraId",
                table: "DetalheDeCompras",
                column: "OrdemDeCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalheDeCompras_ProdutoId",
                table: "DetalheDeCompras",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalheDeVendas_OrdemDeVendaId",
                table: "DetalheDeVendas",
                column: "OrdemDeVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalheDeVendas_ProdutoId",
                table: "DetalheDeVendas",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemDeCompras_FornecedorId",
                table: "OrdemDeCompras",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemDeVendas_ClienteId",
                table: "OrdemDeVendas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoDeEstoques_OrdemDeCompraId",
                table: "TransacaoDeEstoques",
                column: "OrdemDeCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoDeEstoques_OrdemDeVendaId",
                table: "TransacaoDeEstoques",
                column: "OrdemDeVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoDeEstoques_ProdutoId",
                table: "TransacaoDeEstoques",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalheDeCompras");

            migrationBuilder.DropTable(
                name: "DetalheDeVendas");

            migrationBuilder.DropTable(
                name: "TransacaoDeEstoques");

            migrationBuilder.DropTable(
                name: "OrdemDeCompras");

            migrationBuilder.DropTable(
                name: "OrdemDeVendas");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
