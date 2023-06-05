using MinhasVendas.App.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MinhasVendas.App.Models
{
    public class TransacaoDeEstoque
    {
        public int Id { get; set; }

        
        public int ProdutoId { get; set; }
        public int? OrdemDeCompraId { get; set; }
        public int? OrdemDeVendaId { get; set; }

        public int Quantidade { get; set; }
        public TipoDransacaoDeEstoque TipoDransacaoDeEstoque { get; set; }

        /*Ef Relacionamento */
        public Produto? Produto { get; set; }
        public OrdemDeCompra? OrdemDeCompras { get; set; }
        public OrdemDeVenda? OrdemDeVendas { get; set; }

    }
}
