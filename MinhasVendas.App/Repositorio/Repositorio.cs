using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;
using System.Linq.Expressions;

namespace MinhasVendas.App.Repositorio
{
    public abstract class Repositorio<T> : IRepositorio<T> where T : Entidade, new()
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        private readonly DbSet<T> _dbSet;
        
        public Repositorio(MinhasVendasAppContext minhasVendasAppContext)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
            _dbSet = _minhasVendasAppContext.Set<T>();
        }

        public async Task Adicionar(T entidade)
        {
            _dbSet.Add(entidade);
            await SalvarMudancas();
        }

        public async Task Atualizar(T entidade)
        {
            _dbSet.Update(entidade);
            await SalvarMudancas();
        }
        public async Task Remover(int id)
        {
            _dbSet.Remove(new T { Id = id });
            await SalvarMudancas();
        }

        public async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T> BuscarPorId(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> BuscarTodos()
        {
            return await _dbSet.ToListAsync();
        }


        public IQueryable<T> Obter()
        {
            return _dbSet;

        }

        public IQueryable<T> ObterSemRastreamento()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<T> ObterPorId(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public void Desanexar(T entidade)
        {
            _minhasVendasAppContext.Entry(entidade).State = EntityState.Detached;
        }


        public async Task<int> SalvarMudancas()
        {
            return await _minhasVendasAppContext.SaveChangesAsync();
        }

        public void Dispose()
        {
           _minhasVendasAppContext?.Dispose();
        }
    }
}
