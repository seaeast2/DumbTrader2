using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public override string? DisplayName { get; protected set; }

        public MainViewModel(string? displayName)
        {
            DisplayName = displayName;
        }
    }
}
