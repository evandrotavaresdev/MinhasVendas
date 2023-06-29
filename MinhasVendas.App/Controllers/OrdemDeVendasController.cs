using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers
{
    public class OrdemDeVendasController : BaseController
    {
        private readonly MinhasVendasAppContext _context;
        private readonly IOrdemDeVendaServico _ordemDeVendaServico;

        public OrdemDeVendasController(MinhasVendasAppContext context,
                                       IOrdemDeVendaServico ordemDeVendaServico,
                                       INotificador notificador) : base(notificador)
        {
            _context = context;
            _ordemDeVendaServico = ordemDeVendaServico;
        }

        public async Task<IActionResult> CarrinhoDeVendas(int? id)
        {
            if (id == null || _context.OrdemDeVendas == null)
            {
                return NotFound();
            }

            var ordemDeVenda = await _context.OrdemDeVendas
                 .Include(v => v.Cliente)
                 .Include(v => v.DetalheDeVendas).ThenInclude(v => v.Produto)
                 .FirstOrDefaultAsync(m => m.Id == id);


            if (ordemDeVenda == null)
            {
                return NotFound();
            }


            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "NomeProduto");

            var model = new CarrinhoDeVendasViewModel();
            model.OrdemDeVenda = ordemDeVenda;

            if (ordemDeVenda.DetalheDeVendas.Any())
            {
                var precoProduto = from item in model.OrdemDeVenda.DetalheDeVendas select (item.PrecoUnitario * item.Quantidade * (1 - item.Desconto / 100));
                decimal[] precoProdutos = precoProduto.ToArray();
                decimal totalVenda = precoProdutos.Aggregate((a, b) => a + b);
                model.TotalVenda = totalVenda;

                var itens = from item in model.OrdemDeVenda.DetalheDeVendas select (item.Quantidade);
                int totalItens = itens.Sum();
                model.TotalItens = totalItens;
            }

            
            return View(model);
        }
        public async Task<IActionResult> CarrinhoDeVendasPartial(int? id)
        {
            if (id == null || _context.OrdemDeVendas == null)
            {
                return NotFound();
            }

            var ordemDeVenda = await _context.OrdemDeVendas
                 .Include(v => v.Cliente)
                 .Include(v => v.DetalheDeVendas).ThenInclude(v => v.Produto)
                 .FirstOrDefaultAsync(m => m.Id == id);


            if (ordemDeVenda == null)
            {
                return NotFound();
            }


            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "NomeProduto");

            var model = new CarrinhoDeVendasViewModel();
            model.OrdemDeVenda = ordemDeVenda;

            if (ordemDeVenda.DetalheDeVendas.Any())
            {
                var precoProduto = from item in model.OrdemDeVenda.DetalheDeVendas select (item.PrecoUnitario * item.Quantidade * (1 - item.Desconto / 100));
                decimal[] precoProdutos = precoProduto.ToArray();
                decimal totalVenda = precoProdutos.Aggregate((a, b) => a + b);
                model.TotalVenda = totalVenda;

                var itens = from item in model.OrdemDeVenda.DetalheDeVendas select (item.Quantidade);
                int totalItens = itens.Sum();
                model.TotalItens = totalItens;
            }

            return PartialView("CarrinhoDeVendas", model);
           
        }

        /* */

        // GET: OrdemDeVendas
        public async Task<IActionResult> Index()
        {
            var appEstoquesEVendasContext = _context.OrdemDeVendas.Include(o => o.Cliente);
            return View(await appEstoquesEVendasContext.ToListAsync());
        }

        // GET: OrdemDeVendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrdemDeVendas == null)
            {
                return NotFound();
            }

            var ordemDeVenda = await _context.OrdemDeVendas
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordemDeVenda == null)
            {
                return NotFound();
            }

            return View(ordemDeVenda);
        }

        // GET: OrdemDeVendas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        // POST: OrdemDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,StatusOrdemDeVenda,FormaDePagamento,DataDePagamento,DataDeVenda")] OrdemDeVenda ordemDeVenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordemDeVenda);
                ordemDeVenda.StatusOrdemDeVenda = StatusOrdemDeVenda.Orcamento;
                ordemDeVenda.DataDeVenda = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction("CarrinhoDeVendas", new { id = ordemDeVenda.Id });
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
            
        }

        // GET: OrdemDeVendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrdemDeVendas == null)
            {
                return NotFound();
            }

            var ordemDeVenda = await _context.OrdemDeVendas.FindAsync(id);
            if (ordemDeVenda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
        }

        // POST: OrdemDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,StatusOrdemDeVenda,FormaDePagamento,DataDePagamento,DataDeVenda")] OrdemDeVenda ordemDeVenda)
        {
            if (id != ordemDeVenda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordemDeVenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdemDeVendaExists(ordemDeVenda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
        }

        // GET: OrdemDeVendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrdemDeVendas == null)
            {
                return NotFound();
            }

            var ordemDeVenda = await _context.OrdemDeVendas
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordemDeVenda == null)
            {
                return NotFound();
            }

            return View(ordemDeVenda);
        }

        // POST: OrdemDeVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrdemDeVendas == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.OrdemDeVendas'  is null.");
            }
            var ordemDeVenda = await _context.OrdemDeVendas.FindAsync(id);
            if (ordemDeVenda != null)
            {
                _context.OrdemDeVendas.Remove(ordemDeVenda);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdemDeVendaExists(int id)
        {
          return (_context.OrdemDeVendas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
