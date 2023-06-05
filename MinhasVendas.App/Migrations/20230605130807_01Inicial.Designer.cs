﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhasVendas.App.Data;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    [DbContext(typeof(MinhasVendasAppContext))]
    [Migration("20230605130807_01Inicial")]
    partial class _01Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MinhasVendas.App.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeCompra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("CustoUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OrdemDeCompraId")
                        .HasColumnType("int");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("TransacaoDeEstoqueId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrdemDeCompraId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("DetalheDeCompras");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Desconto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OrdemDeVendaId")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrdemDeVendaId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("DetalheDeVendas");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Fornecedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Fornecedores");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeCompra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FornecedorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FornecedorId");

                    b.ToTable("OrdemDeCompras");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataDePagamento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataDeVenda")
                        .HasColumnType("datetime2");

                    b.Property<int>("FormaDePagamento")
                        .HasColumnType("int");

                    b.Property<int>("StatusOrdemDeVenda")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("OrdemDeVendas");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecoBase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrecoDeLista")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.TransacaoDeEstoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("OrdemDeCompraId")
                        .HasColumnType("int");

                    b.Property<int?>("OrdemDeVendaId")
                        .HasColumnType("int");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("TipoDransacaoDeEstoque")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrdemDeCompraId");

                    b.HasIndex("OrdemDeVendaId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("TransacaoDeEstoques");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeCompra", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.OrdemDeCompra", "OrdemDeCompra")
                        .WithMany()
                        .HasForeignKey("OrdemDeCompraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdemDeCompra");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeVenda", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.OrdemDeVenda", "OrdemDeVenda")
                        .WithMany()
                        .HasForeignKey("OrdemDeVendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdemDeVenda");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeCompra", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Fornecedor", "Fornecedor")
                        .WithMany()
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeVenda", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.TransacaoDeEstoque", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.OrdemDeCompra", "OrdemDeCompras")
                        .WithMany()
                        .HasForeignKey("OrdemDeCompraId");

                    b.HasOne("MinhasVendas.App.Models.OrdemDeVenda", "OrdemDeVendas")
                        .WithMany()
                        .HasForeignKey("OrdemDeVendaId");

                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdemDeCompras");

                    b.Navigation("OrdemDeVendas");

                    b.Navigation("Produto");
                });
#pragma warning restore 612, 618
        }
    }
}
