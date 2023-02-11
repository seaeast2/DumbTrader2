using DumbDownloader.Models;
using DumbDownloader.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace DumbDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DbContextFactory? dbContextFactory { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 데이터베이스 초기화
            dbContextFactory = new DbContextFactory(DumbDownloader.Properties.Settings.Default.db_connection);
            using(MyDBContext dBContext = dbContextFactory.CreateDbContext())
            {
                dBContext.Database.Migrate();
            }

            // MainWindow, MainViewModel 설정
            MainWindow window = new MainWindow();
            var viewModel = new MainViewModel("MainViewModel");

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler? handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;

            window.Show();
        }
    }
}
