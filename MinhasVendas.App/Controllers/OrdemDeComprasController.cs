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
    public class OrdemDeComprasController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public OrdemDeComprasController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: OrdemDeCompras
        public async Task<IActionResult> Index()
        {
            var appEstoquesEVendasContext = _context.OrdemDeCompras.Include(o => o.Fornecedor);
            return View(await appEstoquesEVendasContext.ToListAsync());
        }

        // GET: OrdemDeCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrdemDeCompras == null)
            {
                return NotFound();
            }

            var ordemDeCompra = await _context.OrdemDeCompras
                .Include(o => o.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordemDeCompra == null)
            {
                return NotFound();
            }

            return View(ordemDeCompra);
        }

        // GET: OrdemDeCompras/Create
        public IActionResult Create()
        {
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id");
            return View();
        }

        // POST: OrdemDeCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FornecedorId")] OrdemDeCompra ordemDeCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordemDeCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", ordemDeCompra.FornecedorId);
            return View(ordemDeCompra);
        }

        // GET: OrdemDeCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrdemDeCompras == null)
            {
                return NotFound();
            }

            var ordemDeCompra = await _context.OrdemDeCompras.FindAsync(id);
            if (ordemDeCompra == null)
            {
                return NotFound();
            }
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", ordemDeCompra.FornecedorId);
            return View(ordemDeCompra);
        }

        // POST: OrdemDeCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FornecedorId")] OrdemDeCompra ordemDeCompra)
        {
            if (id != ordemDeCompra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordemDeCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdemDeCompraExists(ordemDeCompra.Id))
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
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", ordemDeCompra.FornecedorId);
            return View(ordemDeCompra);
        }

        // GET: OrdemDeCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrdemDeCompras == null)
            {
                return NotFound();
            }

            var ordemDeCompra = await _context.OrdemDeCompras
                .Include(o => o.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordemDeCompra == null)
            {
                return NotFound();
            }

            return View(ordemDeCompra);
        }

        // POST: OrdemDeCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrdemDeCompras == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.OrdemDeCompras'  is null.");
            }
            var ordemDeCompra = await _context.OrdemDeCompras.FindAsync(id);
            if (ordemDeCompra != null)
            {
                _context.OrdemDeCompras.Remove(ordemDeCompra);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdemDeCompraExists(int id)
        {
          return (_context.OrdemDeCompras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
