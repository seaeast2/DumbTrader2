using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DumbDownloader.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        public override string? DisplayName { get; protected set; }

        //private RelayCommand _loginCommand;
        private AsyncRelayCommand _loginCommand;

        public MainViewModel(string? displayName)
        {
            DisplayName = displayName;
        }

        #region Command
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new AsyncRelayCommand(Login);

                return _loginCommand;
            }
        }
        #endregion

        private async Task Login()
        {
            await Task.Run(() =>
            {
                var dialogResult = MessageBox.Show("Message", "Title", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.OK)
                    MessageBox.Show("OK Clicked");
                else
                    MessageBox.Show("Cancel Clicked");
            });
        }
    }
}
