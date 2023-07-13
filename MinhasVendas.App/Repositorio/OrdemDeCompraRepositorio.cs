using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class OrdemDeCompraRepositorio : Repositorio<OrdemDeCompra>, IOrdemDeCompraRepositorio
    {
        public OrdemDeCompraRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
