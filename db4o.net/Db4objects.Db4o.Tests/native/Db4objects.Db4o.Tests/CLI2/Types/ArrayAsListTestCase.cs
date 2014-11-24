/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Types
{
	public class ArrayAsListTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public IList _list;

			public Item(IList list)
			{
				_list = list;
			}

			public IList List
			{
				get { return _list;  }
			}
		}

		static readonly string[] Elements = new string[] { "foo", "bar" };

		protected override void Store()
		{	
			Store(new Item(Elements));
		}

		public void Test()
		{
			Item item = RetrieveOnlyInstance<Item>();
			ArrayAssert.AreEqual(Elements, (string[])item.List);
		}
	}
}
