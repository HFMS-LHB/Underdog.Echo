using CommunityToolkit.Mvvm.ComponentModel;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Underdog.Core.Mvvm;

namespace Underdog.Main.Models
{
    public partial class TestVModel:ViewModelBase
    {
        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private string cardBoxName;

        [ObservableProperty]
        private string cardBoxCode;

        [ObservableProperty]
        private int cardBoxType;

        [ObservableProperty]
        private string cardBoxDescription;

        [ObservableProperty]
        private bool isEnabled;
    }
}
