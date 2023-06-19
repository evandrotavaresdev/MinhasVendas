using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IDetalheDeCompraServico
    {
        Task Adicionar(DetalheDeCompra detalheDeCompra);
        Task Atualizar(DetalheDeCompra detalheDeCompra);
        Task Remover(int id);
        Task VerificarStatus (int id);
        Task InserirProdutoStatus (int id);
        Task RecberProduto(DetalheDeCompra detalheDeCompra);
        Task FinalizarVenda(int id);    

       
    }
}
