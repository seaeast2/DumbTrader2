using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    public class MyDBContext : DbContext
    {
        // database 에 접근 객체
        //public DbSet<DBTestDTO> StockDaily { get; set; }

        // 주식 종목
        public DbSet<t8430_DTO> Stocks { get; set; }

        public MyDBContext(DbContextOptions options) : base(options) { }




        /* 
        // MySQL or Mariadb 초기화 문자열
        private string connectionString = "server=localhost;port=3306;database=wpf_stock_test;user=seaeast2;password=123456";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }*/
    }
}
