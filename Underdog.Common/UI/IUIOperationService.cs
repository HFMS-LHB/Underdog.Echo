using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Common.UI
{
    public interface IUIOperationService
    {
        void LockUI();

        void UnlockUI();
    }
}
