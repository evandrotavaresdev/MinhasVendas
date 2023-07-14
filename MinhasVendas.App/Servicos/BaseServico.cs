using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Notificador;

namespace MinhasVendas.App.Servicos
{
    public abstract class BaseServico
    {
        private readonly INotificador _notificador;

        public BaseServico(INotificador notificador)
        {
            _notificador = notificador; 
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
