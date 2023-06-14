using System.Threading.Tasks;
using MinhasVendas.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MinhasVendas.App.Extensoes
{
    public class ResumoViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public ResumoViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            notificacoes.ForEach(c=> ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));

            return View();
        }
    }
}