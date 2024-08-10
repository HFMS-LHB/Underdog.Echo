using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Model.ViewMessageModels
{
    public class HomeViewChangeMessage(bool isLockAdminLogin)
    {
        public bool IsLockAdminLogin { get; } = isLockAdminLogin;
    }
}
