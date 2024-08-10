using CommunityToolkit.Mvvm.Input;

using Underdog.Echo.Common.GlobalVar;
using Underdog.Core.Mvvm;
using Underdog.Core.Navigation.Regions;
using Underdog.Wpf.Navigation.Regions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Main.ViewModels
{
    public partial class LockLoginViewModel:ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        public LockLoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            BackCommand = new(() => 
            {
                _regionManager.RequestNavigate(RegionKey.Root, "Home");
            });
        }

        public RelayCommand BackCommand { get; }
    }
}
