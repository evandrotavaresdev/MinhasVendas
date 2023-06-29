namespace MinhasVendas.App.Models
{
    public class SaldoDeEstoque
    {
        public int Id { get; set; }
        public string? NomeProduto { get; set; }
        public decimal Preco { get; set; }
        public int EstoqueAtual { get; set; }
        public string? ProdutoCompleto { get; set; }

    }
}
