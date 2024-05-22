using Underdog.Model;
using SqlSugar;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Repository.BASE
{
    public interface IBaseApiRepository<TDto> where TDto : class
    {
        Task GetAsync(int id);
        Task<TDto> GetAsync(string endpoint);
        Task<IEnumerable<TDto>> GetAllAsync(string endpoint);
        Task<TDto> PostAsync(string endpoint, TDto dto);
        Task<TDto> PutAsync(string endpoint, TDto dto);
        Task DeleteAsync(string endpoint, int id);
    }
}
