using AutoMapper;

using Underdog.Model.Dtos;
using Underdog.Model.Models;
using Underdog.Main.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Main.Extensions.AutoMapper
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
