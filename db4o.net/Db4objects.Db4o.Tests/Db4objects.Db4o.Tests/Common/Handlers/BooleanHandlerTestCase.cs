/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class BooleanHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] arguments)
		{
			new BooleanHandlerTestCase().RunSolo();
		}

		public class Item
		{
			public bool _boolWrapper;

			public bool _bool;

			public Item(bool boolWrapper, bool @bool)
			{
				_boolWrapper = boolWrapper;
				_bool = @bool;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is BooleanHandlerTestCase.Item))
				{
					return false;
				}
				BooleanHandlerTestCase.Item other = (BooleanHandlerTestCase.Item)obj;
				return (other._bool == this._bool) && this._boolWrapper.Equals(other._boolWrapper
					);
			}

			public override string ToString()
			{
				return "[" + _bool + "," + _boolWrapper + "]";
			}
		}

		private BooleanHandler BooleanHandler()
		{
			return new BooleanHandler();
		}

		public virtual void TestReadWriteTrue()
		{
			DoTestReadWrite(true);
		}

		public virtual void TestReadWriteFalse()
		{
			DoTestReadWrite(false);
		}

		public virtual void DoTestReadWrite(bool b)
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			BooleanHandler().Write(writeContext, b);
			MockReadContext readContext = new MockReadContext(writeContext);
			bool res = (bool)BooleanHandler().Read(readContext);
			Assert.AreEqual(b, res);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreObject()
		{
			BooleanHandlerTestCase.Item storedItem = new BooleanHandlerTestCase.Item(false, true
				);
			DoTestStoreObject(storedItem);
		}
	}
}
