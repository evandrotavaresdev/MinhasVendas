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

        Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(int id);
        Task<IEnumerable<OrdemDeCompra>> ConsultaOrdemDeCompraFornecedor();
        Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompra(int id);
        Task<OrdemDeCompra> ConsultaOrdemDeCompra(int id);

    }
}
