using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Underdog.Core.Mvvm;
using Underdog.Model.ViewMessageModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Underdog.Core.Navigation.Regions;
using Underdog.Common.GlobalVar;

namespace Underdog.Main.ViewModels
{
    public partial class AdminLoginViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        public AdminLoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;


            LoginCommand = new RelayCommand(async () =>
            {
                await Task.Delay(2000);
                CurrentName = "admin login success";
            });

            BackHomeCommand = new RelayCommand<string?>((string? navigatePath) =>
            {
                if (navigatePath != null)
                    _regionManager.RequestNavigate(RegionKey.Root, navigatePath);
            });
        }

        [ObservableProperty]
        private string currentName = "admin";

        [ObservableProperty]
        private bool isFingerLogin = false;

        public RelayCommand LoginCommand { get; }

        public RelayCommand<string?> BackHomeCommand { get; }

        public RelayCommand ChangeLoginModeCommand => new(() =>
        {
            IsFingerLogin = !IsFingerLogin;
        });
    }
}
