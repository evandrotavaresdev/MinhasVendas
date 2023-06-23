using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Servicos;

public class OrdemDeCompraServico : BaseServico, IOrdemDeCompraServico
{
    private readonly MinhasVendasAppContext _minhasVendasAppContext;
  

    public OrdemDeCompraServico(MinhasVendasAppContext minhasVendasAppContext,
                                INotificador notificador): base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;   
    }

    public async Task Adicionar(OrdemDeCompra ordemDeCompra)
    {
        ordemDeCompra.DataDeCriacao = DateTime.Now;
        ordemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Solicitado;
       
       
        _minhasVendasAppContext.Add(ordemDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();           

    }

    public Task Atualizar(OrdemDeCompra ordemDeCompra)
    {
        throw new NotImplementedException();
    }

    public Task Remover(int id)
    {
        throw new NotImplementedException();
    }

    
    public async Task FinalizarCompra(OrdemDeCompra ordemDeCompra)
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


        _minhasVendasAppContext.Update(itemOrdemDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

    }

    public async Task FinalizarCompraView(int id)
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
}
