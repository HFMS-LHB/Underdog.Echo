using AutoMapper;

using Underdog.Echo.Common.Helper;
using Underdog.Echo.IServices.BASE;
using Underdog.Echo.IServices.External;
using Underdog.Echo.Model.Models;
using Underdog.Echo.Services.BASE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Services.External
{
    public class LoginApiServices: BaseApiServices<CardBox>, ILoginApiServices
    {
        IMapper _mapper;
        public LoginApiServices(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public Task Login() 
        {
            return Task.CompletedTask;
        }
    }
}
