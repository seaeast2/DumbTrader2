using DumbDownloader.Models;
using Microsoft.EntityFrameworkCore;
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
            dbContextFactory = new DbContextFactory(DumbDownloader.Properties.Settings.Default.db_connection);
            using(MyDBContext dBContext = dbContextFactory.CreateDbContext())
            {
                dBContext.Database.Migrate();
            }

            /*MainWindow = new MainWindow()
            {
                DataContext = new MainView
            }*/

            base.OnStartup(e);
        }
    }
}
