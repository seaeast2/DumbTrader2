using DumbDownloader.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DumbDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {


            /*string connectionString = "server=localhost;port=3306;database=wpf_stock_test;user=seaeast2;password=123456";
            DbContextOptions options = new DbContextOptionsBuilder().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
            MyDBContext dbContext = new MyDBContext(options);

            dbContext.Database.Migrate();*/

            /*MainWindow = new MainWindow()
            {
                DataContext = new MainView
            }*/

            base.OnStartup(e);
        }
    }
}
