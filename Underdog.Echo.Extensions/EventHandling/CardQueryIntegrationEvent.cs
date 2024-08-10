using Underdog.Echo.EventBus.Eventbus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.EventHandling
{
    public class CardQueryIntegrationEvent : IntegrationEvent
    {
        public string CardId { get; private set; }

        public CardQueryIntegrationEvent(string cardid)
            => CardId = cardid;
    }
}
