using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    /*
    db 생성 명령어
    
    PM> add-migration Initial -verbose

    PM> update-database -verbose
     */
    public class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDBContext>
    {
        public MyDBContext CreateDbContext(string[] args)
        {
            string connectionString = "server=localhost;port=3306;database=wpf_stock_test;user=seaeast2;password=123456";
            DbContextOptions options = new DbContextOptionsBuilder().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
            return new MyDBContext(options);
        }
    }
}
