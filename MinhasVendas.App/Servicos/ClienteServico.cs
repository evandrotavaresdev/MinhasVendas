using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Servicos
{
    public class ClienteServico : BaseServico, IClienteServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        public ClienteServico(MinhasVendasAppContext minhasVendasAppContext,
                              INotificador notificador) : base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;   
        }

        public async Task<IEnumerable<Cliente>> ConsutaClientes()
        {
            var clientes = await _minhasVendasAppContext.Clientes.ToListAsync();

            return clientes;
        }
    }
}
