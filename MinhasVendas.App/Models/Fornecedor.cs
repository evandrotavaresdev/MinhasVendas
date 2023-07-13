namespace MinhasVendas.App.Models
{
    public class Fornecedor : Entidade
    {
        //public int Id { get; set; }

        public string? Nome { get; set; }

        /* Ef Relacionamentos */
        ICollection<OrdemDeCompra>? OrdemDeCompras { get; set; }

    }
}
