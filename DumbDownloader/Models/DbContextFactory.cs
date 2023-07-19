using Microsoft.EntityFrameworkCore;
using System;

namespace DumbDownloader.Models
{
    public class DbContextFactory
    {
        private readonly string connectionString_;

        public DbContextFactory(string connectionString)
        {
            this.connectionString_ = connectionString;
        }

        public MyDBContext? CreateDbContext()
        {
            DbContextOptions options;
            try
            {
                options = new DbContextOptionsBuilder().UseMySql(
                    connectionString_, ServerVersion.AutoDetect(connectionString_)).Options;
            }
            catch (MySqlConnector.MySqlException)
            {
                return null;
            }

            return new MyDBContext(options);
        }
    }
}


