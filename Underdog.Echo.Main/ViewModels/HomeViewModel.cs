using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Underdog.Echo.Common.Caches;
using Underdog.Core.Mvvm;
using Underdog.Echo.IServices;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Underdog.Wpf.Navigation.Regions;
using Underdog.Core.Navigation.Regions;
using Underdog.Echo.Common.GlobalVar;
using CommunityToolkit.Mvvm.Messaging;
using Underdog.Echo.Model.ViewMessageModels;
using Underdog.Echo.Common.Caches.Interface;

namespace Underdog.Echo.Main.ViewModels
{
    public partial class HomeViewModel : ViewModelBase, INavigationAware
    {
        private readonly ILogger<HomeViewModel> _logger;
        private readonly ICaching _caching;
        private readonly IRegionManager _regionManager;
        private readonly ISysUserInfoServices _sysUserInfoServices;
        public HomeViewModel(ILogger<HomeViewModel> logger,
                             ICaching caching,
                             IRegionManager regionManager,
                             ISysUserInfoServices sysUserInfoServices)
        {
            _logger = logger;
            _caching = caching;
            _regionManager = regionManager;
            _sysUserInfoServices = sysUserInfoServices;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 导航进入此页面执行
            // 使用消息传递
            WeakReferenceMessenger.Default.Send(new HomeViewChangeMessage(false));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // 是否复用实例，返回true表示复用此实例，返回false表示不复用此实例，如果所有当前页面的实例都不能复用，则创建此页面的新实例
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // 导航离开此页面执行
            WeakReferenceMessenger.Default.Send(new HomeViewChangeMessage(true));
        }
    }
}
