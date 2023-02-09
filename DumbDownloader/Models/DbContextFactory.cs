using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    public class DbContextFactory
    {
        private readonly string connectionString_;

        public DbContextFactory(string connectionString)
        {
            this.connectionString_ = connectionString;
        }

        public MyDBContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseMySql(connectionString_, ServerVersion.AutoDetect(connectionString_)).Options;

            return new MyDBContext(options);
        }
    }
}
