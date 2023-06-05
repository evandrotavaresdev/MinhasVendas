using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Data
{
    public class MinhasVendasAppContext : DbContext
    {
        public MinhasVendasAppContext (DbContextOptions<MinhasVendasAppContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente>? Clientes { get; set; }
        public DbSet<DetalheDeCompra>? DetalheDeCompras { get; set; }
        public DbSet<DetalheDeVenda>? DetalheDeVendas { get; set; }
        public DbSet<Fornecedor>? Fornecedores { get; set; }
        public DbSet<OrdemDeCompra>? OrdemDeCompras { get; set; }
        public DbSet<OrdemDeVenda>? OrdemDeVendas { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }

    }
}
