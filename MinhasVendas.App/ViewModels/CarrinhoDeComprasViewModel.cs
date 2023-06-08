using MinhasVendas.App.Models;

namespace MinhasVendas.App.ViewModels
{
    public class CarrinhoDeComprasViewModel
    {
        public Produto? Produto { get; set; }
        public DetalheDeCompra? DetalheDeCompra { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }

        public decimal TotalProduto { get; set; }

        public decimal TotalCompra { get; set; }

        public int TotalItens { get; set; }
    }
}
