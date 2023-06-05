using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Models
{
    public class OrdemDeVenda
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
       
        public StatusOrdemDeVenda StatusOrdemDeVenda { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }
        public DateTime DataDePagamento { get; set; }
        public DateTime DataDeVenda { get; set; }
       
        /* Ef Relacionamento */
        ICollection<DetalheDeVenda>? DetalheDeVendas { get; set; }
        ICollection<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }

        public Cliente? Cliente { get; set; }
    }
}
