using MinhasVendas.App.Models;
using MinhasVendas.App.Servicos;

namespace MinhasVendas.App.Interfaces.Servico
{
    public interface IClienteServico
    {
        Task Adicionar(Cliente cliente);
        Task Atualizar(Cliente cliente);
        Task Remover(int id);
    }
}
