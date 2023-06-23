﻿using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Servicos;

public class DetalheDeVendaServico : BaseServico, IDetalheDeVendaServico
{
    
    private readonly MinhasVendasAppContext _minhasVendasAppContext;
    public DetalheDeVendaServico(MinhasVendasAppContext minhasVendasAppContext,
                                 INotificador notificador) : base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;
        
    }
    public Task Adicionar(DetalheDeVenda detalheDeVenda)
    {
        throw new NotImplementedException();
    }

    public async Task AdicionarView(int id)
    {
        var ordemDevenda = _minhasVendasAppContext.OrdemDeVendas.FirstOrDefault(o => o.Id == id);

        if (ordemDevenda == null)
        {
            Notificar("ADICIONAR ITEM DE VENDA - Não exsitem ordem de venda com o id informada");
            return;
        }

        if (ordemDevenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("ADICIONAR ITEM DE VENDA - Ordem de venda com staus vendido");
        }


    }

    public Task Atualizar(DetalheDeVenda detalheDeVenda)
    {
        throw new NotImplementedException();
    }

    public async Task Remover(int id, bool? ehView)
    {
        var detalheDeVenda = await _minhasVendasAppContext.DetalheDeVendas.Include(o => o.OrdemDeVenda).FirstOrDefaultAsync(o => o.Id == id);

        if (detalheDeVenda.OrdemDeVenda.Id == null)
        {
            Notificar("REMOVER ITEM DE VENDA - Não exsitem ordem de venda com o id informada");
            return;
        }

        if (detalheDeVenda.OrdemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("REMOVER ITEM DE VENDA - Ordem de venda com staus vendido");
        }

        if (ehView == true) return;

         _minhasVendasAppContext.Remove(detalheDeVenda);
        await _minhasVendasAppContext.SaveChangesAsync();

    }
}
