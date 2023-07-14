using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly IClienteRespositorio _clienteRespositorio;
        private readonly IClienteServico _clienteServico;

        public ClientesController(
                                  IClienteRespositorio clienteRespositorio,
                                  IClienteServico clienteServico,
                                  INotificador notificador) : base(notificador)  
        {
            _clienteRespositorio = clienteRespositorio;
            _clienteServico = clienteServico;
        }
     
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRespositorio.Obter().ToListAsync();

            return View(clientes);

        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();
          
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id","Nome")] Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            await _clienteServico.Adicionar(cliente);

            if (!OperacaoValida()) return View(cliente);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();
          
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();

            var clienteDB = await _clienteRespositorio.ObterSemRastreamento().FirstOrDefaultAsync(c=> c.Id == id);

            if (clienteDB == null) return NotFound();

            await _clienteServico.Atualizar(cliente);

            if (!OperacaoValida()) return View(cliente);

            return RedirectToAction(nameof(Index));
        
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteRespositorio.ObterPorId(c => c.Id == id);
            
            if (cliente == null) return NotFound();
           
            return View(cliente);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();

            _clienteRespositorio.Desanexar(cliente);

            await _clienteServico.Remover(id);

            if (!OperacaoValida()) return View(cliente);

            return RedirectToAction(nameof(Index));
        }
    }
}
