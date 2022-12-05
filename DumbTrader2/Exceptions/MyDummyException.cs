using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DumbTrader2.Exceptions
{
    // Exception 처리 하는법을 위한 더미 class
    public class MyDummyException : Exception
    {
        public int DummyId { get; }

        public MyDummyException(int dummyId)
        {
            DummyId = dummyId;
        }

        public MyDummyException(string? message, int dummyId) : base(message)
        {
            DummyId = dummyId;
        }

        public MyDummyException(string? message, Exception? innerException, int dummyId) : base(message, innerException)
        {
            DummyId = dummyId;
        }

        protected MyDummyException(SerializationInfo info, StreamingContext context, int dummyId) : base(info, context)
        {
            DummyId = dummyId;
        }
    }
}
