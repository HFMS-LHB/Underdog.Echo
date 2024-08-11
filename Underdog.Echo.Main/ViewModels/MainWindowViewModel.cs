using Castle.Components.DictionaryAdapter.Xml;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Underdog.Echo.Common.Seed;
using Underdog.Core.Mvvm;
using Underdog.Core.Navigation.Regions;
using Underdog.Echo.IServices;
using Underdog.Echo.Model.ViewMessageModels;
using Underdog.Echo.Services;
using Underdog.Wpf.Navigation.Regions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Underdog.Echo.Common.GlobalVar;
using Microsoft.Extensions.Logging;
using Underdog.Echo.Common.Caches;
using Underdog.Echo.Common.Caches.Interface;
using HandyControl.Controls;
using HandyControl.Data;

namespace Underdog.Echo.Main.ViewModels
{
    public partial class MainWindowViewModel: ViewModelBase
    {
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly ICaching _caching;
        private readonly IRegionManager _regionManager;
        private readonly ISysUserInfoServices _sysUserInfoServices;
        
        public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
                                   ICaching caching,
                                   IRegionManager regionManager,
                                   ISysUserInfoServices sysUserInfoServices)
        {
            _logger = logger;
            _caching = caching;
            _regionManager = regionManager;
            _sysUserInfoServices = sysUserInfoServices;

            WeakReferenceMessenger.Default.Register<HomeViewChangeMessage>(this, (r, m) =>
            {
                // 消息机制
            });
        }

        [ObservableProperty]
        private string title = "Hello Underdog.Echo";


        public RelayCommand<FunctionEventArgs<object>> SwitchItemCmd => new(SwitchItem);

        private void SwitchItem(FunctionEventArgs<object> info) => Growl.Info((info.Info as SideMenuItem)?.Header.ToString());

        public RelayCommand<string> SelectCmd => new(Select);

        private void Select(string header) => Growl.Success(header);
    }
}
