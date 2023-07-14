using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IDetalheDeVendaServico
    {
        Task Adicionar(DetalheDeVenda detalheDeVenda);
        Task AdicionarView(int id);
        Task Atualizar(DetalheDeVenda detalheDeVenda);
        Task Remover(int id, bool? ehView);
        Task<DetalheDeVenda> ConsultaDetalheDeVendaOrdemDeVenda(int id);
        Task<DetalheDeVenda> ConsultaDetalheDeVendaProdutoOrdemDeVenda(int id);
        
    }
}
