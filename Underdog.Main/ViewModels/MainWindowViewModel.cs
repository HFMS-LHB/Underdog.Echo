using Castle.Components.DictionaryAdapter.Xml;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Underdog.Common.Seed;
using Underdog.Core.Mvvm;
using Underdog.Core.Navigation.Regions;
using Underdog.IServices;
using Underdog.Model.ViewMessageModels;
using Underdog.Services;
using Underdog.Wpf.Navigation.Regions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Underdog.Common.GlobalVar;
using Microsoft.Extensions.Logging;
using Underdog.Common.Caches;

namespace Underdog.ViewModels
{
    public partial class MainWindowViewModel: ViewModelBase
    {
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly ICaching _caching;
        private readonly IRegionManager _regionManager;
        private readonly ICardBoxServices _cardBoxServices;
        
        public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
                                   ICaching caching,
                                   IRegionManager regionManager,
                                   ICardBoxServices cardBoxServices)
        {
            _logger = logger;
            _caching = caching;
            _regionManager = regionManager;
            _cardBoxServices = cardBoxServices;

            // 注册命令
            AdminLoginCommand = new RelayCommand<string?>(ExecuteAdminLogin);

            WeakReferenceMessenger.Default.Register<HomeViewChangeMessage>(this, (r, m) =>
            {
                LockAdminLogin = m.IsLockAdminLogin;
            });
        }

        [ObservableProperty]
        private string title = "Logo";

        [ObservableProperty]
        private bool lockAdminLogin = false;

        public RelayCommand<string?> AdminLoginCommand { get; }

        private void ExecuteAdminLogin(string? navigatePath)
        {
            if (!LockAdminLogin && navigatePath != null)
                _regionManager.RequestNavigate(RegionKey.Root, navigatePath);
        }
    }
}
