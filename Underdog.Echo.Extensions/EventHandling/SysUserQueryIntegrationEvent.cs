using Underdog.Echo.EventBus.Eventbus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.EventHandling
{
    public class SysUserQueryIntegrationEvent : IntegrationEvent
    {
        public string LoginName { get; private set; }

        public SysUserQueryIntegrationEvent(string loginName)
            => LoginName = loginName;
    }
}
