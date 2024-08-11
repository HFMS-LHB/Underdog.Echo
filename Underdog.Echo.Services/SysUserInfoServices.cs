using AutoMapper;

using Underdog.Echo.Common.Attributes;
using Underdog.Echo.IServices;
using Underdog.Echo.Model.Models;
using Underdog.Echo.Services.BASE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Underdog.Echo.Repository.BASE;

namespace Underdog.Echo.Services
{
    internal class SysUserInfoServices : BaseServices<SysUserInfo>, ISysUserInfoServices
    {
        IMapper _mapper;
        public SysUserInfoServices(IMapper mapper)
        {
            this._mapper = mapper;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd)
        {
            SysUserInfo sysUserInfo = new SysUserInfo(loginName, loginPwd);
            SysUserInfo model = new SysUserInfo();
            var userList = await base.Query(a => a.LoginName == sysUserInfo.LoginName && a.LoginPWD == sysUserInfo.LoginPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(sysUserInfo);
                model = await base.QueryById(id);
            }
            return model;
        }
    }
}
