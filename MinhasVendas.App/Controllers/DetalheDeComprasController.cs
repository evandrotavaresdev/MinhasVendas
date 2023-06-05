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
    public class DetalheDeComprasController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public DetalheDeComprasController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: DetalheDeCompras
        public async Task<IActionResult> Index()
        {
            var appEstoquesEVendasContext = _context.DetalheDeCompras.Include(d => d.OrdemDeCompra).Include(d => d.Produto);
            return View(await appEstoquesEVendasContext.ToListAsync());
        }

        // GET: DetalheDeCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetalheDeCompras == null)
            {
                return NotFound();
            }

            var detalheDeCompra = await _context.DetalheDeCompras
                .Include(d => d.OrdemDeCompra)
                .Include(d => d.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalheDeCompra == null)
            {
                return NotFound();
            }

            return View(detalheDeCompra);
        }

        // GET: DetalheDeCompras/Create
        public IActionResult Create()
        {
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id");
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id");
            return View();
        }

        // POST: DetalheDeCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,OrdemDeCompraId,TransacaoDeEstoqueId,Quantidade,CustoUnitario")] DetalheDeCompra detalheDeCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalheDeCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id", detalheDeCompra.OrdemDeCompraId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeCompra.ProdutoId);
            return View(detalheDeCompra);
        }

        // GET: DetalheDeCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetalheDeCompras == null)
            {
                return NotFound();
            }

            var detalheDeCompra = await _context.DetalheDeCompras.FindAsync(id);
            if (detalheDeCompra == null)
            {
                return NotFound();
            }
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id", detalheDeCompra.OrdemDeCompraId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeCompra.ProdutoId);
            return View(detalheDeCompra);
        }

        // POST: DetalheDeCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,OrdemDeCompraId,TransacaoDeEstoqueId,Quantidade,CustoUnitario")] DetalheDeCompra detalheDeCompra)
        {
            if (id != detalheDeCompra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalheDeCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalheDeCompraExists(detalheDeCompra.Id))
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
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeCompras, "Id", "Id", detalheDeCompra.OrdemDeCompraId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeCompra.ProdutoId);
            return View(detalheDeCompra);
        }

        // GET: DetalheDeCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetalheDeCompras == null)
            {
                return NotFound();
            }

            var detalheDeCompra = await _context.DetalheDeCompras
                .Include(d => d.OrdemDeCompra)
                .Include(d => d.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalheDeCompra == null)
            {
                return NotFound();
            }

            return View(detalheDeCompra);
        }

        // POST: DetalheDeCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetalheDeCompras == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.DetalheDeCompras'  is null.");
            }
            var detalheDeCompra = await _context.DetalheDeCompras.FindAsync(id);
            if (detalheDeCompra != null)
            {
                _context.DetalheDeCompras.Remove(detalheDeCompra);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalheDeCompraExists(int id)
        {
          return (_context.DetalheDeCompras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
