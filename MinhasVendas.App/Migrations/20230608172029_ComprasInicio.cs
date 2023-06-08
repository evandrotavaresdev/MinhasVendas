using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    public partial class ComprasInicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataDeCriacao",
                table: "OrdemDeCompras",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StatusOrdemDeCompra",
                table: "OrdemDeCompras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDeFrete",
                table: "OrdemDeCompras",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDeRecebimento",
                table: "DetalheDeCompras",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "RegistradoTransacaoDeEstoque",
                table: "DetalheDeCompras",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataDeCriacao",
                table: "OrdemDeCompras");

            migrationBuilder.DropColumn(
                name: "StatusOrdemDeCompra",
                table: "OrdemDeCompras");

            migrationBuilder.DropColumn(
                name: "ValorDeFrete",
                table: "OrdemDeCompras");

            migrationBuilder.DropColumn(
                name: "DataDeRecebimento",
                table: "DetalheDeCompras");

            migrationBuilder.DropColumn(
                name: "RegistradoTransacaoDeEstoque",
                table: "DetalheDeCompras");
        }
    }
}
