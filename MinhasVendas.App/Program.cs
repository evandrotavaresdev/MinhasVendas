using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;
using MinhasVendas.App.Notificador;
using MinhasVendas.App.Repositorio;
using MinhasVendas.App.Servicos;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<MinhasVendasAppContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MinhasVendasAppContext") ?? throw new InvalidOperationException("Connection string 'MinhasVendasAppContext' not found.")));

builder.Services.AddDbContext<MinhasVendasAppContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var CUSTOMCONNSTR_ = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<INotificador, Notificador>();

builder.Services.AddScoped<IProdutoServico, ProdutoServico>();
builder.Services.AddScoped<IDetalheDeCompraServico, DetalheDeCompraServico>();
builder.Services.AddScoped<IOrdemDeCompraServico, OrdemDeCompraServico>();
builder.Services.AddScoped<IOrdemDeVendaServico, OrdemDeVendaServico>();
builder.Services.AddScoped<IDetalheDeVendaServico, DetalheDeVendaServico>();
builder.Services.AddScoped<ITransacaoDeEstoqueServico, TransacaoDeEstoqueServico>();
builder.Services.AddScoped<IFornecedorServico, FornecedorServico>();
builder.Services.AddScoped<IClienteServico, ClienteServico>();


builder.Services.AddScoped<IClienteRespositorio, ClienteRepositorio>();
builder.Services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IOrdemDeCompraRepositorio, OrdemDeCompraRepositorio>();
builder.Services.AddScoped<IOrdemDeVendaRepositorio, OrdemDeVendaRepositorio>();
builder.Services.AddScoped<IDetalheDeCompraRepositorio, DetalheDeCompraRepositorio>();
builder.Services.AddScoped<IDetalheDeVendaRepositorio, DetalheDeVendaRepositorio>();
builder.Services.AddScoped<ITransacaoDeEstoqueRepositorio, TransacaoDeEstoqueRepositorio>();



var app = builder.Build();



// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
