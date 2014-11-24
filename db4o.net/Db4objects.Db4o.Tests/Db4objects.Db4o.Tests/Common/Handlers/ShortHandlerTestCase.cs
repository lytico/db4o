/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class ShortHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new ShortHandlerTestCase().RunSolo();
		}

		public class Item
		{
			public short _short;

			public short _shortWrapper;

			public Item(short s, short wrapper)
			{
				_short = s;
				_shortWrapper = wrapper;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is ShortHandlerTestCase.Item))
				{
					return false;
				}
				ShortHandlerTestCase.Item other = (ShortHandlerTestCase.Item)obj;
				return (other._short == this._short) && this._shortWrapper.Equals(other._shortWrapper
					);
			}

			public override string ToString()
			{
				return "[" + _short + "," + _shortWrapper + "]";
			}
		}

		private ShortHandler ShortHandler()
		{
			return new ShortHandler();
		}

		public virtual void TestReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			short expected = (short)unchecked((int)(0x1020));
			ShortHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			short shortValue = (short)ShortHandler().Read(readContext);
			Assert.AreEqual(expected, shortValue);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreObject()
		{
			ShortHandlerTestCase.Item storedItem = new ShortHandlerTestCase.Item((short)unchecked(
				(int)(0x1020)), (short)unchecked((int)(0x1122)));
			DoTestStoreObject(storedItem);
		}
	}
}
