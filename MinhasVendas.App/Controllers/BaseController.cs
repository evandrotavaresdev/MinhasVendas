using Microsoft.AspNetCore.Mvc;
using MinhasVendas.App.Interfaces.Servico;

namespace MinhasVendas.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificador _notificador;

        public BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificador.Notificacao(mensagem));
        }
    }
}
