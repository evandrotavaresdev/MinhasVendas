using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Servico;
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
        if (detalheDeCompra.RegistradoTransacaoDeEstoque)
        {
            Notificar("Produto já registrado no estoque. Não é possível excluir.");
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

        if (detalheDeCompra.RegistradoTransacaoDeEstoque)
        {
            Notificar("Produto já registrado no estoque.");
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

    public async Task<DetalheDeCompra> ConsultaDetalheDeCompraProdutoOrdemDeCompra(int id)
    {
        var consultaDetalheDeCompraProdutoOrdemDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                                                                 .AsNoTracking()
                                                                 .Include(p => p.Produto)
                                                                 .Include(o => o.OrdemDeCompra)
                                                                 .FirstOrDefaultAsync(d=> d.Id == id);

        return consultaDetalheDeCompraProdutoOrdemDeCompra;
    }

    public async Task<DetalheDeCompra> Consulta(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras.AsNoTracking().FirstOrDefaultAsync(d=> d.Id == id);

        return detalheDeCompra;
    }
}    

