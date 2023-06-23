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
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers
{
    public class OrdemDeComprasController : BaseController
    {
        private readonly MinhasVendasAppContext _context;
        private readonly IOrdemDeCompraServico _ordemDeCompraServico;

        public OrdemDeComprasController(MinhasVendasAppContext context,
                                        IOrdemDeCompraServico ordemDeCompraServico,
                                        INotificador notificador) : base(notificador)
        {
            _context = context;
            _ordemDeCompraServico = ordemDeCompraServico;
        }
        public async Task<IActionResult> CarrinhoDeCompras(int? id)
        {
            if (id == null || _context.OrdemDeCompras == null)
            {
                return NotFound();
            }

            var ordemDeCompra = await _context.OrdemDeCompras
                 .Include(c => c.Fornecedor)
                 .Include(c => c.DetalheDeCompras).ThenInclude(v => v.Produto)
                 .FirstOrDefaultAsync(m => m.Id == id);


            if (ordemDeCompra == null)
            {
                return NotFound();
            }


            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "NomeProduto");

            var model = new CarrinhoDeComprasViewModel();
            model.OrdemDeCompra = ordemDeCompra;

            if (ordemDeCompra.DetalheDeCompras.Any())
            {
                var precoProduto = from item in model.OrdemDeCompra.DetalheDeCompras select (item.CustoUnitario * item.Quantidade);
                decimal[] precoProdutos = precoProduto.ToArray();
                decimal totalCompra = precoProdutos.Aggregate((a, b) => a + b) + ordemDeCompra.ValorDeFrete;
                model.TotalCompra = totalCompra;

                var itens = from item in model.OrdemDeCompra.DetalheDeCompras select (item.Quantidade);
                int totalItens = itens.Sum();
                model.TotalItens = totalItens;
            }


            return View(model);
        }


        //
        //
        //

        [HttpGet]
        public async Task<IActionResult> FinalizarCompra(int id)
        {
            ViewData["OrdemDeCompraId"] = id;

            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

            await _ordemDeCompraServico.FinalizarCompraView(id);

            if (!OperacaoValida()) return PartialView("_OrdemDeCompraAberta", model);

            var ordemDeCompra =
                await _context.OrdemDeCompras
               .Include(v => v.DetalheDeCompras)
               .FirstOrDefaultAsync(v => v.Id == id);


            model.OrdemDeCompra = ordemDeCompra;

            return PartialView("_FinalizarCompra", model);
        }


        [HttpPost]
        public async Task<IActionResult> FinalizarCompra(int id, CarrinhoDeComprasViewModel model)
        {
            if (id != model.OrdemDeCompra.Id) return NotFound();

            var ordemDeCompra = await _context.OrdemDeCompras.FindAsync(model.OrdemDeCompra.Id);

            if (ordemDeCompra == null) return NotFound();           

            await _ordemDeCompraServico.FinalizarCompra(model.OrdemDeCompra);

            if (!OperacaoValida()) return PartialView("_OrdemDeCompraAberta", model);
           
            return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

        }

        // GET: OrdemDeCompras
        public async Task<IActionResult> Index()
        {
            var minhasVendasAppContext = _context.OrdemDeCompras.Include(o => o.Fornecedor);
            return View(await minhasVendasAppContext.ToListAsync());
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
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Nome");
            return View();
        }

        // POST: OrdemDeCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FornecedorId,DataDeCriacao,StatusOrdemDeCompra,ValorDeFrete")] OrdemDeCompra ordemDeCompra)
        {
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", ordemDeCompra.FornecedorId);
         
            if (!ModelState.IsValid) return View(ordemDeCompra);
                               
            await _ordemDeCompraServico.Adicionar(ordemDeCompra);
            
            if (!OperacaoValida()) return View(ordemDeCompra);

            return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });
           // return RedirectToAction(nameof(Index));
            
            
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FornecedorId,DataDeCriacao,StatusOrdemDeCompra,ValorDeFrete")] OrdemDeCompra ordemDeCompra)
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
                return Problem("Entity set 'MinhasVendasAppContext.OrdemDeCompras'  is null.");
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
