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
    public class DetalheDeComprasController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public DetalheDeComprasController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> InserirProduto(int id)
        {
           

            var produtos = await _context.Produtos.ToListAsync();

         

            var listaProdutos = (from c in produtos
                                 select new
                                 {
                                     Id = c.Id,
                                     NomeProtudo = c.Nome,
                                     Preco = c.PrecoBase,
                                     ProdutoCompleto = c.Nome + " | " + "Valor: R$ " + " " + c.PrecoBase
                                 });

            ViewData["ProdutoId"] = new SelectList(listaProdutos, "Id", "ProdutoCompleto");
            ViewData["OrdemDeCompraId"] = id;
          
            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

            return PartialView("_InserirProduto", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InserirProduto([Bind("Id,OrdemDeCompraId,ProdutoId,Quantidade,CustoUnitario")] DetalheDeCompra detalheDeCompra)
        {
            if (ModelState.IsValid)
            {                             
                TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();
                transacaoDeEstoque.ProdutoId = detalheDeCompra.ProdutoId;
                transacaoDeEstoque.OrdemDeCompraId = detalheDeCompra.OrdemDeCompraId;
                transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Compra;
                transacaoDeEstoque.DataDeTransacao = DateTime.Now;
                transacaoDeEstoque.Quantidade = detalheDeCompra.Quantidade;

                _context.TransacaoDeEstoques.Add(transacaoDeEstoque);
                await _context.SaveChangesAsync();  

                detalheDeCompra.TransacaoDeEstoqueId = transacaoDeEstoque.Id;
                detalheDeCompra.DataDeRecebimento = DateTime.Now;
                detalheDeCompra.RegistradoTransacaoDeEstoque = true;

                _context.Add(detalheDeCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeCompra.ProdutoId);           
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", detalheDeCompra.OrdemDeCompraId);

            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();
            model.DetalheDeCompra = detalheDeCompra;

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

        // GET: DetalheDeCompras
        public async Task<IActionResult> Index()
        {
            var minhasVendasAppContext = _context.DetalheDeCompras.Include(d => d.OrdemDeCompra).Include(d => d.Produto);
            return View(await minhasVendasAppContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,OrdemDeCompraId,TransacaoDeEstoqueId,Quantidade,CustoUnitario,DataDeRecebimento,RegistradoTransacaoDeEstoque")] DetalheDeCompra detalheDeCompra)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,OrdemDeCompraId,TransacaoDeEstoqueId,Quantidade,CustoUnitario,DataDeRecebimento,RegistradoTransacaoDeEstoque")] DetalheDeCompra detalheDeCompra)
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
                return Problem("Entity set 'MinhasVendasAppContext.DetalheDeCompras'  is null.");
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
