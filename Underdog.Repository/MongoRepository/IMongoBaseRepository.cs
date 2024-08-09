using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Repository.MongoRepository
{
    public interface IMongoBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetAsync(int Id);
        Task<List<TEntity>> GetListAsync();
        Task<TEntity> GetByObjectIdAsync(string Id);
        Task<List<TEntity>> GetListFilterAsync(FilterDefinition<TEntity> filter);
    }
}
