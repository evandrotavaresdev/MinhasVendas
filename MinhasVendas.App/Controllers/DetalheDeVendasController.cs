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
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;

public class DetalheDeVendasController : BaseController
{   
    private readonly IDetalheDeVendaServico _detalheDeVendaServico;
    private readonly ITransacaoDeEstoqueServico _transacaoDeEstoqueServico;
    private readonly IProdutoServico _produtoServico;

    public DetalheDeVendasController(IDetalheDeVendaServico detalheDeVendaServico,
                                     ITransacaoDeEstoqueServico transacaoDeEstoqueServico,
                                     IProdutoServico produtoServico,
                                     INotificador notificador) : base(notificador)
    {        
        _detalheDeVendaServico = detalheDeVendaServico;
        _transacaoDeEstoqueServico = transacaoDeEstoqueServico;
        _produtoServico = produtoServico;
    }
    [HttpGet]
    public async Task<IActionResult> InserirProduto(int id)
    {

        var listaDeProdutos = await _transacaoDeEstoqueServico.ConsultaSaldoDeEstoque();

        ViewData["ProdutoId"] = new SelectList(listaDeProdutos, "Id", "ProdutoCompleto");
        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        await _detalheDeVendaServico.AdicionarView(id);

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        return PartialView("_InserirProduto", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InserirProduto([Bind("Id,OrdemDeVendaId,ProdutoId,Quantidade,PrecoUnitario,Desconto")] DetalheDeVenda detalheDeVenda)  //
    {
        var listaDeProdutos = await _transacaoDeEstoqueServico.ConsultaSaldoDeEstoque();

        ViewData["ProdutoId"] = new SelectList(listaDeProdutos, "Id", "ProdutoCompleto");
        ViewData["OrdemDeVendaId"] = detalheDeVenda.OrdemDeVendaId;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();
        model.DetalheDeVenda = detalheDeVenda;

        if (!ModelState.IsValid) return PartialView("_InserirProduto", model);

        await _detalheDeVendaServico.Adicionar(detalheDeVenda);

        if (!OperacaoValida()) return PartialView("_InserirProduto", model);

        var url = Url.Action("CarrinhoDeVendasPartial", "OrdemDeVendas", new { id = detalheDeVenda.OrdemDeVendaId });
        return Json(new { sucesso = true, url });

    }

    // GET: VendasDetalhes/Delete/5
    public async Task<IActionResult> ExcluirProduto(int id)
    {
        var detalheDeVenda = await _detalheDeVendaServico.ConsultaDetalheDeVendaProdutoOrdemDeVenda(id);

        if (detalheDeVenda is null) return NotFound("Item não encontrado.");

        await _detalheDeVendaServico.Remover(id, true);

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        model.DetalheDeVenda = detalheDeVenda;

        return PartialView("_ExcluirProduto", model);
    }

    // POST: VendasDetalhes/Delete/5
    [HttpPost, ActionName("ExcluirProduto")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var detalheDeVenda = await _detalheDeVendaServico.ConsultaDetalheDeVendaOrdemDeVenda(id);

        if (detalheDeVenda is null) return NotFound("Item Não econtrado.");

        await _detalheDeVendaServico.Remover(id, false);

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { detalheDeVenda.OrdemDeVenda.Id });
    }
}
