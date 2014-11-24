/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class FloatHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new FloatHandlerTestCase().RunSolo();
		}

		private Db4objects.Db4o.Internal.Handlers.FloatHandler FloatHandler()
		{
			return new Db4objects.Db4o.Internal.Handlers.FloatHandler();
		}

		public virtual void TestReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			float expected = float.MaxValue;
			FloatHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			float f = (float)FloatHandler().Read(readContext);
			Assert.AreEqual(expected, f);
		}

		public virtual void TestStoreObject()
		{
			FloatHandlerTestCase.Item storedItem = new FloatHandlerTestCase.Item(1.23456789f, 
				1.23456789f);
			DoTestStoreObject(storedItem);
		}

		public class Item
		{
			public float _float;

			public float _floatWrapper;

			public Item(float f, float wrapper)
			{
				_float = f;
				_floatWrapper = wrapper;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is FloatHandlerTestCase.Item))
				{
					return false;
				}
				FloatHandlerTestCase.Item other = (FloatHandlerTestCase.Item)obj;
				return (other._float == this._float) && this._floatWrapper.Equals(other._floatWrapper
					);
			}

			public override string ToString()
			{
				return "[" + _float + "," + _floatWrapper + "]";
			}
		}
	}
}
