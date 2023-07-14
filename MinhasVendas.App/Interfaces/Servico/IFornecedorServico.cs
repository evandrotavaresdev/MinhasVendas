using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IFornecedorServico
    {
        Task Adicionar(Fornecedor fornecedor);
        Task Atualizar(Fornecedor fornecedor);
        Task Remover(int id);

    }
}
