using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Servicos
{
    public class FornecedorServico : BaseServico, IFornecedorServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        public FornecedorServico(INotificador notificador,
                                 IFornecedorRepositorio fornecedorRepositorio) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            await _fornecedorRepositorio.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            await _fornecedorRepositorio.Atualizar(fornecedor);
        }

        public async Task Remover(int id)
        {
            await _fornecedorRepositorio.Remover(id);
        }
    }
}
