/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class IndexedBlockSizeQueryTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new IndexedBlockSizeQueryTestCase().RunNetworking();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.BlockSize(10);
			config.ObjectClass(typeof(IndexedBlockSizeQueryTestCase.Item)).ObjectField("_name"
				).Indexed(true);
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				// public Object _untypedMember;
				// _untypedMember = name;
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new IndexedBlockSizeQueryTestCase.Item("one"));
		}

		public virtual void Test()
		{
			IQuery q = NewQuery(typeof(IndexedBlockSizeQueryTestCase.Item));
			q.Descend("_name").Constrain("one");
			Assert.AreEqual(1, q.Execute().Count);
		}
	}
}
