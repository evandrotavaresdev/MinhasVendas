using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IOrdemDeCompraServico
    {
        Task Adicionar(OrdemDeCompra ordemDeCompra);
        Task Atualizar(OrdemDeCompra ordemDeCompra);
        Task Remover(int id);
        Task FinalizarCompra(OrdemDeCompra ordemDeCompra);
        Task FinalizarCompraView(int id);
    }
}
