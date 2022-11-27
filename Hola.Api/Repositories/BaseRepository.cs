using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Hola.Api.Repositories
{
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

        public Task<T> UpdateAsyncAgain(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
