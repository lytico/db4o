/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeOnDeleteTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string item;
		}

		public class Holder
		{
			public CascadeOnDeleteTestCase.Item[] items;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestNoAccidentalDeletes()
		{
			AssertNoAccidentalDeletes(true, true);
			AssertNoAccidentalDeletes(true, false);
			AssertNoAccidentalDeletes(false, true);
			AssertNoAccidentalDeletes(false, false);
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertNoAccidentalDeletes(bool cascadeOnUpdate, bool cascadeOnDelete
			)
		{
			DeleteAll(typeof(CascadeOnDeleteTestCase.Holder));
			DeleteAll(typeof(CascadeOnDeleteTestCase.Item));
			IObjectClass oc = Fixture().Config().ObjectClass(typeof(CascadeOnDeleteTestCase.Holder
				));
			oc.CascadeOnDelete(cascadeOnDelete);
			oc.CascadeOnUpdate(cascadeOnUpdate);
			Reopen();
			CascadeOnDeleteTestCase.Item item = new CascadeOnDeleteTestCase.Item();
			CascadeOnDeleteTestCase.Holder holder = new CascadeOnDeleteTestCase.Holder();
			holder.items = new CascadeOnDeleteTestCase.Item[] { item };
			Db().Store(holder);
			Db().Commit();
			holder.items[0].item = "abrakadabra";
			Db().Store(holder);
			if (!cascadeOnDelete && !cascadeOnUpdate)
			{
				// the only case, where we don't cascade
				Db().Store(holder.items[0]);
			}
			Assert.AreEqual(1, CountOccurences(typeof(CascadeOnDeleteTestCase.Item)));
			Db().Commit();
			Assert.AreEqual(1, CountOccurences(typeof(CascadeOnDeleteTestCase.Item)));
		}
	}
}
