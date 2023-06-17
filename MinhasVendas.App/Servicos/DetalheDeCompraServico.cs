using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Servicos
{
    public class DetalheDeCompraServico : BaseServico, IDetalheDeCompraServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        public DetalheDeCompraServico(MinhasVendasAppContext minhasVendasAppContext,
                                      INotificador notificador): base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
        }

        public Task Adicionar(DetalheDeCompra detalheDeCompra)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(DetalheDeCompra detalheDeCompra)
        {
            throw new NotImplementedException();
        }

        public async Task Remover(int id)
        {
            var detalheDeCompra =  await _minhasVendasAppContext.DetalheDeCompras               
                                   .Include(v => v.OrdemDeCompra)
                                   .FirstOrDefaultAsync(m => m.Id == id);
          

            if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
            {
                Notificar("Ordem de Venda está fechada. Não é possível Excluir o produto.");
                return;
            }
            _minhasVendasAppContext.DetalheDeCompras.Remove(detalheDeCompra);
            await _minhasVendasAppContext.SaveChangesAsync();

        }

        public async Task VerificarStatus(int id)
        {
            var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                                  .Include(v => v.OrdemDeCompra)
                                  .FirstOrDefaultAsync(m => m.Id == id);


            if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
            {
                Notificar("Ordem de Venda está fechada. Não é possível Excluir o produto.");
                return;
            }
        }
    }
}
