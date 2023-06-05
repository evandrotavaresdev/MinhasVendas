namespace MinhasVendas.App.Models
{
    public class DetalheDeVenda
    {
        public int Id { get; set; }

        public int OrdemDeVendaId { get; set; }
        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Desconto { get; set; }

        /* Ef Relacionamento*/
        public OrdemDeVenda? OrdemDeVenda { get; set; }
        public Produto? Produto { get; set; }

    }
}
