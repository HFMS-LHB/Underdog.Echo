using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Underdog.Common.Caches;
using Underdog.Core.Mvvm;
using Underdog.IServices;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Underdog.Common.Caches.Interface;

namespace Underdog.Main.ViewModels
{
    public partial class AdminHomeViewModel : ViewModelBase
    {
        private readonly ILogger<AdminHomeViewModel> _logger;
        private readonly ICaching _caching;
        private readonly ICardBoxServices _cardBoxServices;
        public AdminHomeViewModel(ILogger<AdminHomeViewModel> logger,
                             ICaching caching,
                             ICardBoxServices cardBoxServices)
        {
            _logger = logger;
            _caching = caching;
            _cardBoxServices = cardBoxServices;

            _caching.SetString("access_token", "23456");
        }
    }
}
