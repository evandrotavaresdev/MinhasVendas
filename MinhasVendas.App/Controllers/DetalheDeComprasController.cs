using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MinhasVendas.App.Controllers
{
    public class DetalheDeComprasController : BaseController
    {
        private readonly MinhasVendasAppContext _context;
        private readonly IDetalheDeCompraServico _detalheDeCompraServico;

        public DetalheDeComprasController(MinhasVendasAppContext context,
                                          IDetalheDeCompraServico detalheDeCompraServico,
                                          INotificador notificador) : base(notificador)
        {
            _context = context;
            _detalheDeCompraServico = detalheDeCompraServico;
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
                                     ProdutoCompleto = c.Nome + " | " + "Custo: R$ " + " " + c.PrecoBase
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
                _context.Add(detalheDeCompra);
                await _context.SaveChangesAsync();

                return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Id", detalheDeCompra.ProdutoId);           
            ViewData["OrdemDeCompraId"] = new SelectList(_context.OrdemDeVendas, "Id", "Id", detalheDeCompra.OrdemDeCompraId);

            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();
            model.DetalheDeCompra = detalheDeCompra;

            return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
        }


        [HttpGet]
        public async Task<IActionResult> ReceberProduto(int? id)
        {


            if (id == null || _context.DetalheDeVendas == null)
            {
                return NotFound();
            }

            var detalheDeCompra = await _context.DetalheDeCompras
                .Include(v => v.Produto)
                .Include(v => v.OrdemDeCompra)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (detalheDeCompra == null)
            {
                return NotFound();
            }

            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();
            model.DetalheDeCompra = detalheDeCompra;

            return PartialView("_ReceberProduto", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceberProduto(CarrinhoDeComprasViewModel carrinhoDeComprasViewModel)
        {


            var detalheDeCompra = await _context.DetalheDeCompras.FindAsync(carrinhoDeComprasViewModel.DetalheDeCompra.Id);
                                        
                                     
            if (detalheDeCompra == null)
            {
                return NotFound("Erro ao Receber o Produo.");
            }

            detalheDeCompra.DataDeRecebimento = carrinhoDeComprasViewModel.DetalheDeCompra.DataDeRecebimento;
            detalheDeCompra.RegistradoTransacaoDeEstoque = true;
            _context.Update(detalheDeCompra);
            await _context.SaveChangesAsync();

            TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();
            transacaoDeEstoque.ProdutoId = detalheDeCompra.ProdutoId;
            transacaoDeEstoque.OrdemDeCompraId = detalheDeCompra.OrdemDeCompraId;
            transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Compra;
            transacaoDeEstoque.DataDeTransacao = DateTime.Now;
            transacaoDeEstoque.Quantidade = detalheDeCompra.Quantidade;
            _context.Add(transacaoDeEstoque);
            await _context.SaveChangesAsync();
           
            return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
        }


        [HttpGet]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            if (id == null || _context.DetalheDeVendas == null)
            {
                return NotFound();
            }

            

            var detalheDeCompra = await _context.DetalheDeCompras
                .Include(v => v.Produto)
                .Include(v => v.OrdemDeCompra)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (detalheDeCompra == null)
            {
                return NotFound();
            }

            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();
            model.DetalheDeCompra = detalheDeCompra;
            await _detalheDeCompraServico.VerificarStatus(id);

            return PartialView("_ExcluirProduto", model);
        }
        
        [HttpPost, ActionName("ExcluirProduto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            if (_context.DetalheDeCompras == null) return Problem("Entity set 'AuroraCollabContext.VendaDetalhe'  is null.");
            
            var detalheDeCompra = await _context.DetalheDeCompras.FindAsync(id);
            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();
            model.DetalheDeCompra = detalheDeCompra;

            await _detalheDeCompraServico.Remover(id);
                       

            if (!OperacaoValida()) return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });

            return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
            

        }
        [HttpGet]
        public async Task<IActionResult> FinalizarVenda(int id)
        {
            var ordemDeCompra = await _context.OrdemDeCompras
                 .Include(v => v.DetalheDeCompras)
                 .FirstOrDefaultAsync(v => v.Id == id);

            ViewData["OrdemDeCompraId"] = id;
            CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

            if (ordemDeCompra.DetalheDeCompras.Any())
            {
                var custoProduto = from item in ordemDeCompra.DetalheDeCompras select (item.CustoUnitario * item.Quantidade);
                decimal[] custoProdutos = custoProduto.ToArray();
                decimal totalCompra= custoProdutos.Aggregate((a, b) => a + b);
                model.TotalCompra = totalCompra;
            }
            return PartialView("_FinalizarVenda", model);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarVenda(CarrinhoDeComprasViewModel model)
        {
            var ordemDeCompra = await _context.OrdemDeCompras.FindAsync(model.DetalheDeCompra.OrdemDeCompraId);
            if (ordemDeCompra == null)
            {
                return NotFound("Erro ao finalizar a Venda.");
            }

            ordemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Aprovado;
            ordemDeCompra.FormaDePagamento = model.OrdemDeCompra.FormaDePagamento;

            _context.Update(ordemDeCompra);
            await _context.SaveChangesAsync();

            return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

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
