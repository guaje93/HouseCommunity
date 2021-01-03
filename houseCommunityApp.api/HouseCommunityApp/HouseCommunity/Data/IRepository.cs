using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        Task<TEntity> GetById(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
