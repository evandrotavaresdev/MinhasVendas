namespace MinhasVendas.App.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string? Nome { get; set; }   
        
        /* Ef Relacionamento */
        ICollection<OrdemDeVenda>? OrdemDeVendas { get; set; }
    }
}
