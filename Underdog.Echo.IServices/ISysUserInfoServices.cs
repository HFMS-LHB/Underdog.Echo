using Underdog.Echo.IServices.BASE;
using Underdog.Echo.Model.Models;

namespace Underdog.Echo.IServices
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public interface ISysUserInfoServices : IBaseServices<SysUserInfo>
    {
        Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd);
    }
}
