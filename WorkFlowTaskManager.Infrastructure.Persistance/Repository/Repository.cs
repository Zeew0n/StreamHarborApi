using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Specifications.Base;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkFlowTaskManager.Infrastructure.Persistance.Data;

namespace WorkFlowTaskManager.Infrastructure.Persistence.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Context was not supplied");
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAllIgnoreQueryFilter()
        {
            IQueryable<T> query = _dbSet.IgnoreQueryFilters().AsNoTracking();
            return query.AsNoTracking();
        }

        public IQueryable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null && pageSize != 0)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query.AsNoTracking();
        }

        public IQueryable<T> GetAll(ISpecification<T> spec)
        {
            return ApplySpecification(spec);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           List<Expression<Func<T, object>>> includes = null,
           int? page = null,
           int? pageSize = null)
        {
            return await GetAll(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

        public T Insert(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
            return entity;
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(Guid id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(Guid userId, List<Guid> roles)
        {
            foreach (var item in roles)
            {
                T entityToDelete = _dbSet.Find(userId, item);
                Delete(entityToDelete);
            }
        }

        public async void DeleteAsync(Guid id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public void Delete(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}