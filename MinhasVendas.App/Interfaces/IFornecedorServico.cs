using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface IFornecedorServico
    {
        Task<IEnumerable<Fornecedor>> ConsultaFornecedor();
        
    }
}
