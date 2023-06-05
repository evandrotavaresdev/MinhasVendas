using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Controllers
{
    public class DetalheDeVendasController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public DetalheDeVendasController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: DetalheDeVendas
        public async Task<IActionResult> Index()
        {
            var appEstoquesEVendasContext = _context.DetalheDeVendas.Include(d => d.OrdemDeVenda).Include(d => d.Produto);
            return View(await appEstoquesEVendasContext.ToListAsync());
        }

        // GET: DetalheDeVendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetalheDeVendas == null)
            {
                return NotFound();
            }

            var detalheDeVenda = await _context.DetalheDeVendas
                .Include(d => d.OrdemDeVenda)
                .Include(d => d.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalheDeVenda == null)
            {
                return NotFound();
            }

            return View(detalheDeVenda);
        }

        // GET: DetalheDeVendas/Create
        public IActionResult Create()
        {
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id");
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id");
            return View();
        }

        // POST: DetalheDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdemDeVendaId,ProdutoId,Quantidade,PrecoUnitario,Desconto")] DetalheDeVenda detalheDeVenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalheDeVenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", detalheDeVenda.OrdemDeVendaId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeVenda.ProdutoId);
            return View(detalheDeVenda);
        }

        // GET: DetalheDeVendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetalheDeVendas == null)
            {
                return NotFound();
            }

            var detalheDeVenda = await _context.DetalheDeVendas.FindAsync(id);
            if (detalheDeVenda == null)
            {
                return NotFound();
            }
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", detalheDeVenda.OrdemDeVendaId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeVenda.ProdutoId);
            return View(detalheDeVenda);
        }

        // POST: DetalheDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrdemDeVendaId,ProdutoId,Quantidade,PrecoUnitario,Desconto")] DetalheDeVenda detalheDeVenda)
        {
            if (id != detalheDeVenda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalheDeVenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalheDeVendaExists(detalheDeVenda.Id))
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
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", detalheDeVenda.OrdemDeVendaId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeVenda.ProdutoId);
            return View(detalheDeVenda);
        }

        // GET: DetalheDeVendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetalheDeVendas == null)
            {
                return NotFound();
            }

            var detalheDeVenda = await _context.DetalheDeVendas
                .Include(d => d.OrdemDeVenda)
                .Include(d => d.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalheDeVenda == null)
            {
                return NotFound();
            }

            return View(detalheDeVenda);
        }

        // POST: DetalheDeVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetalheDeVendas == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.DetalheDeVendas'  is null.");
            }
            var detalheDeVenda = await _context.DetalheDeVendas.FindAsync(id);
            if (detalheDeVenda != null)
            {
                _context.DetalheDeVendas.Remove(detalheDeVenda);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalheDeVendaExists(int id)
        {
          return (_context.DetalheDeVendas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
