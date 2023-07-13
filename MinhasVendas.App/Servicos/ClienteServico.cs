using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Servicos
{
    public class ClienteServico : BaseServico, IClienteServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        private readonly IClienteRespositorio _clienteRespositorio;

        public ClienteServico(MinhasVendasAppContext minhasVendasAppContext,
                              INotificador notificador,
                              IClienteRespositorio clienteRespositorio) : base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext; 
            _clienteRespositorio = clienteRespositorio;
        }

        public async Task<IEnumerable<Cliente>> ConsutaClientes()
        {
            var clientes = await _minhasVendasAppContext.Clientes.ToListAsync();
            
            return clientes;
        }
    }
}
