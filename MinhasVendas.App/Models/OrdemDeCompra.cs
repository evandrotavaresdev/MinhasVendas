namespace MinhasVendas.App.Models
{
    public class OrdemDeCompra
    {
        public int Id { get; set; }

        public int FornecedorId { get; set; }

        /* Ef Relacionamento */
        public Fornecedor? Fornecedor { get; set; }
       
    }
}
