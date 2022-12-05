using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DumbTestApp
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

            var converter = new BrushConverter();
            ObservableCollection<Member> members = new ObservableCollection<Member>();

            // Create DataGrid Item Info
            members.Add(new Member { Number = "1", Character = "ㄱ", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "김방구", Position = "코치", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "2", Character = "ㄱ", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "김말뚝", Position = "매니저", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "3", Character = "ㄱ", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "김빡통", Position = "코치", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "4", Character = "ㄷ", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "대갈통", Position = "관리자", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "5", Character = "ㅂ", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "방구맨", Position = "매니저", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "6", Character = "ㅇ", BgColor = (Brush)converter.ConvertFromString("#6741d9"), Name = "오징어", Position = "매니저", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "7", Character = "ㅇ", BgColor = (Brush)converter.ConvertFromString("#ff6d00"), Name = "이지랄", Position = "코치", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "8", Character = "ㅇ", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "오늘도", Position = "관리자", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "9", Character = "ㅎ", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "하랄랄", Position = "매니저", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "10", Character = "ㅈ", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "장삼봉", Position = "코치", Email = "aa@gmail.com", Phone = "010-9997-0875" });

            membersDataGrid.ItemsSource = members;
            //StockDataSource = new XingAPIService();
            //StockDataSource.Logger = PrintLog;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
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


    public class Member
    {
        public string? Character { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Brush? BgColor { get; set; }
    }
}
