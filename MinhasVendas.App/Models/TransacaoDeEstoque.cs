using MinhasVendas.App.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MinhasVendas.App.Models
{
    public class TransacaoDeEstoque : Entidade
    {
        //public int Id { get; set; }

        
        public int ProdutoId { get; set; }
        public int? OrdemDeCompraId { get; set; }
        public int? OrdemDeVendaId { get; set; }

        public int Quantidade { get; set; }
        public TipoDransacaoDeEstoque TipoDransacaoDeEstoque { get; set; }
        public DateTime DataDeTransacao { get; set; }

        /*Ef Relacionamento */
        public Produto? Produto { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }
        public OrdemDeVenda? OrdemDeVenda { get; set; }

    }
}
