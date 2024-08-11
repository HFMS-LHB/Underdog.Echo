using CommunityToolkit.Mvvm.ComponentModel;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Underdog.Core.Mvvm;

namespace Underdog.Echo.Main.Models
{
    public partial class TestVModel:ViewModelBase
    {
        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private string loginName;

        [ObservableProperty]
        private string realName;

        [ObservableProperty]
        private int status;

        [ObservableProperty]
        private string departmentId;
    }
}
