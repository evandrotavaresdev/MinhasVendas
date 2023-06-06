using MinhasVendas.App.Models;

namespace MinhasVendas.App.ViewModels
{
    public class CarrinhoDeVendasViewModel
    {
        public Produto? Produto { get; set; }
        public DetalheDeVenda? DetalheDeVenda { get; set; }
        public OrdemDeVenda? OrdemDeVenda { get; set; }

        public decimal TotalProduto { get; set; }

        public decimal TotalVenda { get; set; }

        public int TotalItens { get; set; }
    }
}
