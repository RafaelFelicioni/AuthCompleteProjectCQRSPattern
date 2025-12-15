using System.Linq.Expressions;

namespace CleanArchMonolit.Application.Auth.Interfaces.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<List<TEntity>> GetAll();
        IQueryable<TEntity> GetDbSetQuery();
        Task<TEntity> FindAsync(int key);
        Task<IQueryable<TEntity>> Where(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);
        Task AddListAsync(IEnumerable<TEntity> entities);
        Task UpdateListAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task<int> SaveChangesAsync();
        //Task AddWithLogAsync(TEntity entity);
        //Task UpdateWithLog(TEntity entity, int id);
        //Task RemoveWithLog(TEntity entity, int id);
        //Task SaveChangesWithAuditAsync();
    }
}
