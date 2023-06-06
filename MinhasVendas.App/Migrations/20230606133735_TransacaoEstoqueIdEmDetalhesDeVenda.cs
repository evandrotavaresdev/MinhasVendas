﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    public partial class TransacaoEstoqueIdEmDetalhesDeVenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataDeTransacao",
                table: "TransacaoDeEstoques",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TransacaoDeEstoqueId",
                table: "DetalheDeVendas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataDeTransacao",
                table: "TransacaoDeEstoques");

            migrationBuilder.DropColumn(
                name: "TransacaoDeEstoqueId",
                table: "DetalheDeVendas");
        }
    }
}
