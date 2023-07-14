using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Controllers
{
    
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IFornecedorServico _fornecedorServico;

        public FornecedoresController(IFornecedorRepositorio fornecedorRepositorio,
                                      IFornecedorServico fornecedorServico,
                                      INotificador notificador) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _fornecedorServico = fornecedorServico;
        }

        public async Task<IActionResult> Index()
        {
            var fornecedores = await _fornecedorRepositorio.BuscarTodos();
            return View(fornecedores);

        }

        public async Task<IActionResult> Details(int id)
        {
            var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

            if (fornecedor == null) return NotFound();
          
            return View(fornecedor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Fornecedor fornecedor)
        {
            if (!ModelState.IsValid) return View(fornecedor);

            await _fornecedorServico.Adicionar(fornecedor);

            if (!OperacaoValida()) return View(fornecedor);

            return RedirectToAction(nameof(Index));
      
        }

        public async Task<IActionResult> Edit(int id)
        {
            var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

            if (fornecedor == null) return NotFound();
          
            return View(fornecedor);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Fornecedor fornecedor)
        {
            if (id != fornecedor.Id) return NotFound();

            if (!ModelState.IsValid) return View(fornecedor);

            await _fornecedorServico.Atualizar(fornecedor);

            if (!OperacaoValida()) return View(fornecedor);

            return RedirectToAction(nameof(Index));

        }
     
        public async Task<IActionResult> Delete(int id)
        {
            var fornecedor = await _fornecedorRepositorio.ObterPorId(m => m.Id == id);
            
            if (fornecedor == null) return NotFound();
         
            return View(fornecedor);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);
            
            if (fornecedor == null) return View(fornecedor);

            _fornecedorRepositorio.Desanexar(fornecedor);

            await _fornecedorServico.Remover(id);

            if (!OperacaoValida()) return View(fornecedor);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
