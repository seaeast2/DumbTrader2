using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    class MyDBContext : DbContext
    {
        private string connectionString = "server=localhost;port=3306;database=wpf_stock_test;user=seaeast2;password=123456";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString);
        }

    }
}
