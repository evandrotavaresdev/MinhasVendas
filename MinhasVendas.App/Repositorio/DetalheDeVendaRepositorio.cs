using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class DetalheDeVendaRepositorio : Repositorio<DetalheDeVenda>, IDetalheDeVendaRepositorio
    {
        public DetalheDeVendaRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
