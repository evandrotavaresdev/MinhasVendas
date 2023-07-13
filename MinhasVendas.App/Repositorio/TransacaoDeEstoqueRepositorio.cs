using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class TransacaoDeEstoqueRepositorio : Repositorio<TransacaoDeEstoque>, ITransacaoDeEstoqueRepositorio
    {
        public TransacaoDeEstoqueRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
