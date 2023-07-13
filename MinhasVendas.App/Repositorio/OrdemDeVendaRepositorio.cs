using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class OrdemDeVendaRepositorio : Repositorio<OrdemDeVenda>, IOrdemDeVendaRepositorio
    {
        public OrdemDeVendaRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
