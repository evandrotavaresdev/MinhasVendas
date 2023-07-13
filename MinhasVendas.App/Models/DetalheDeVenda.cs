using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.Models
{
    public class DetalheDeVenda : Entidade
    {
        //public int Id { get; set; }

        public int OrdemDeVendaId { get; set; }
        public int ProdutoId { get; set; }
        //[Range(1, 1000, ErrorMessage = "Valor Inválido")]
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        [Range(0, 20, ErrorMessage = "Valor Inválido")]
        public decimal Desconto { get; set; }
        public bool RegistroTransacaoDeEstoque { get; set; }
        public int TransacaoDeEstoqueId { get; set; }

        /* Ef Relacionamento*/
        public OrdemDeVenda? OrdemDeVenda { get; set; }
        public Produto? Produto { get; set; }

    }
}
