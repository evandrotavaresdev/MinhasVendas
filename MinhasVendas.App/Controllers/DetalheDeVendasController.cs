using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers
{
    public class DetalheDeVendasController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public DetalheDeVendasController(MinhasVendasAppContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> InserirProduto(int id)
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

            var listaProdutos = (from c in produtos
                                 select new
                                 {
                                     Id = c.Id,
                                     NomeProtudo = c.Nome,
                                     Preco = c.PrecoDeLista,
                                     EstoqueAtual = c.EstoqueAtual,
                                     ProdutoCompleto = c.Nome + " | " + "Valor: R$ " + " " + c.PrecoDeLista + " | " + c.EstoqueAtual
                                 });
           
            ViewData["ProdutoId"] = new SelectList(listaProdutos, "Id", "ProdutoCompleto");            
            ViewData["OrdemDeVendaId"] = id;           

            CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

            return PartialView("_InserirProduto", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InserirProduto([Bind("Id,OrdemDeVendaId,ProdutoId,Quantidade,PrecoUnitario,Desconto")] DetalheDeVenda detalheDeVenda)
        {
            if (ModelState.IsValid)
            {
                var precoUnitario = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == detalheDeVenda.ProdutoId);
                detalheDeVenda.PrecoUnitario = precoUnitario.PrecoDeLista;

                TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();
                transacaoDeEstoque.ProdutoId = detalheDeVenda.ProdutoId;
                transacaoDeEstoque.OrdemDeVendaId = detalheDeVenda.OrdemDeVendaId;
                transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Venda;
                transacaoDeEstoque.DataDeTransacao = DateTime.Now;
                transacaoDeEstoque.Quantidade = detalheDeVenda.Quantidade;
               
                _context.TransacaoDeEstoques.Add(transacaoDeEstoque);
                await _context.SaveChangesAsync();

                detalheDeVenda.TransacaoDeEstoqueId = transacaoDeEstoque.Id;

                _context.Add(detalheDeVenda);
                await _context.SaveChangesAsync();
                return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = detalheDeVenda.OrdemDeVendaId });
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeVenda.ProdutoId);
            ViewData["TransacaoDeEstoqueId"] = new SelectList(_context.TransacaoDeEstoques, "Id", "Id", detalheDeVenda.TransacaoDeEstoqueId);            
            ViewData["OrdemDeVendaId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", detalheDeVenda.OrdemDeVendaId);

            CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();
            model.DetalheDeVenda = detalheDeVenda;

            return View(model);
        }

        // GET: VendasDetalhes/Delete/5
        public async Task<IActionResult> ExcluirProduto(int? id)
        {
            if (id == null || _context.DetalheDeVendas == null)
            {
                return NotFound();
            }

            var detalheDeVenda = await _context.DetalheDeVendas
                .Include(v => v.Produto)
                .Include(v => v.OrdemDeVenda)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (detalheDeVenda == null)
            {
                return NotFound();
            }

            CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();
            model.DetalheDeVenda = detalheDeVenda;

            return PartialView("_ExcluirProduto", model);
        }

        // POST: VendasDetalhes/Delete/5
        [HttpPost, ActionName("ExcluirProduto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            if (_context.DetalheDeVendas == null)
            {
                return Problem("Entity set 'AuroraCollabContext.VendaDetalhe'  is null.");
            }
            var detalheDeVenda = await _context.DetalheDeVendas.FindAsync(id);
            if (detalheDeVenda != null)
            {
                _context.DetalheDeVendas.Remove(detalheDeVenda);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = detalheDeVenda.OrdemDeVendaId });
        }

        public async Task<IActionResult> FinalizarVenda(int id)
        {
            var ordemDeVenda = await _context.OrdemDeVendas
                 .Include(v => v.DetalheDeVendas)
                 .FirstOrDefaultAsync(v => v.Id == id);

            ViewData["OrdemDeVendaId"] = id;
            CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

            if (ordemDeVenda.DetalheDeVendas.Any())
            {
                var precoProduto = from item in ordemDeVenda.DetalheDeVendas select (item.PrecoUnitario * item.Quantidade * (1 - item.Desconto / 100));
                decimal[] precoProdutos = precoProduto.ToArray();
                decimal totalVenda = precoProdutos.Aggregate((a, b) => a + b);
                model.TotalVenda = totalVenda;
            }

            return PartialView("_FinalizarVenda", model);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarVenda(CarrinhoDeVendasViewModel model)
        {
            var ordemDeVenda = await _context.OrdemDeVendas.FindAsync(model.DetalheDeVenda.OrdemDeVendaId);
            if (ordemDeVenda == null)
            {
                return NotFound("Erro ao finalizar a Venda.");
            }

            ordemDeVenda.StatusOrdemDeVenda = StatusOrdemDeVenda.Vendido;
            ordemDeVenda.FormaDePagamento = model.OrdemDeVenda.FormaDePagamento;
           
            _context.Update(ordemDeVenda);
            await _context.SaveChangesAsync();

            return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = ordemDeVenda.Id });

        }

        /* */

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
