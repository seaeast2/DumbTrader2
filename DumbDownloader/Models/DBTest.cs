using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    // DTO : data transfer object
    // DTO 객체는 Database 의 스키마와 일치시켜서, DB 작업이 편하게 구성한다.
    public class DBTestDTO
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class DBTest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DBTest(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
