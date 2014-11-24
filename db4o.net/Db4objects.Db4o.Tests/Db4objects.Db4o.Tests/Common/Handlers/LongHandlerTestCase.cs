/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class LongHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new LongHandlerTestCase().RunSolo();
		}

		private Db4objects.Db4o.Internal.Handlers.LongHandler LongHandler()
		{
			return new Db4objects.Db4o.Internal.Handlers.LongHandler();
		}

		public virtual void TestReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			long expected = unchecked((long)(0x1020304050607080l));
			LongHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			long longValue = (long)LongHandler().Read(readContext);
			Assert.AreEqual(expected, longValue);
		}

		public virtual void TestStoreObject()
		{
			LongHandlerTestCase.Item storedItem = new LongHandlerTestCase.Item(unchecked((long
				)(0x1020304050607080l)), unchecked((long)(0x1122334455667788l)));
			DoTestStoreObject(storedItem);
		}

		public class Item
		{
			public long _long;

			public long _longWrapper;

			public Item(long l, long wrapper)
			{
				_long = l;
				_longWrapper = wrapper;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is LongHandlerTestCase.Item))
				{
					return false;
				}
				LongHandlerTestCase.Item other = (LongHandlerTestCase.Item)obj;
				return (other._long == this._long) && this._longWrapper.Equals(other._longWrapper
					);
			}

			public override string ToString()
			{
				return "[" + _long + "," + _longWrapper + "]";
			}
		}
	}
}
