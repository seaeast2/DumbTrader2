using CommunityToolkit.Mvvm.Input;
using DumbStockAPIService.Services;
using Microsoft.Extensions.Logging;
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


        private XingAPIService xingApiService;


        // RelayCommands
        private RelayCommand? _connectCommand;
        private RelayCommand? _disconnectCommand;
        private RelayCommand? _loginCommand;
        private AsyncRelayCommand? _loadStocksCommand;
        private AsyncRelayCommand? _loadAccountInfoCommand;



        public MainViewModel(string? displayName)
        {
            DisplayName = displayName;
            _messageLog = "";

            xingApiService = new XingAPIService();
            xingApiService.Logger = PrintLog;
        }

        #region Command
        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                    _connectCommand = new RelayCommand(Connect);

                return _connectCommand;
            }
        }


        public ICommand DisconnectCommand
        {
            get
            {
                if (_disconnectCommand == null)
                    _disconnectCommand = new RelayCommand(Disconnect);

                return _disconnectCommand;
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    //_loginCommand = new AsyncRelayCommand(Login);
                    _loginCommand = new RelayCommand(Login);

                return _loginCommand;
            }
        }

        public ICommand LoadStocksCommand
        {
            get
            {
                if (_loadStocksCommand == null)
                    _loadStocksCommand = new AsyncRelayCommand(LoadStocks);

                return _loadStocksCommand;
            }
        }

        public ICommand LoadAccountInfoCommand
        {
            get
            {
                if (_loadAccountInfoCommand == null)
                    _loadAccountInfoCommand = new AsyncRelayCommand(LoadAccountInfo);

                return _loadAccountInfoCommand;
            }
        }
        #endregion
        private void Connect()
        {
            if (!xingApiService.Connect(XingAPIService.ServerType.Test))
                return;
        }

        private void Disconnect()
        {
            xingApiService.Disconnect();
        }

        private void Login()
        {
            xingApiService.Login("seaeast2", "mytest01", false);
        }

        /*private async Task Login()
        {
            await Task.Run(() =>
            {
                xingApiService.Login("seaeast2", "mytest01", false);
            });
        }*/

        // 종목 로딩하여 db에 저장하기. 
        private async Task LoadStocks()
        {
            await Task.Run(() =>
            {
                PrintLog("종목 로딩 테스트");
                // TODO : t8430 으로 데이터 읽기
                

            });
        }

        // 종목 로딩하여 db에 저장하기. 
        private async Task LoadAccountInfo()
        {
            await Task.Run(() =>
            {
                if (!xingApiService.CheckConnAndLoginState())
                {
                    return;
                }

                PrintLog("내 Account 정보 읽어 오기");

                // 계좌정보 조회
                var (res, accounts) = xingApiService.GetAccountInfos();
                if(!res)
                {
                    PrintLog("계좌 정보 읽기 실패");
                    return;
                }
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
