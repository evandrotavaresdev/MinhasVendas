using MinhasVendas.App.Models;
using MinhasVendas.App.Servicos;

namespace MinhasVendas.App.Interfaces
{
    public interface IClienteServico 
    {
        Task<IEnumerable<Cliente>> ConsutaClientes();
    }
}
