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
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Notificador;
using MinhasVendas.App.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MinhasVendas.App.Controllers;

public class DetalheDeComprasController : BaseController
{    
    private readonly IDetalheDeCompraServico _detalheDeCompraServico;

    public DetalheDeComprasController(IDetalheDeCompraServico detalheDeCompraServico,
                                      INotificador notificador) : base(notificador)
    {        
        _detalheDeCompraServico = detalheDeCompraServico;
    }

    [HttpGet]
    public async Task<IActionResult> InserirProduto(int id)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel(); // InserirProduto na ViewModel?            

        await _detalheDeCompraServico.InserirProdutoStatus(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        ViewData["OrdemDeCompraId"] = id;

        return PartialView("_InserirProduto", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InserirProduto([Bind("Id,OrdemDeCompraId,ProdutoId,Quantidade,CustoUnitario")] DetalheDeCompra detalheDeCompra)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        ViewData["OrdemDeCompraId"] = detalheDeCompra.OrdemDeCompraId;

        if (!ModelState.IsValid) return PartialView("_InserirProduto", model);

        await _detalheDeCompraServico.Adicionar(detalheDeCompra);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
    }

    [HttpGet]
    public async Task<IActionResult> ReceberProduto(int id)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        var detalheDeCompra = await _detalheDeCompraServico.ConsultaDetalheDeCompraProdutoOrdemDeCompra(id);

        if (detalheDeCompra == null) return NotFound("Item de Venda não encontrado");

        await _detalheDeCompraServico.VerificarStatus(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        model.DetalheDeCompra = detalheDeCompra;

        return PartialView("_ReceberProduto", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReceberProduto(CarrinhoDeComprasViewModel carrinhoDeComprasViewModel)
    {

        var detalheDeCompra = await _detalheDeCompraServico.Consulta(carrinhoDeComprasViewModel.DetalheDeCompra.Id);

        if (detalheDeCompra == null) return NotFound("Item não encontrado.");

        await _detalheDeCompraServico.RecberProduto(detalheDeCompra);

        if (!OperacaoValida()) return PartialView("_ReceberProduto", carrinhoDeComprasViewModel);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
    }

    [HttpGet]
    public async Task<IActionResult> ExcluirProduto(int id)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        var detalheDeCompra = await _detalheDeCompraServico.ConsultaDetalheDeCompraProdutoOrdemDeCompra(id);

        if (detalheDeCompra == null) return NotFound("Item não encontrado.");

        model.DetalheDeCompra = detalheDeCompra;

        await _detalheDeCompraServico.VerificarStatus(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return PartialView("_ExcluirProduto", model);
    }

    [HttpPost, ActionName("ExcluirProduto")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var detalheDeCompra = await _detalheDeCompraServico.Consulta(id);

        if (detalheDeCompra is null) return NotFound("Item não encontrado");

        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        model.DetalheDeCompra = detalheDeCompra;

        await _detalheDeCompraServico.Remover(id);

        if (!OperacaoValida()) return PartialView("_ExcluirProduto", model); // Implementar Modal Ajax. Não chega aqui por causa do Get ExcluirProduto() "VerificarStatus"

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });

    }   
}
