using AutoMapper;

using Underdog.Echo.Model.Dtos;
using Underdog.Echo.Model.Models;
using Underdog.Echo.Main.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Main.Extensions.AutoMapper
{
    public class VModelProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public VModelProfile()
        {
            CreateMap<BoxDto, TestVModel>();
        }
    }
}
