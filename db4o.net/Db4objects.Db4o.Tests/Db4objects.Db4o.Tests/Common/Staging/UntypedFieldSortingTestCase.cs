/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	/// <summary>#COR-1790</summary>
	public class UntypedFieldSortingTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public Item(object id)
			{
				_id = id;
			}

			public object _id;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UntypedFieldSortingTestCase.Item(2));
			Store(new UntypedFieldSortingTestCase.Item(3));
			Store(new UntypedFieldSortingTestCase.Item(1));
		}

		public virtual void Test()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(UntypedFieldSortingTestCase.Item));
			query.Descend("_id").OrderAscending();
			IObjectSet objectSet = query.Execute();
			int lastId = 0;
			while (objectSet.HasNext())
			{
				UntypedFieldSortingTestCase.Item item = ((UntypedFieldSortingTestCase.Item)objectSet
					.Next());
				int currentId = ((int)item._id);
				Assert.IsGreater(lastId, currentId);
				currentId = lastId;
			}
		}
	}
}
