using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Main.Common.HostDispatcher
{
    public interface IDispatcher
    {
        void Invoke(Action action);
        Task InvokeAsync(Action action);
    }
}
