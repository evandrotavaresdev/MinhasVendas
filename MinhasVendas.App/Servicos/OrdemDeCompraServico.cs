using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Servicos;

public class OrdemDeCompraServico : BaseServico, IOrdemDeCompraServico
{
    private readonly IOrdemDeCompraRepositorio _ordemDeCompraRepositorio;

    public OrdemDeCompraServico(
                                INotificador notificador,
                                IOrdemDeCompraRepositorio ordemDeCompraRepositorio) : base(notificador)
    {
        _ordemDeCompraRepositorio = ordemDeCompraRepositorio;
    }

    public async Task Adicionar(OrdemDeCompra ordemDeCompra)
    {
        ordemDeCompra.DataDeCriacao = DateTime.Now;
        ordemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Solicitado;

        await _ordemDeCompraRepositorio.Adicionar(ordemDeCompra);
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
        var itemOrdemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d=> d.DetalheDeCompras).FirstOrDefaultAsync(o=> o.Id == ordemDeCompra.Id);

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

        await _ordemDeCompraRepositorio.Atualizar(itemOrdemDeCompra);

    }

    public async Task FinalizarCompraView(int id)
    {
          var ordemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d => d.DetalheDeCompras).FirstOrDefaultAsync(o => o.Id == id);


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

    public async Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(int id)
    {
        var ordemDeCompra = await _ordemDeCompraRepositorio.Obter()
                                    .Include(d => d.DetalheDeCompras)
                                    .ThenInclude(p=> p.Produto)
                                    .Include(f=> f.Fornecedor)
                                    .FirstOrDefaultAsync(o => o.Id == id);

        return ordemDeCompra;
    }

    public async Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompra(int id)
    {

        var ordemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d => d.DetalheDeCompras).FirstOrDefaultAsync(o => o.Id == id);
      
        return ordemDeCompra;
    }

    public async Task<OrdemDeCompra> ConsultaOrdemDeCompra(int id)
    {
        var ordemDeCompra = await _ordemDeCompraRepositorio.BuscarPorId(id);
       
        return ordemDeCompra;
    }

    public async Task<IEnumerable<OrdemDeCompra>> ConsultaOrdemDeCompraFornecedor()
    {
        var ordeDecompraFornecedor = await _ordemDeCompraRepositorio.ObterSemRastreamento().Include(f=> f.Fornecedor).ToListAsync();
     
        return ordeDecompraFornecedor;
    }
}
