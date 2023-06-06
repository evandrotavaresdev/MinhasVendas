﻿namespace MinhasVendas.App.Models
{
    public class DetalheDeCompra
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }
        public int OrdemDeCompraId { get; set; }
        public int TransacaoDeEstoqueId { get; set; } 
        
        public int Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }

        /* EF Relacionamento */
        public Produto? Produto { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }

    }
}