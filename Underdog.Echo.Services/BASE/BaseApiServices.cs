using Underdog.Echo.IServices.BASE;
using Underdog.Echo.Repository.BASE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Services.BASE
{
    public class BaseApiServices<TDto> : IBaseApiServices<TDto> where TDto : class, new()
    {
        public BaseApiServices(IBaseApiRepository<TDto> BaseDal = null)
        {
            this.BaseDal = BaseDal;
        }

        public IBaseApiRepository<TDto> BaseDal { get; set; }
    }
}
