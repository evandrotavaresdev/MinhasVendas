using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Models;
using System.Linq.Expressions;

namespace MinhasVendas.App.Interfaces.Repositorio;

public interface IRepositorio<T> : IDisposable where T : Entidade
{
    Task Adicionar(T entidade);
    Task Atualizar(T entidade);
    Task Remover(int id);
    Task<T> BuscarPorId(int id);
    Task<List<T>> BuscarTodos();
    Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);

    IQueryable<T> Obter();
    IQueryable<T> ObterSemRastreamento();
    Task<T> ObterPorId(Expression<Func<T, bool>> predicate);
    void Desanexar(T entidade);

    Task<int> SalvarMudancas();


}
