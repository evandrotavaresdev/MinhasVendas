using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Servicos
{
    public class ProdutoServico : BaseServico, IProdutoServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        
        public ProdutoServico(MinhasVendasAppContext minhasVendasAppContext,
                              INotificador notificador): base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
        }

        public async Task Adicionar(Produto produto)
        {
           
            if (_minhasVendasAppContext.Produtos.ToListAsync().Result.Any(p => p.Nome == produto.Nome))
            {
                Notificar("Já existe um produto com este nome informado.");
                return;
            }

            _minhasVendasAppContext.Add(produto);
            await _minhasVendasAppContext.SaveChangesAsync();
        }

        public Task Atualizar(Produto produto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Produto>> ConsultaProdutos()
        {
            var produtos = await _minhasVendasAppContext.Produtos.AsNoTracking().ToListAsync();

            return produtos;
        }

        public Task Remover(int id)
        {
            throw new NotImplementedException();
        }

        




    }
}
