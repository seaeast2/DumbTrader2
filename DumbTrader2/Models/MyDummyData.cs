using DumbTrader2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DumbTrader2.Models
{
    // WPF 학습을 위한 Dummy Model class
    public class MyDummyData
    {
        public int Id { get; }
        public string Name { get; }

        // 시간 관련 값 정의 하는 법
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Length => EndTime.Subtract(StartTime);

        // List 필터링 하는 법
        private List<int> MyList = new List<int>();

        public MyDummyData(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // List 의 저장된 값을 조회 하는 법
        public IEnumerable<int> GetMyList(int id)
        {
            return MyList.Where(x => x == id);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is MyDummyData data &&
                   Id == data.Id &&
                   Name == data.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public void DoSomethingForException(int id)
        {
            throw new MyDummyException(id);
        }
    }
}
