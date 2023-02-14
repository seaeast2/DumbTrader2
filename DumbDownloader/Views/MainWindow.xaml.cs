using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DumbDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //XingAPIService StockDataSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        /*void PrintLog(string Log = "")
        {
            MsgLog.Text += Log + "\n";
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = StockDataSource.Connect(XingAPIService.ServerType.Test);

            if (!result)
            {
                PrintLog("접속 실패");
                return;
            }

            PrintLog("접속 성공");

            if (StockDataSource.IsConnected)
                PrintLog("접속중 확인 완료");
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            StockDataSource.Disconnect();
            PrintLog("접속 해제 성공");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            StockDataSource.Login("seaeast2", "mytest01", false);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
        }*/

        //private void RequestData_Click(object sender, RoutedEventArgs e)
        //{
        // 계좌정보 조회
        /*var (res, accounts) = StockDataSource.GetAccountInfos();
        if(!res)
        {
            PrintLog("계좌 정보 읽기 실패");
        }*/

        // 종목정보 조회
        /*XATimerQuery xATimerQuery = new XATimerQuery("t8430");

        xATimerQuery.Logger = PrintLog;

        xATimerQuery.SetInBlock((xaq) => {
            // 조회 조건 입력
            xaq.SetFieldData("t8430InBlock", "gubun", 0, "1"); // KOSPI 조회
            PrintLog("조회 조건 입력 완료");
        });

        xATimerQuery.Request(true,                
            (tcode) => {
                // 조회 결과
                var count = xATimerQuery.GetBlockCount("t8430OutBlock");
                PrintLog(String.Format("[종목 개수] {0}", count));
                //PrintLog(xATimerQuery.GetBlockData("t8430OutBlock"));

                PrintLog("-------------------------------------------------------------------------------------------------------------------------------");
                PrintLog(String.Format("| {0,20} | {1,10} | {2,15} | {3,10} | {4,10} | {5,10} | {6,10} | {7,15} | {8,10} | {9,10}", "종목명", "단축코드", "확장코드", "ETF구분", "상한가", "하한가", "전일가", "주문수량단위", "기준가", "구분"));
                PrintLog("-------------------------------------------------------------------------------------------------------------------------------");

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

                    PrintLog(String.Format("| {0,20} | {1,10} | {2,15} | {3,10} | {4,10} | {5,10} | {6,10} | {7,15} | {8,10} | {9,10}",
                        hname, shcode, expcode, etfgubun, uplmtprice, dnlmtprice, jnilclose, memedan, recprice, gubun));
                }
                PrintLog("-------------------------------------------------------------------------------------------------------------------------------");
            });*/


        // 종목정보 조회
        /*XATimerQuery xATimerQuery = new XATimerQuery("t8430");

        PrintLog(String.Format("TR {0}: 초당제한 {1}, 초당제한개수 {2}, 10분당제한 {3}, 10분내요청한TR개수 {4}, ", 
            xATimerQuery.TCode,
            xATimerQuery.GetTRCountPerSec(),
            xATimerQuery.GetTRCountBaseSec(),
            xATimerQuery.GetTRCountLimit(), 
            xATimerQuery.GetTRCountRequest()
            ));*/

        //}
    }

}
