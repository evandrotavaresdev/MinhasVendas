using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IProdutoServico
    {
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(int id);
        Task<IEnumerable<Produto>> ConsultaProdutos();
        
    }
}
