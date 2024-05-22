using Underdog.Model;
using Underdog.Repository.UnitOfWorks;

using SqlSugar;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Repository.BASE
{
    public class BaseApiRepository<TDto> : IBaseApiRepository<TDto> where TDto : class, new()
    {
        public BaseApiRepository()
        {

        }

        public Task DeleteAsync(string endpoint, int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDto>> GetAllAsync(string endpoint)
        {
            throw new NotImplementedException();
        }

        public Task GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TDto> GetAsync(string endpoint)
        {
            throw new NotImplementedException();
        }

        public Task<TDto> PostAsync(string endpoint, TDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<TDto> PutAsync(string endpoint, TDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
