using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class DetalheDeCompraRepositorio : Repositorio<DetalheDeCompra>, IDetalheDeCompraRepositorio
    {
        public DetalheDeCompraRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
