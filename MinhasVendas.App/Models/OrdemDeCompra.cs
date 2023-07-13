using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Models
{
    public class OrdemDeCompra : Entidade
    {
        //public int Id { get; set; }

        public int FornecedorId { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public StatusOrdemDeCompra StatusOrdemDeCompra { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }
        public Decimal ValorDeFrete { get; set; }


        /* Ef Relacionamento */
        public Fornecedor? Fornecedor { get; set; }
        public ICollection<DetalheDeCompra>? DetalheDeCompras { get; set; }
       
    }
}
