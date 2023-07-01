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
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;

public class OrdemDeVendasController : BaseController
{       
    private readonly IOrdemDeVendaServico _ordemDeVendaServico;
    private readonly IClienteServico _clienteServico;

    public OrdemDeVendasController(MinhasVendasAppContext context,
                                   IOrdemDeVendaServico ordemDeVendaServico,
                                   IClienteServico clienteServico,
                                   INotificador notificador) : base(notificador)
    {
        _ordemDeVendaServico = ordemDeVendaServico;
        _clienteServico = clienteServico;
    }

    public async Task<IActionResult> Index()
    {
        var ordemDeVendasClientes = await _ordemDeVendaServico.ConsultaOrdemDevendaCliente();

        return View(ordemDeVendasClientes);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["ClienteId"] = new SelectList(await _clienteServico.ConsutaClientes(), "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ClienteId,StatusOrdemDeVenda,FormaDePagamento,DataDePagamento,DataDeVenda")] OrdemDeVenda ordemDeVenda)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ClienteId"] = new SelectList(await _clienteServico.ConsutaClientes(), "Id", "Nome", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
        }

        await _ordemDeVendaServico.Adicionar(ordemDeVenda);

        if (!OperacaoValida())
        {
            ViewData["ClienteId"] = new SelectList(await _clienteServico.ConsutaClientes(), "Id", "Nome", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
        }

        return RedirectToAction("CarrinhoDeVendas", new { id = ordemDeVenda.Id });
    }

    public async Task<IActionResult> CarrinhoDeVendas(int id)
    {
        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(id);

        if (ordemDeVenda == null) return NotFound("Carrinho de Compra não Existe.");

        var model = new CarrinhoDeVendasViewModel();

        model.OrdemDeVenda = ordemDeVenda;

        return View(model);
    }

    public async Task<IActionResult> CarrinhoDeVendasPartial(int id)
    {
        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(id);

        if (ordemDeVenda == null) return NotFound("Carrinho de Compra não EXISTE.");

        var model = new CarrinhoDeVendasViewModel();

        model.OrdemDeVenda = ordemDeVenda;

        return PartialView("CarrinhoDeVendas", model);
    }

    public async Task<IActionResult> FinalizarVenda(int id)
    {
        await _ordemDeVendaServico.FinalizarVendaView(id);

        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaAberta", model);

        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalheDeVenda(id);

        model.OrdemDeVenda = ordemDeVenda;

        return PartialView("_FinalizarVenda", model);
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarVenda(int id, CarrinhoDeVendasViewModel model)
    {
        if (id != model.OrdemDeVenda.Id) return NotFound();

        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVenda(model.OrdemDeVenda.Id);

        if (ordemDeVenda is null) return NotFound();

        await _ordemDeVendaServico.FinalizarVenda(model.OrdemDeVenda);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraAberta", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = ordemDeVenda.Id });

    }
}
