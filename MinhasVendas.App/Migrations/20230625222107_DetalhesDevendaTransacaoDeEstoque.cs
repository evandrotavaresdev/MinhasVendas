using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    public partial class DetalhesDevendaTransacaoDeEstoque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RegistroTransacaoDeEstoque",
                table: "DetalheDeVendas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistroTransacaoDeEstoque",
                table: "DetalheDeVendas");
        }
    }
}
