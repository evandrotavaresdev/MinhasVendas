using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.Models
{
    public class DetalheDeCompra : Entidade
    {
        //public int Id { get; set; }

        public int ProdutoId { get; set; }
        public int OrdemDeCompraId { get; set; }
        public int TransacaoDeEstoqueId { get; set; }
        [Range(1, 1000, ErrorMessage = "Valor Inválido")]
        public int Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
        public DateTime? DataDeRecebimento {  get; set; }
        public bool RegistradoTransacaoDeEstoque { get; set; }

        /* EF Relacionamento */
        public Produto? Produto { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }

    }
}
