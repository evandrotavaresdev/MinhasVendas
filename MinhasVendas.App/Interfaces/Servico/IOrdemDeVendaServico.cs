using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IOrdemDeVendaServico
    {
        Task Adicionar(OrdemDeVenda ordemDeVenda);
        Task Atualizar(OrdemDeVenda ordemDeVenda);
        Task Remover(int id);
        Task FinalizarVenda(OrdemDeVenda ordemDeVenda);
        Task FinalizarVendaView(int id);
        Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(int id);
        Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalheDeVenda(int id);
        Task<IEnumerable<OrdemDeVenda>> ConsultaOrdemDevendaCliente();
        Task<OrdemDeVenda> ConsultaOrdemDeVenda(int id);
    }
}
