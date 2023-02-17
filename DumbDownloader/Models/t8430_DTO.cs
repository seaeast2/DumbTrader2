using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumbDownloader.Models
{
    // field 는 property 형식으로 정의해야 제대로 동작한다.
    public class t8430_DTO
    {
        public string Hname { get; set; } // 종목명
        [Key]
        public string Shcode { get; set; } // 단축코드
        public string Extcode { get; set; } // 확장코드        
        public string Gubun { get; set; } // 구분 (1 : 코스피, 2 : 코스닥)

        public int DataCount { get; set; } // 데이터 개수
        public DateTime LastUpdate { get; set; } // 마지막 갱신일
    }
}
