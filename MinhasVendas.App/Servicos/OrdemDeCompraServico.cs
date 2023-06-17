using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Servicos
{
    public class OrdemDeCompraServico : BaseServico, IOrdemDeCompraServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
      

        public OrdemDeCompraServico(MinhasVendasAppContext minhasVendasAppContext,
                                    INotificador notificador): base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;   
        }

        public async Task Adicionar(OrdemDeCompra ordemDeCompra)
        {
            ordemDeCompra.DataDeCriacao = DateTime.Now;
            ordemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Solicitado;
            ordemDeCompra.ValorDeFrete = 0;
           
            _minhasVendasAppContext.Add(ordemDeCompra);
            await _minhasVendasAppContext.SaveChangesAsync();           

        }

        public Task Atualizar(OrdemDeCompra ordemDeCompra)
        {
            throw new NotImplementedException();
        }

        public Task Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
