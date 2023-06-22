﻿using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Servicos;

public class DetalheDeCompraServico : BaseServico, IDetalheDeCompraServico
{
    private readonly MinhasVendasAppContext _minhasVendasAppContext;
    public DetalheDeCompraServico(MinhasVendasAppContext minhasVendasAppContext,
                                  INotificador notificador) : base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;
    }

    public async Task Adicionar(DetalheDeCompra detalheDeCompra)
    {
        var ordemDeCompra = await _minhasVendasAppContext.OrdemDeCompras.FindAsync(detalheDeCompra.OrdemDeCompraId);

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }
        _minhasVendasAppContext.Add(detalheDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

    }

    public Task Atualizar(DetalheDeCompra detalheDeCompra)
    {
        throw new NotImplementedException();
    }

    public async Task Remover(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                               .Include(v => v.OrdemDeCompra)
                               .FirstOrDefaultAsync(m => m.Id == id);


        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Venda está fechada. Não é possível Excluir o produto.");
            return;
        }
        _minhasVendasAppContext.DetalheDeCompras.Remove(detalheDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

    }

    public async Task VerificarStatus(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                              .Include(v => v.OrdemDeCompra)
                              .FirstOrDefaultAsync(m => m.Id == id);


        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Venda está fechada.");
            return;
        }
    }

    public async Task InserirProdutoStatus(int id)
    {
        var ordemDeCompra = await _minhasVendasAppContext.OrdemDeCompras.FindAsync(id);

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }



    }

    public async Task RecberProduto(DetalheDeCompra detalheDeCompra)
    {
        var ordemDeCompra = await _minhasVendasAppContext.OrdemDeCompras.FindAsync(detalheDeCompra.OrdemDeCompraId);

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }

        var itemDetalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras.FindAsync(detalheDeCompra.Id);

        itemDetalheDeCompra.DataDeRecebimento = detalheDeCompra.DataDeRecebimento;
        itemDetalheDeCompra.RegistradoTransacaoDeEstoque = true;
        _minhasVendasAppContext.Update(itemDetalheDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

        TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();
        transacaoDeEstoque.ProdutoId = detalheDeCompra.ProdutoId;
        transacaoDeEstoque.OrdemDeCompraId = detalheDeCompra.OrdemDeCompraId;
        transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Compra;
        transacaoDeEstoque.DataDeTransacao = DateTime.Now;
        transacaoDeEstoque.Quantidade = detalheDeCompra.Quantidade;
        _minhasVendasAppContext.Add(transacaoDeEstoque);
        await _minhasVendasAppContext.SaveChangesAsync();
    }

    public async Task FinalizarVendaStatus(int id)
    {

        var ordemDeCompra =
            await _minhasVendasAppContext.OrdemDeCompras
           .Include(v => v.DetalheDeCompras)
           .FirstOrDefaultAsync(v => v.Id == id);

        var temItensDeCompra = ordemDeCompra.DetalheDeCompras.Any();

        var temRegistroDeEstoqueAberto = ordemDeCompra.DetalheDeCompras.Any(d => d.RegistradoTransacaoDeEstoque == false);

        if (!temItensDeCompra)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra vazia");
            return;
        }

        if (temRegistroDeEstoqueAberto)
        {
            Notificar("FINALIZAR COMPRA. Existe produto sem recebimento.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra já está fechada.");
            return;
        }
        
    }
    public async Task FinalizarVenda(OrdemDeCompra ordemDeCompra)
    {

        var itemOrdemDeCompra =
            await _minhasVendasAppContext.OrdemDeCompras
           .Include(v => v.DetalheDeCompras)
           .FirstOrDefaultAsync(v => v.Id == ordemDeCompra.Id);

        var temItensDeCompra = itemOrdemDeCompra.DetalheDeCompras.Any();

        var temRegistroDeEstoqueAberto = itemOrdemDeCompra.DetalheDeCompras.Any(d => d.RegistradoTransacaoDeEstoque == false);

        if (!temItensDeCompra)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra vazia");
            return;
        }

        if (temRegistroDeEstoqueAberto)
        {
            Notificar("FINALIZAR COMPRA. Existe produto sem recebimento.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra já está fechada.");
            return;
        }

        itemOrdemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Fechado;
        itemOrdemDeCompra.FormaDePagamento = ordemDeCompra.FormaDePagamento;


        _minhasVendasAppContext.Update(ordemDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

    }

}    

