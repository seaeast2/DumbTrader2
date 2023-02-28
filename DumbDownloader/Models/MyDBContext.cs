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
        private string stockTableName_;

        // 주식 종목
        public DbSet<t8430_DTO> Stocks { get; set; }

        public MyDBContext(DbContextOptions options) : base(options) { }


        /* https://youtu.be/iNTnLqXf_Ik 참고
         * db table 선택
         * protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<dailystock>().ToTable($"{stockTableName}");
        }*/



        /* 
        // MySQL or Mariadb 초기화 문자열
        private string connectionString = "server=localhost;port=3306;database=wpf_stock_test;user=seaeast2;password=123456";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }*/
    }
}


/*public async Task CreateReservation(Reservation reservation)
{
    // DBContext 객체를 잠깐만 사용하고 버리는 것에 주목.
    using (ReservoomDbContext context = _dbContextFactory.CreateDbContext())
    {
        await Task.Delay(3000);

        ReservationDTO reservationDTO = ToReservationDTO(reservation);

        context.Reservations.Add(reservationDTO);
        await context.SaveChangesAsync();
    }
}*/


/*public async Task<IEnumerable<Reservation>> GetAllReservations()
{
    using (ReservoomDbContext context = _dbContextFactory.CreateDbContext())
    {
        await Task.Delay(3000);

        IEnumerable<ReservationDTO> reservationDTOs = await context.Reservations.ToListAsync();

        return reservationDTOs.Select(r => ToReservation(r));
    }
}*/