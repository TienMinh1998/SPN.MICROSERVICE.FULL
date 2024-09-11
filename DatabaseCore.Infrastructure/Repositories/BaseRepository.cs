using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;


namespace DatabaseCore.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected EFContext DbContext { get; set; }
    internal DbSet<T> dbSet;
    public BaseRepository(EFContext DbContext)
    {
        this.DbContext = DbContext;
        dbSet = DbContext.Set<T>();

    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>,
        IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T,
         object>> include = null)
    {
        try
        {
            IQueryable<T> query = DbContext.Set<T>().AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
     Func<IQueryable<T>, IIncludableQueryable<T, Object>> include = null)
    {
        try
        {
            IQueryable<T> query = DbContext.Set<T>()
                .AsNoTracking();
            if (include != null) query = include(query);
            query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);

            return await query.FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<T> AddAsync(T entity)
    {
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> AddManyAsync(IEnumerable<T> entities)
    {
        await DbContext.Set<T>().AddRangeAsync(entities);
        await DbContext.SaveChangesAsync();
        return true;
    }
    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
            DbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            throw e;
        }

    }
    public async Task DeleteAsync(T entity)
    {
        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
    public async Task<EFContext> GetDbContext()
    {
        return await Task.FromResult(DbContext);
    }

    public int Count(Expression<Func<T, bool>> where)
    {
        return dbSet.Count(where);
    }
    public virtual void Insert(T entity)
    {
        dbSet.Add(entity);
    }
    public virtual void Delete(T entity)
    {
        if (DbContext.Entry(entity).State == EntityState.Detached)
            dbSet.Attach(entity);
        dbSet.Remove(entity);
    }
    public IEnumerable<T> FromSqlQuery(string sql, bool allowTracking)
    {
        if (allowTracking)
        {
            return dbSet.FromSqlRaw(sql).AsNoTracking();
        }
        return dbSet.FromSqlRaw(sql).AsNoTracking().AsEnumerable();
    }
    public async Task<int> CountAsync(Expression<Func<T, bool>> where = null)
    {
        if (where != null) return await dbSet.AsNoTracking().CountAsync(where);
        return await dbSet.AsNoTracking().CountAsync();

    }

    public virtual void Update(T entity)
    {
        dbSet.Attach(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
    }
    /// <summary>
    /// Phân Trang
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="predicate"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    public PaginationSet<T> ListPaging(int pageNumber, int pageSize, Func<T, bool> predicate, Dictionary<string, bool> sortList)
    {
        IQueryable<T> queryAble;
        var skip = (pageNumber - 1) * pageSize;

        if (predicate != null)
        {
            queryAble = dbSet.Where(predicate).AsQueryable();
        }
        else
        {
            queryAble = dbSet.AsQueryable();
        }

        foreach (var item in sortList)
        {
            queryAble = OrderByDynamic(queryAble, item.Key, item.Value);
        }

        var total = queryAble.Count();
        var resultSet = queryAble.Skip(skip).Take(pageSize);

        var paginationSet = new PaginationSet<T>()
        {
            Items = resultSet,
            TotalCount = total
        };
        return paginationSet;
    }


    public PaginationSet<T> GetListPaged(int pageNumber, int pageSize, Func<T, bool> predicate, string sortColumnName, bool descending = false)
    {
        IQueryable<T> queryAble;
        var skip = (pageNumber - 1) * pageSize;

        if (predicate != null)
        {
            var local = dbSet.Where(predicate).AsQueryable();
            queryAble = OrderByDynamic(local, sortColumnName, descending);
        }
        else
        {
            queryAble = OrderByDynamic(dbSet, sortColumnName, descending);
        }

        var total = queryAble.Count();
        var resultSet = queryAble.Skip(skip).Take(pageSize);

        var paginationSet = new PaginationSet<T>()
        {
            Items = resultSet,
            TotalCount = total,
            currentPage = pageNumber
        };
        return paginationSet;
    }

    public Task<T> UpdateAsyncAgain(T entity)
    {
        throw new NotImplementedException();
    }


    // Private

    private IQueryable<T> OrderByDynamic(IQueryable<T> query, string sortColumn, bool descending)
    {
        if (string.IsNullOrEmpty(sortColumn))
        {
            sortColumn = "created_on";
        }

        var parameter = Expression.Parameter(typeof(T), "p");

        string command = "OrderBy";

        if (descending)
        {
            command = "OrderByDescending";
        }

        Expression resultExpression = null;

        var property = typeof(T).GetProperty(sortColumn);
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExpression));

        return query.Provider.CreateQuery<T>(resultExpression);
    }

}
public class PaginationSet<T>
{
    #region Properties

    public int Count
    {
        get
        {
            return (Items != null) ? Items.Count() : 0;
        }
    }

    public IEnumerable<T> Items { set; get; }
    public int? currentPage { get; set; }
    public int TotalCount { set; get; }
    #endregion Properties
}
