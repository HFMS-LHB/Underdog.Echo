using Underdog.Echo.Common.Helper.Console;
using Underdog.Echo.EventBus.Eventbus;
using Underdog.Echo.IServices;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.EventHandling
{
    public class SysUserQueryIntegrationEventHandler : IIntegrationEventHandler<SysUserQueryIntegrationEvent>
    {
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly ILogger<SysUserQueryIntegrationEventHandler> _logger;

        public SysUserQueryIntegrationEventHandler(
            ISysUserInfoServices sysUserInfoServices,
            ILogger<SysUserQueryIntegrationEventHandler> logger)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(SysUserQueryIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Underdog", @event);

            ConsoleHelper.WriteSuccessLine($"----- Handling integration event: {@event.Id} at Underdog - ({@event})");

            await _sysUserInfoServices.QueryById(@event.LoginName.ToString());
        }

    }
}
