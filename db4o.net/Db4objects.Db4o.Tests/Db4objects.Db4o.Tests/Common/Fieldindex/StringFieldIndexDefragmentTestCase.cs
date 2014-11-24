/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class StringFieldIndexDefragmentTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return _name;
			}
		}

		private const int ItemCount = 1000;

		// We need at least 700 items so IDs overlap with addresses. 
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(StringFieldIndexDefragmentTestCase.Item)).ObjectField("_name"
				).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Defragment();
			for (int i = 0; i < ItemCount; i++)
			{
				IQuery query = NewQuery(typeof(StringFieldIndexDefragmentTestCase.Item));
				query.Descend("_name").Constrain(i.ToString());
				IObjectSet result = query.Execute();
				Assert.AreEqual(1, result.Count);
			}
		}

		protected override void Store()
		{
			for (int i = 0; i < ItemCount; i++)
			{
				Store(new StringFieldIndexDefragmentTestCase.Item(i.ToString()));
			}
		}
	}
}
