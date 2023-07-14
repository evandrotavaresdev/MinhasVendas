using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Servicos;

public class OrdemDeVendaServico : BaseServico, IOrdemDeVendaServico
{
    private readonly MinhasVendasAppContext _minhasVendasAppContext;
    public OrdemDeVendaServico(MinhasVendasAppContext minhasVendasAppContext,
                               INotificador notificador) : base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;
    }
    public async Task FinalizarVendaView(int id)
    {
        var ordemDeVenda = await
            _minhasVendasAppContext.OrdemDeVendas
                .Include(v => v.DetalheDeVendas)
                    .FirstOrDefaultAsync(v => v.Id == id);

        var temItensDeVenda = ordemDeVenda.DetalheDeVendas.Any();

        if (!temItensDeVenda)
        {
            Notificar("FINALIZAR VENDA. Ordem de Venda vazia.");
            return;
        }

        if (ordemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("FINALIZAR VENDA. Ordem de venda já está com status vendido.");
            return;
        }
    }

    public async Task FinalizarVenda(OrdemDeVenda ordemDeVendaEntrada)
    {
        var ordemDeVenda = await _minhasVendasAppContext.OrdemDeVendas.Include(d => d.DetalheDeVendas).FirstOrDefaultAsync(o => o.Id == ordemDeVendaEntrada.Id);

        var temItensDeVenda = ordemDeVenda.DetalheDeVendas.Any();

        if (!temItensDeVenda)
        {
            Notificar("FINALIZAR VENDA. Ordem de Venda vazia.");
            return;
        }

        if (ordemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("FINALIZAR VENDA. Ordem de venda já está com status vendido.");
            return;
        }

        foreach (var item in ordemDeVenda.DetalheDeVendas)
        {
            TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();

            transacaoDeEstoque.ProdutoId = item.ProdutoId;
            transacaoDeEstoque.OrdemDeVendaId = item.OrdemDeVendaId;
            transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Venda;
            transacaoDeEstoque.DataDeTransacao = DateTime.Now;
            transacaoDeEstoque.Quantidade = item.Quantidade;

            _minhasVendasAppContext.TransacaoDeEstoques.Add(transacaoDeEstoque);
            await _minhasVendasAppContext.SaveChangesAsync();

            item.TransacaoDeEstoqueId = transacaoDeEstoque.Id;
            item.RegistroTransacaoDeEstoque = true;

            _minhasVendasAppContext.DetalheDeVendas.Update(item);
            await _minhasVendasAppContext.SaveChangesAsync();

        }
        ordemDeVenda.StatusOrdemDeVenda = StatusOrdemDeVenda.Vendido;
        ordemDeVenda.FormaDePagamento = ordemDeVendaEntrada.FormaDePagamento;

        _minhasVendasAppContext.Update(ordemDeVenda);
        await _minhasVendasAppContext.SaveChangesAsync();
    }
    public async Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(int id)
    {
        var ordemDeVenda = await _minhasVendasAppContext.OrdemDeVendas
               .Include(v => v.Cliente)
               .Include(v => v.DetalheDeVendas).ThenInclude(v => v.Produto)
               .FirstOrDefaultAsync(m => m.Id == id);

        return ordemDeVenda;
    }

    public async Task Adicionar(OrdemDeVenda ordemDeVenda)
    {
        _minhasVendasAppContext.OrdemDeVendas.Add(ordemDeVenda);
        
        ordemDeVenda.StatusOrdemDeVenda = StatusOrdemDeVenda.Orcamento;
        ordemDeVenda.DataDeVenda = DateTime.Now;
        
        await _minhasVendasAppContext.SaveChangesAsync();
    }

    public Task Atualizar(OrdemDeVenda ordemDeVenda)
    {
        throw new NotImplementedException();
    }

    public Task Remover(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalheDeVenda(int id)
    {
        var ordemDeVenda = await _minhasVendasAppContext
                            .OrdemDeVendas
                                .Include(v => v.DetalheDeVendas)
                            .FirstOrDefaultAsync(v => v.Id == id);
      
        return ordemDeVenda;
    }

    public async  Task<OrdemDeVenda> ConsultaOrdemDeVenda(int id)
    {
        var ordemDeVenda = await _minhasVendasAppContext.OrdemDeVendas.FindAsync(id);

        return ordemDeVenda;
    }

    public async Task<IEnumerable<OrdemDeVenda>> ConsultaOrdemDevendaCliente()
    {
        var ordemDeVendaCliente = await _minhasVendasAppContext.OrdemDeVendas.AsNoTracking().Include(c => c.Cliente).ToListAsync();

        return ordemDeVendaCliente;
    }
}