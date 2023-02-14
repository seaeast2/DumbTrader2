using CommunityToolkit.Mvvm.Input;
using DumbStockAPIService.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DumbDownloader.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        #region 프로퍼티들
        public override string? DisplayName { get; protected set; }

        private string _messageLog;
        public string MessageLog
        {
            get
            {
                return _messageLog;
            }
            set
            {
                if (string.Equals(value, _messageLog))
                    return;
                _messageLog = value;
                OnPropertyChanged("MessageLog");
            }
        }
        #endregion


        private XingAPIService StockDataSource;


        // RelayCommands
        //private RelayCommand _loginCommand;
        private AsyncRelayCommand? _loginCommand;


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
            PrintLog("테스트");

            /*bool result = StockDataSource.Connect(XingAPIService.ServerType.Test);

            if (!result)
            {
                PrintLog("접속 실패");
                return;
            }

            PrintLog("접속 성공");

            if (StockDataSource.IsConnected)
                PrintLog("접속중 확인 완료");*/


            /*await Task.Run(() =>
            {
                var dialogResult = MessageBox.Show("Message", "Title", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.OK)
                    MessageBox.Show("OK Clicked");
                else
                    MessageBox.Show("Cancel Clicked");
            });*/
        }

        void PrintLog(string Log = "")
        {
            MessageLog += Log + "\n";
        }
    }
}
