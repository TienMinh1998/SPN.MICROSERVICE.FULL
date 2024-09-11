using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using DatabaseCore.Infrastructure.Repositories;

namespace Hola.Api.Service.BaseServices
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IRepository<T> _baseReponsitory;
        protected string connectionString;
        public BaseService(IRepository<T> baseReponsitory)
        {
            _baseReponsitory = baseReponsitory;
            connectionString = "Server=194.163.190.91;Port=5432;User Id=postgres;Password=Cvbn152231392;Pooling=true;Timeout=300;CommandTimeout=300;";
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

        public PaginationSet<T> ListPaging(int pageNumber, int pageSize, Func<T, bool> predicate, Dictionary<string, bool> sortList)
        {
            return _baseReponsitory.ListPaging(pageNumber, pageSize, predicate, sortList);
        }

        public PaginationSet<T> GetListPaged(int pageNumber, int pageSize, Func<T, bool> predicate, string sortColumnName, bool descending = false)
        {
            return _baseReponsitory.GetListPaged(pageNumber, pageSize, predicate, sortColumnName, descending);
        }
    }
}
