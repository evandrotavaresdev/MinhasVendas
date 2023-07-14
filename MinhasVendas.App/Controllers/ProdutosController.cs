using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;

namespace MinhasVendas.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly MinhasVendasAppContext _context;
        private readonly IProdutoServico _produtoServico;

        public ProdutosController(MinhasVendasAppContext context,
                                  INotificador notificador,
                                  IProdutoServico produtoServico) : base(notificador)
        {
            _context = context;
            _produtoServico = produtoServico;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            var qtdCompraEVenda =
                      from produto in _context.TransacaoDeEstoques
                      group produto by produto.ProdutoId into produtoGroup
                      select new
                      {
                          produtoGroup.Key,
                          totalProdutoComprado = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra).Sum(p => p.Quantidade),
                          totalProdutoVendido = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Venda).Sum(p => p.Quantidade)
                      };

            var produtos = await _context.Produtos.ToListAsync();

            foreach (var produto in qtdCompraEVenda)
            {
                var item = produtos.Find(p => p.Id == produto.Key);
                item.EstoqueAtual = produto.totalProdutoComprado - produto.totalProdutoVendido;
            }
            return _context.Produtos != null ? 
                          View(produtos) :
                          Problem("Entity set 'MinhasVendasContext.Produtos'  is null.");
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,PrecoDeLista,PrecoBase")] Produto produto)
        {
            if (!ModelState.IsValid) return View(produto);

            await _produtoServico.Adicionar(produto);

            if (!OperacaoValida()) return View(produto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,PrecoDeLista,PrecoBase")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
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
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.Produtos'  is null.");
            }
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
          return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
