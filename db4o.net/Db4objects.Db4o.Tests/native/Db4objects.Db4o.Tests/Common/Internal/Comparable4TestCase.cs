using System;
using Db4objects.Db4o.Internal.Handlers;

namespace Db4objects.Db4o.Tests.Common.Internal
{
    public partial class Comparable4TestCase
    {
        public void TestDecimalHandler()
        {
            AssertHandlerComparison(typeof(DecimalHandler), ((decimal)0), ((decimal) 1));
            AssertHandlerComparison(typeof(DecimalHandler), ((decimal)-1), ((decimal) 0));
            AssertHandlerComparison(typeof(DecimalHandler), Decimal.MinValue, Decimal.MaxValue);
        }

        public void TestSByteHandler()
        {
            AssertHandlerComparison(typeof(SByteHandler), ((sbyte)0), ((sbyte)1));
            AssertHandlerComparison(typeof(SByteHandler), ((sbyte)-1), ((sbyte)0));
            AssertHandlerComparison(typeof(SByteHandler), SByte.MinValue, SByte.MaxValue);
        }
        
        public void TestULongHandler()
        {
            AssertHandlerComparison(typeof(ULongHandler), ((ulong)0), ((ulong)1));
            AssertHandlerComparison(typeof(ULongHandler), UInt64.MinValue, UInt64.MaxValue);
        }

        public void TestUIntHandler()
        {
            AssertHandlerComparison(typeof(UIntHandler), ((uint)0), ((uint)1));
            AssertHandlerComparison(typeof(UIntHandler), UInt32.MinValue, UInt32.MaxValue);
        }
        
        public void TestUShortHandler()
        {
            AssertHandlerComparison(typeof(UShortHandler), ((ushort)0), ((ushort)1));
            AssertHandlerComparison(typeof(UShortHandler), UInt16.MinValue, UInt16.MaxValue);
        }

        public void TestDateTimeHandler()
        {
            AssertHandlerComparison(typeof(DateTimeHandler), new DateTime(2007, 12, 18), new DateTime(2007, 12, 19));
            AssertHandlerComparison(typeof(DateTimeHandler), DateTime.MinValue, DateTime.MaxValue);
        }
    }
}
