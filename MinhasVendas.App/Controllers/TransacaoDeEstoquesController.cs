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
    public class TransacaoDeEstoquesController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public TransacaoDeEstoquesController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: TransacaoDeEstoques
        public async Task<IActionResult> Index()
        {
            var appEstoquesEVendasContext = _context.TransacaoDeEstoques.Include(t => t.OrdemDeCompra).Include(t => t.OrdemDeVenda).Include(t => t.Produto);
            return View(await appEstoquesEVendasContext.ToListAsync());
        }

        // GET: TransacaoDeEstoques/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TransacaoDeEstoques == null)
            {
                return NotFound();
            }

            var transacaoDeEstoque = await _context.TransacaoDeEstoques
                .Include(t => t.OrdemDeCompra)
                .Include(t => t.OrdemDeVenda)
                .Include(t => t.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transacaoDeEstoque == null)
            {
                return NotFound();
            }

            return View(transacaoDeEstoque);
        }

        // GET: TransacaoDeEstoques/Create
        public IActionResult Create()
        {
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id");
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id");
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id");
            return View();
        }

        // POST: TransacaoDeEstoques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,OrdemDeCompraId,OrdemDeVendaId,Quantidade,TipoDransacaoDeEstoque")] TransacaoDeEstoque transacaoDeEstoque)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transacaoDeEstoque);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id", transacaoDeEstoque.OrdemDeCompraId);
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", transacaoDeEstoque.OrdemDeVendaId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", transacaoDeEstoque.ProdutoId);
            return View(transacaoDeEstoque);
        }

        // GET: TransacaoDeEstoques/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TransacaoDeEstoques == null)
            {
                return NotFound();
            }

            var transacaoDeEstoque = await _context.TransacaoDeEstoques.FindAsync(id);
            if (transacaoDeEstoque == null)
            {
                return NotFound();
            }
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id", transacaoDeEstoque.OrdemDeCompraId);
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", transacaoDeEstoque.OrdemDeVendaId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", transacaoDeEstoque.ProdutoId);
            return View(transacaoDeEstoque);
        }

        // POST: TransacaoDeEstoques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,OrdemDeCompraId,OrdemDeVendaId,Quantidade,TipoDransacaoDeEstoque")] TransacaoDeEstoque transacaoDeEstoque)
        {
            if (id != transacaoDeEstoque.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transacaoDeEstoque);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransacaoDeEstoqueExists(transacaoDeEstoque.Id))
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
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id", transacaoDeEstoque.OrdemDeCompraId);
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", transacaoDeEstoque.OrdemDeVendaId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", transacaoDeEstoque.ProdutoId);
            return View(transacaoDeEstoque);
        }

        // GET: TransacaoDeEstoques/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TransacaoDeEstoques == null)
            {
                return NotFound();
            }

            var transacaoDeEstoque = await _context.TransacaoDeEstoques
                .Include(t => t.OrdemDeCompra)
                .Include(t => t.OrdemDeVenda)
                .Include(t => t.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transacaoDeEstoque == null)
            {
                return NotFound();
            }

            return View(transacaoDeEstoque);
        }

        // POST: TransacaoDeEstoques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TransacaoDeEstoques == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.TransacaoDeEstoques'  is null.");
            }
            var transacaoDeEstoque = await _context.TransacaoDeEstoques.FindAsync(id);
            if (transacaoDeEstoque != null)
            {
                _context.TransacaoDeEstoques.Remove(transacaoDeEstoque);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransacaoDeEstoqueExists(int id)
        {
          return (_context.TransacaoDeEstoques?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
