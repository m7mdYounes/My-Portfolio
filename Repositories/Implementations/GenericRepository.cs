using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Repositories.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace MyPortfolio.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<TEntity> QueryAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public async Task<TEntity?> GetByIdAsync(
            object id,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync([id], cancellationToken);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (include is not null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (include is not null)
                query = include(query);

            return await query.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<List<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (include is not null)
                query = include(query);

            if (predicate is not null)
                query = query.Where(predicate);

            if (orderBy is not null)
                query = orderBy(query);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (predicate is not null)
                query = query.Where(predicate);

            if (orderBy is not null)
                query = orderBy(query);

            return await query.Select(selector).ToListAsync(cancellationToken);
        }

        public async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            ValidatePaging(pageNumber, pageSize);

            IQueryable<TEntity> query = DbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (include is not null)
                query = include(query);

            if (predicate is not null)
                query = query.Where(predicate);

            var totalCount = await query.CountAsync(cancellationToken);

            if (orderBy is not null)
                query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<TResult> Items, int TotalCount)> GetPagedAsync<TResult>(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            ValidatePaging(pageNumber, pageSize);

            IQueryable<TEntity> query = DbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (predicate is not null)
                query = query.Where(predicate);

            var totalCount = await query.CountAsync(cancellationToken);

            if (orderBy is not null)
                query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate is null)
                return await DbSet.CountAsync(cancellationToken);

            return await DbSet.CountAsync(predicate, cancellationToken);
        }

        public async Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public async Task<bool> DeleteByIdAsync(
            object id,
            CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);

            if (entity is null)
                return false;

            Delete(entity);
            return true;
        }

        public void Attach(TEntity entity)
        {
            DbSet.Attach(entity);
        }

        public void Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        private static void ValidatePaging(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

            if (pageSize > 200)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size cannot be greater than 200.");
        }
    }
}
