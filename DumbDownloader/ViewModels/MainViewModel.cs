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

        private int _messageLineCounter = 0;
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
        private AsyncRelayCommand? _loginCommand;
        private AsyncRelayCommand? _disconnectCommand;
        


        public MainViewModel(string? displayName)
        {
            DisplayName = displayName;
            _messageLog = "";

            StockDataSource = new XingAPIService();
        }

        #region Command
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new AsyncRelayCommand(Login);
                    //_loginCommand = new RelayCommand(Login);

                return _loginCommand;
            }
        }

        public ICommand DisconnectCommand
        {
            get
            {
                if (_disconnectCommand == null)
                    _disconnectCommand = new AsyncRelayCommand(Disconnect);

                return _disconnectCommand;
            }
        }
        #endregion

        private async Task Login()
        {
            await Task.Run(() =>
            {
                bool result = StockDataSource.Connect(XingAPIService.ServerType.Test);

                if (!result)
                {
                    PrintLog("접속 실패");
                    return;
                }

                PrintLog("접속 성공");

                if (!StockDataSource.IsConnected)
                    PrintLog("접속 실패");

                PrintLog("로그인 시도");
                StockDataSource.Login("seaeast2", "mytest01", false);
                PrintLog("로그인 성공");
            });
        }

        private async Task Disconnect()
        {
            await Task.Run(() =>
            {
                StockDataSource.Disconnect();
                PrintLog("접속 해제 성공");
            });
        }

        public void PrintLog(string Log)
        {
            MessageLog += Log + "\n";
            _messageLineCounter++;
            if (_messageLineCounter > 100)
            {
                MessageLog = Log;
                _messageLineCounter = 1;
            }
        }
    }
}
