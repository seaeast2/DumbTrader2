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
            members.Add(new Member { Number = "1", Character = "��", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "��汸", Position = "��ġ", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "2", Character = "��", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "�踻��", Position = "�Ŵ���", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "3", Character = "��", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "�����", Position = "��ġ", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "4", Character = "��", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "�밥��", Position = "������", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "5", Character = "��", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "�汸��", Position = "�Ŵ���", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "6", Character = "��", BgColor = (Brush)converter.ConvertFromString("#6741d9"), Name = "��¡��", Position = "�Ŵ���", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "7", Character = "��", BgColor = (Brush)converter.ConvertFromString("#ff6d00"), Name = "������", Position = "��ġ", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "8", Character = "��", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "���õ�", Position = "������", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "9", Character = "��", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "�϶���", Position = "�Ŵ���", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "10", Character = "��", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "����", Position = "��ġ", Email = "aa@gmail.com", Phone = "010-9997-0875" });

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
                PrintLog("���� ����");
                return;
            }

            PrintLog("���� ����");

            if (StockDataSource.IsConnected)
                PrintLog("������ Ȯ�� �Ϸ�");
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            StockDataSource.Disconnect();
            PrintLog("���� ���� ����");
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
        // �������� ��ȸ
        /*var (res, accounts) = StockDataSource.GetAccountInfos();
        if(!res)
        {
            PrintLog("���� ���� �б� ����");
        }*/

        // �������� ��ȸ
        /*XATimerQuery xATimerQuery = new XATimerQuery("t8430");

        xATimerQuery.Logger = PrintLog;

        xATimerQuery.SetInBlock((xaq) => {
            // ��ȸ ���� �Է�
            xaq.SetFieldData("t8430InBlock", "gubun", 0, "1"); // KOSPI ��ȸ
            PrintLog("��ȸ ���� �Է� �Ϸ�");
        });

        xATimerQuery.Request(true,                
            (tcode) => {
                // ��ȸ ���
                var count = xATimerQuery.GetBlockCount("t8430OutBlock");
                PrintLog(String.Format("[���� ����] {0}", count));
                //PrintLog(xATimerQuery.GetBlockData("t8430OutBlock"));

                PrintLog("-------------------------------------------------------------------------------------------------------------------------------");
                PrintLog(String.Format("| {0,20} | {1,10} | {2,15} | {3,10} | {4,10} | {5,10} | {6,10} | {7,15} | {8,10} | {9,10}", "�����", "�����ڵ�", "Ȯ���ڵ�", "ETF����", "���Ѱ�", "���Ѱ�", "���ϰ�", "�ֹ���������", "���ذ�", "����"));
                PrintLog("-------------------------------------------------------------------------------------------------------------------------------");

                // ������ ���� Ƚ�� ��ŭ �ݺ��ϸ鼭 ���� ���� ���� ��������
                for (int index = 0; index < count; ++index)
                {
                    var temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "hname", index); // �����
                    string hname = Convert.ToString(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "shcode", index);    // �����ڵ�
                    string shcode = Convert.ToString(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "expcode", index);   // Ȯ���ڵ�
                    string expcode = Convert.ToString(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "etfgubun", index);  // ETF����
                    string etfgubun = Convert.ToString(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "uplmtprice", index);// ���Ѱ�
                    int uplmtprice = Convert.ToInt32(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "dnlmtprice", index);// ���Ѱ�
                    int dnlmtprice = Convert.ToInt32(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "jnilclose", index); // ���ϰ�
                    int jnilclose = Convert.ToInt32(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "memedan", index);   // �ֹ���������
                    string memedan = Convert.ToString(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "recprice", index);  // ���ذ�
                    int recprice = Convert.ToInt32(temp_value);
                    temp_value = xATimerQuery.GetFieldData("t8430OutBlock", "gubun", index);     // ����
                    string gubun = Convert.ToString(temp_value);

                    PrintLog(String.Format("| {0,20} | {1,10} | {2,15} | {3,10} | {4,10} | {5,10} | {6,10} | {7,15} | {8,10} | {9,10}",
                        hname, shcode, expcode, etfgubun, uplmtprice, dnlmtprice, jnilclose, memedan, recprice, gubun));
                }
                PrintLog("-------------------------------------------------------------------------------------------------------------------------------");
            });*/


        // �������� ��ȸ
        /*XATimerQuery xATimerQuery = new XATimerQuery("t8430");

        PrintLog(String.Format("TR {0}: �ʴ����� {1}, �ʴ����Ѱ��� {2}, 10�д����� {3}, 10�г���û��TR���� {4}, ", 
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
