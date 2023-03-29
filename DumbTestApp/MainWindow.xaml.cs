
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DumbTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var converter = new BrushConverter();
            ObservableCollection<Member> members = new ObservableCollection<Member>();

            // Create DataGrid Item Info
            members.Add(new Member { Number = "1", Character = "¤¡", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "±è¹æ±¸", Position = "ÄÚÄ¡", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "2", Character = "¤¡", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "±è¸»¶Ò", Position = "¸Å´ÏÀú", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "3", Character = "¤¡", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "±èºýÅë", Position = "ÄÚÄ¡", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "4", Character = "¤§", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "´ë°¥Åë", Position = "°ü¸®ÀÚ", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "5", Character = "¤²", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "¹æ±¸¸Ç", Position = "¸Å´ÏÀú", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "6", Character = "¤·", BgColor = (Brush)converter.ConvertFromString("#6741d9"), Name = "¿ÀÂ¡¾î", Position = "¸Å´ÏÀú", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "7", Character = "¤·", BgColor = (Brush)converter.ConvertFromString("#ff6d00"), Name = "ÀÌÁö¶ö", Position = "ÄÚÄ¡", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "8", Character = "¤·", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "¿À´Ãµµ", Position = "°ü¸®ÀÚ", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "9", Character = "¤¾", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "ÇÏ¶ö¶ö", Position = "¸Å´ÏÀú", Email = "aa@gmail.com", Phone = "010-9997-0875" });
            members.Add(new Member { Number = "10", Character = "¤¸", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Àå»ïºÀ", Position = "ÄÚÄ¡", Email = "aa@gmail.com", Phone = "010-9997-0875" });

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
