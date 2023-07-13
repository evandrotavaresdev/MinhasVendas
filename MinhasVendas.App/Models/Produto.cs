namespace MinhasVendas.App.Models
{
    public class Produto : Entidade
    {
        //public int Id { get; set; }

        public string? Nome { get; set; }
        public decimal PrecoDeLista { get; set; }
        public decimal PrecoBase { get; set; }
        public int EstoqueAtual { get; set; }

        /* Ef Relacionamento */
        ICollection<DetalheDeVenda>? DetalheDeVendas { get; set; }
        ICollection<DetalheDeCompra>? DetalheDeCompras { get; set; }
        ICollection<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }
    }
}
