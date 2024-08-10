using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.RabbitMQ
{
    public interface IRabbitMQListener
    {
        void StartListening();

        void StopListening();
    }
}
