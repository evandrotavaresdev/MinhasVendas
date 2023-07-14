using Microsoft.AspNetCore.Components.Web;
using MinhasVendas.App.Notificador;

namespace MinhasVendas.App.Interfaces.Servico
{
    public interface INotificador
    {
        void Handle(Notificacao notificacao);
        List<Notificacao> ObterNotificacoes();
        bool TemNotificacao();
    }
}
