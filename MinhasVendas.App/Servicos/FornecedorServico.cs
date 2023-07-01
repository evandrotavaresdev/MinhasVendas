using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Servicos
{
    public class FornecedorServico : BaseServico, IFornecedorServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        public FornecedorServico(MinhasVendasAppContext minhasVendasAppContext,
                                 INotificador notificador) : base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
        }

        public async Task<IEnumerable<Fornecedor>> ConsultaFornecedor()
        {
            var fornecedor =  await _minhasVendasAppContext.Fornecedores.ToListAsync();

            return fornecedor;
        }
    }
}
