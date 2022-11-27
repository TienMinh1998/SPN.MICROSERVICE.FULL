using Hola.Api.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Hola.Api.Service.BaseServices
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IRepository<T> _baseReponsitory;

        public BaseService(IRepository<T> baseReponsitory)
        {
            _baseReponsitory = baseReponsitory;
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _baseReponsitory.AddAsync(entity);
        }

        public async Task<bool> AddManyAsync(IEnumerable<T> entities)
        {
            return await _baseReponsitory.AddManyAsync(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            await _baseReponsitory.DeleteAsync(entity);
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            return await _baseReponsitory.GetAllAsync(predicate, orderBy, include);
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            return await _baseReponsitory.GetFirstOrDefaultAsync(predicate, orderBy, include);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _baseReponsitory.UpdateAsync(entity);
        }
        public int Count(Expression<Func<T, bool>> where)
        {
            return _baseReponsitory.Count(where);
        }

        public IEnumerable<T> FromSqlQuery(string sql, bool allowTracking)
        {
            return _baseReponsitory.FromSqlQuery(sql, allowTracking);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where = null)
        {
            return await _baseReponsitory.CountAsync(where);
        }

    }
}
