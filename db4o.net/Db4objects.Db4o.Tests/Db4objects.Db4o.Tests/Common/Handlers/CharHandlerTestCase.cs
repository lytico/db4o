/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class CharHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new CharHandlerTestCase().RunSolo();
		}

		public class Item
		{
			public char _char;

			public char _charWrapper;

			public Item(char c, char wrapper)
			{
				_char = c;
				_charWrapper = wrapper;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is CharHandlerTestCase.Item))
				{
					return false;
				}
				CharHandlerTestCase.Item other = (CharHandlerTestCase.Item)obj;
				return (other._char == this._char) && this._charWrapper.Equals(other._charWrapper
					);
			}

			public override string ToString()
			{
				return "[" + _char + "," + _charWrapper + "]";
			}
		}

		private CharHandler CharHandler()
		{
			return new CharHandler();
		}

		public virtual void TestReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			char expected = (char)unchecked((int)(0x4e2d));
			CharHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			char charValue = (char)CharHandler().Read(readContext);
			Assert.AreEqual(expected, charValue);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreObject()
		{
			CharHandlerTestCase.Item storedItem = new CharHandlerTestCase.Item((char)unchecked(
				(int)(0x4e2f)), (char)unchecked((int)(0x4e2d)));
			DoTestStoreObject(storedItem);
		}
	}
}
