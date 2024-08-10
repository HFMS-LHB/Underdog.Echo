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
    public class CardQueryIntegrationEventHandler : IIntegrationEventHandler<CardQueryIntegrationEvent>
    {
        private readonly ICardBoxServices _cardBoxServices;
        private readonly ILogger<CardQueryIntegrationEventHandler> _logger;

        public CardQueryIntegrationEventHandler(
            ICardBoxServices cardBoxServices,
            ILogger<CardQueryIntegrationEventHandler> logger)
        {
            _cardBoxServices = cardBoxServices;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(CardQueryIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Underdog", @event);

            ConsoleHelper.WriteSuccessLine($"----- Handling integration event: {@event.Id} at Underdog - ({@event})");

            await _cardBoxServices.QueryById(@event.CardId.ToString());
        }

    }
}
