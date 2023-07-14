using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Servicos
{
    public class ClienteServico : BaseServico, IClienteServico
    {
        private readonly IClienteRespositorio _clienteRespositorio;
        public ClienteServico(INotificador notificador, 
                              IClienteRespositorio clienteRespositorio) : base(notificador)
        {
            _clienteRespositorio = clienteRespositorio;
        }

        public async Task Adicionar(Cliente cliente)
        {
            await _clienteRespositorio.Adicionar(cliente);
        }

        public async Task Atualizar(Cliente cliente)
        {
             await _clienteRespositorio.Atualizar(cliente);
        }

        public async Task Remover(int id)
        {
            await _clienteRespositorio.Remover(id);
        }
    }
}
