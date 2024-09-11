using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using DatabaseCore.Infrastructure.Repositories;


namespace Hola.Api.Service.BaseServices
{
    public interface IBaseService<T> where T : class
    {

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
        IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
        IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<T> AddAsync(T entity);

        Task<bool> AddManyAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        int Count(Expression<Func<T, bool>> where);

        Task<int> CountAsync(Expression<Func<T, bool>> where = null);
        IEnumerable<T> FromSqlQuery(string sql, bool allowTracking);

        PaginationSet<T> ListPaging(int pageNumber, int pageSize, Func<T, bool> predicate, Dictionary<string, bool> sortList);

        PaginationSet<T> GetListPaged(int pageNumber, int pageSize, Func<T, bool> predicate, string sortColumnName, bool descending = false);

    }
}
