using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;

public class OrdemDeComprasController : BaseController
{       
    private readonly IOrdemDeCompraServico _ordemDeCompraServico;
    private readonly IFornecedorRepositorio _fornecedorRepositorio;

    public OrdemDeComprasController(IOrdemDeCompraServico ordemDeCompraServico,
                                    IFornecedorRepositorio fornecedorRepositorio,
                                    INotificador notificador) : base(notificador)
    {
        _ordemDeCompraServico = ordemDeCompraServico;
        _fornecedorRepositorio = fornecedorRepositorio;
    }

    public async Task<IActionResult> Index()
    {
        var OrdemDeCompraFornecedor = await _ordemDeCompraServico.ConsultaOrdemDeCompraFornecedor();
        return View(OrdemDeCompraFornecedor);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["FornecedorId"] = new SelectList(await  _fornecedorRepositorio.BuscarTodos(), "Id", "Nome");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FornecedorId,DataDeCriacao,StatusOrdemDeCompra,ValorDeFrete")] OrdemDeCompra ordemDeCompra)
    {
        ViewData["FornecedorId"] = new SelectList(await _fornecedorRepositorio.BuscarTodos(), "Id", "Nome", ordemDeCompra.FornecedorId);

        if (!ModelState.IsValid) return View(ordemDeCompra);

        await _ordemDeCompraServico.Adicionar(ordemDeCompra);

        if (!OperacaoValida()) return View(ordemDeCompra);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

    }
    public async Task<IActionResult> CarrinhoDeCompras(int id)
    {
        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(id);

        if (ordemDeCompra is null) NotFound("Ordem de Compra não Existe.");

        var model = new CarrinhoDeComprasViewModel();

        model.OrdemDeCompra = ordemDeCompra;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> FinalizarCompra(int id)
    {
        ViewData["OrdemDeCompraId"] = id;

        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        await _ordemDeCompraServico.FinalizarCompraView(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompraDetalheDeCompra(id);

        model.OrdemDeCompra = ordemDeCompra;

        return PartialView("_FinalizarCompra", model);
    }


    [HttpPost]
    public async Task<IActionResult> FinalizarCompra(int id, CarrinhoDeComprasViewModel model)
    {
        if (id != model.OrdemDeCompra.Id) return NotFound();

        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompra(model.OrdemDeCompra.Id);

        if (ordemDeCompra == null) return NotFound();

        await _ordemDeCompraServico.FinalizarCompra(model.OrdemDeCompra);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

    }

}

