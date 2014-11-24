/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Updatedepth;

namespace Db4objects.Db4o.Tests.Common.Updatedepth
{
	public class UpdateDepthWithCascadingDeleteTestCase : AbstractDb4oTestCase
	{
		private const int ChildId = 2;

		private const int RootId = 1;

		public class Item
		{
			public UpdateDepthWithCascadingDeleteTestCase.Item _item;

			public int _id;

			public Item(int id, UpdateDepthWithCascadingDeleteTestCase.Item item)
			{
				_id = id;
				_item = item;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(UpdateDepthWithCascadingDeleteTestCase.Item)).CascadeOnDelete
				(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UpdateDepthWithCascadingDeleteTestCase.Item(RootId, new UpdateDepthWithCascadingDeleteTestCase.Item
				(ChildId, null)));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestUpdateDepth()
		{
			UpdateDepthWithCascadingDeleteTestCase.Item item = QueryItemByID(RootId);
			int changedRootID = 42;
			item._id = changedRootID;
			item._item._id = 43;
			Store(item);
			Reopen();
			UpdateDepthWithCascadingDeleteTestCase.Item changed = QueryItemByID(changedRootID
				);
			Assert.AreEqual(ChildId, changed._item._id);
		}

		private UpdateDepthWithCascadingDeleteTestCase.Item QueryItemByID(int id)
		{
			IQuery query = NewQuery(typeof(UpdateDepthWithCascadingDeleteTestCase.Item));
			query.Descend("_id").Constrain(id);
			IObjectSet result = query.Execute();
			Assert.IsTrue(result.HasNext());
			UpdateDepthWithCascadingDeleteTestCase.Item item = ((UpdateDepthWithCascadingDeleteTestCase.Item
				)result.Next());
			return item;
		}
	}
}
