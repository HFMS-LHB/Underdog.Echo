using AutoMapper;

using Underdog.Common.Helper;
using Underdog.IServices.BASE;
using Underdog.IServices.External;
using Underdog.Model.Models;
using Underdog.Services.BASE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Services.External
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
