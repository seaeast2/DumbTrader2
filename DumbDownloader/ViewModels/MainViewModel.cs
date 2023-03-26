using CommunityToolkit.Mvvm.Input;
using DumbDownloader.Models;
using DumbStockAPIService.Helpers;
using DumbStockAPIService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace DumbDownloader.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        
        public override string? DisplayName { get; protected set; }

        // db 접근자
        private DbContextFactory _dbContextFactory;

        // 하단 메세지 출력
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

        // Xing API
        private XingAPIService xingApiService;


        // RelayCommands
        private RelayCommand? _connectCommand;
        private RelayCommand? _disconnectCommand;
        private RelayCommand? _loginCommand;
        private AsyncRelayCommand? _updateStocksCommand;
        private AsyncRelayCommand? _loadAccountInfoCommand;



        public MainViewModel(string? displayName, DbContextFactory dbContextFactory)
        {
            DisplayName = displayName;
            _dbContextFactory = dbContextFactory;
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

        public ICommand UpdateStocksCommand
        {
            get
            {
                if (_updateStocksCommand == null)
                    _updateStocksCommand = new AsyncRelayCommand(LoadStocks);

                return _updateStocksCommand;
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

        // 종목 로딩하여 db에 저장하기. 
        private async Task LoadStocks()
        {
            List<t8430_DTO> newStocks = new List<t8430_DTO>();

            await Task.Run(() =>
            {
                PrintLog("종목 로딩 테스트");
                // t8430 으로 데이터 읽기

                // 종목정보 조회
                XATimerQuery xATimerQuery = new XATimerQuery("t8430");

                xATimerQuery.Logger = PrintLog;

                xATimerQuery.SetInBlock((xaq) =>
                {
                    // 조회 조건 입력
                    xaq.SetFieldData("t8430InBlock", "gubun", 0, "1"); // KOSPI 조회
                    PrintLog("조회 조건 입력 완료");
                });

                xATimerQuery.Request(true,
                    (tcode) =>
                    {
                        // 조회 결과
                        var count = xATimerQuery.GetBlockCount("t8430OutBlock");
                        PrintLog(String.Format("[종목 개수] {0}", count));
                        //PrintLog(xATimerQuery.GetBlockData("t8430OutBlock"));

                        //PrintLog("-------------------------------------------------------------------------------------------------------------------------------");
                        //PrintLog(String.Format("| {0,20} | {1,10} | {2,15} | {3,10} | {4,10} | {5,10} | {6,10} | {7,15} | {8,10} | {9,10}", "종목명", "단축코드", "확장코드", "ETF구분", "상한가", "하한가", "전일가", "주문수량단위", "기준가", "구분"));
                        //PrintLog("-------------------------------------------------------------------------------------------------------------------------------");

                        // 가져온 종목 횟수 만큼 반복하면서 실제 종목 정보 가져오기
                        for (int index = 0; index < count; ++index)
                        {
                            var temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "hname", index); // 종목명
                            string hname = Convert.ToString(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "shcode", index);    // 단축코드
                            string shcode = Convert.ToString(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "expcode", index);   // 확장코드
                            string expcode = Convert.ToString(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "etfgubun", index);  // ETF구분
                            string etfgubun = Convert.ToString(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "uplmtprice", index);// 상한가
                            int uplmtprice = Convert.ToInt32(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "dnlmtprice", index);// 하한가
                            int dnlmtprice = Convert.ToInt32(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "jnilclose", index); // 전일가
                            int jnilclose = Convert.ToInt32(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "memedan", index);   // 주문수량단위
                            string memedan = Convert.ToString(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "recprice", index);  // 기준가
                            int recprice = Convert.ToInt32(temp_value);
                            temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "gubun", index);     // 구분
                            string gubun = Convert.ToString(temp_value);

                            newStocks.Add(new t8430_DTO() { Shcode = shcode, Hname = hname, Expcode = expcode, Gubun = gubun, DataCount = 0, LastUpdate = DateTime.Now});

                            PrintLog(String.Format("| {0,20} | {1,10} | {2,15} | {3,10} | {4,10} | {5,10} | {6,10} | {7,15} | {8,10} | {9,10}",
                                hname, shcode, expcode, etfgubun, uplmtprice, dnlmtprice, jnilclose, memedan, recprice, gubun));
                        }
                        //PrintLog("-------------------------------------------------------------------------------------------------------------------------------");
                    });
            });

            // 읽어들인 데이터를 DB에 저장. 만약 이미 있는 데이터면 무시
            using (MyDBContext context = _dbContextFactory.CreateDbContext())
            {
                await Task.Delay(1000);

                if (newStocks.Count > 0)
                {
                    foreach (t8430_DTO data in newStocks)
                    {
                        var found = await context.Stocks.FindAsync(data.Shcode);
                        if (found == null)
                            context.Stocks.Add(data);
                    }

                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex) 
                    {
                        PrintLog(ex.ToString());
                    }
                }

            }
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

// TODO : 왼쪽창에 종목 리스트 출력

// TODO : 종목 리스트에 체크 버튼으로 다운로드 대상 종목 표시
    // TODO : 체크버튼을 누르면 자동으로 데이터 다운로드

// TODO : Filter 기능 추가. 전체 혹은 다운로드 대상 종목만 표시 되도록

// TODO : 왼쪽에서 종목을 선택하면 오른쪽에 데이터베이스 데이터를 출력해줌.

// TODO : runtime 에 테이블 생성
//context.Database.ExecuteSqlRaw(""); // 직접 SQL문을 실행시켜서 실행한다.



