using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Tests.CLI1.Soda
{
	public class CoerceUnsignedTypesTestUnit : AbstractDb4oTestCase
	{
		protected override void Store()
		{
			Store(TestVariables.Current.NewMinValue());
			Store(TestVariables.Current.NewMaxValue());
		}

		public void TestSimple()
		{
			var result = RunGreaterThanQuery(0);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(TestVariables.Current.NewMaxValue(), result[0], TestVariables.Current.TypeName);
		}		
		
		public void TestRangeError()
		{
			if (TestVariables.Current.SupportsRangeErrorTests)
			{
				Assert.Expect<OverflowException>(() => RunGreaterThanQuery(TestVariables.Current.InvalidValue));
			}
			else
			{
				TestPlatform.EmitWarning("Test does not suport range error checking: skipped");
			}
		}

		public void TestUnsignedRange()
		{
			AssertEqualityInRange(TestVariables.Current.MinValue);
			AssertEqualityInRange(TestVariables.Current.MaxValue);
		}

		private void AssertEqualityInRange(object toBeTested)
		{
			var result = RunQuery(toBeTested);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(TestVariables.Current.NewInstance(toBeTested), result[0]);
		}

		public void TestInvalidType()
		{
			Assert.Expect<Db4oException>(() => RunGreaterThanQuery("string's cannot be cast to unsigned types"));
		}

		public void TestInvalidCustomType()
		{
			Assert.Expect<Db4oException>(() => RunGreaterThanQuery(new CustomTypeForQuery()));
		}

		public void TestValidCustomTypeDoesNotThrow()
		{
			RunGreaterThanQuery(new ConvertibleCustomTypeThat());
		}
		
		private IObjectSet RunGreaterThanQuery(object value)
		{
			var query = NewQuery();

			query.Descend("_value").Constrain(value).Greater();
			return query.Execute();
		}
		
		private IObjectSet RunQuery(object value)
		{
			var query = NewQuery();
			query.Descend("_value").Constrain(value);
			return query.Execute();
		}
	}

	public class ConvertibleCustomTypeThat : IConvertible
	{
		public TypeCode GetTypeCode()
		{
			throw new NotImplementedException();
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			return false;
		}

		public char ToChar(IFormatProvider provider)
		{
			return 'A';
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			return 42;
		}

		public byte ToByte(IFormatProvider provider)
		{
			return 42;
		}

		public short ToInt16(IFormatProvider provider)
		{
			return 42;
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			return 42;
		}

		public int ToInt32(IFormatProvider provider)
		{
			return 42;
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			return 42;
		}

		public long ToInt64(IFormatProvider provider)
		{
			return 42;
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			return 42;
		}

		public float ToSingle(IFormatProvider provider)
		{
			return 42;
		}

		public double ToDouble(IFormatProvider provider)
		{
			return 42;
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			return 42;
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			return new DateTime(1971, 5, 1);
		}

		public string ToString(IFormatProvider provider)
		{
			return "42";
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new NotImplementedException();
		}
	}

	public class CustomTypeForQuery
	{
	}
}
