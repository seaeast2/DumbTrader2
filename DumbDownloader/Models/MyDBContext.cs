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
        public DbSet<DBTestDTO> DBTests { get; set; }

        public MyDBContext(DbContextOptions options) : base(options)
        {

        }

        /*private string connectionString = "server=localhost;port=3306;database=wpf_stock_test;user=seaeast2;password=12345";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }*/
    }
}
