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
using Underdog.Wpf.Navigation.Regions;
using Underdog.Core.Navigation.Regions;
using Underdog.Common.GlobalVar;
using CommunityToolkit.Mvvm.Messaging;
using Underdog.Model.ViewMessageModels;
using Underdog.Common.Caches.Interface;

namespace Underdog.Main.ViewModels
{
    public partial class HomeViewModel : ViewModelBase, INavigationAware
    {
        private readonly ILogger<AdminHomeViewModel> _logger;
        private readonly ICaching _caching;
        private readonly IRegionManager _regionManager;
        private readonly ICardBoxServices _cardBoxServices;
        public HomeViewModel(ILogger<AdminHomeViewModel> logger,
                             ICaching caching,
                             IRegionManager regionManager,
                             ICardBoxServices cardBoxServices)
        {
            _logger = logger;
            _caching = caching;
            _regionManager = regionManager;
            _cardBoxServices = cardBoxServices;

            Choose1Or2Command = new RelayCommand(() =>
            {
                IsFirstGrid = !IsFirstGrid;
            });

            Click1Command = new(() =>
            {
                _regionManager.RequestNavigate(RegionKey.Root, "LockLogin");
                _logger.LogDebug(_caching.GetString("access_token"));
            });

            Click2Command = new(() =>
            {
                _regionManager.RequestNavigate(RegionKey.Root, "LockLogin");
                _logger.LogDebug(_caching.GetString("access_token"));
            });

            _caching.SetString("access_token", "测试缓存");
        }

        [ObservableProperty]
        private bool isFirstGrid = true;

        public RelayCommand Choose1Or2Command { get; }

        public RelayCommand Click1Command { get; }

        public RelayCommand Click2Command { get; }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 导航进入此页面执行
            // 使用消息传递
            WeakReferenceMessenger.Default.Send(new HomeViewChangeMessage(false));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // 导航离开此页面执行
            WeakReferenceMessenger.Default.Send(new HomeViewChangeMessage(true));
        }
    }
}
