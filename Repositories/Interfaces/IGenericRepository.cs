using System.Linq.Expressions;

namespace MyPortfolio.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Queryable
        IQueryable<TEntity> Query();
        IQueryable<TEntity> QueryAsNoTracking();

        // Get single
        Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        // Get list
        Task<List<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<List<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        // Paging
        Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        Task<(List<TResult> Items, int TotalCount)> GetPagedAsync<TResult>(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        // Exists / count
        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        // Create
        Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);

        Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default);

        // Update
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        // Delete
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        Task<bool> DeleteByIdAsync(
            object id,
            CancellationToken cancellationToken = default);

        // Attach
        void Attach(TEntity entity);
        void Detach(TEntity entity);
    }
}
