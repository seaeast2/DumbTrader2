using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    // field 는 property 형식으로 정의해야 제대로 동작한다.
    [Table("t8430")]
    public class t8430_DTO
    {
        [Key]
        [Column("shcode")]
        [MaxLength(10)]
        public string Shcode { get; set; } = string.Empty; // 단축코드

        [Column("hname")]
        [MaxLength(20)]
        [Required] // NOT NULL 일 때
        public string Hname { get; set; } = string.Empty; // 종목명

        [Column("expcode")]
        [MaxLength(15)]
        public string Expcode { get; set; } = string.Empty; // 확장코드        

        [Column("gubun")]
        [MaxLength(15)]
        public string Gubun { get; set; } = string.Empty; // 구분 (1 : 코스피, 2 : 코스닥)

        [Column("datacount")]
        public int DataCount { get; set; } // 데이터 개수

        [Column("lastupdate")]
        public DateTime LastUpdate { get; set; } // 마지막 갱신일

        /* 왜래키 설정 방법
        [ForeignKey("ClassId")]
        public virtual Class? Class { get; set; }*/
    }
}
