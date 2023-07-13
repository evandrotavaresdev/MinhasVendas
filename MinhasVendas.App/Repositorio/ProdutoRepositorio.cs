using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class ProdutoRepositorio : Repositorio<Produto>, IProdutoRepositorio
    {
        public ProdutoRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
