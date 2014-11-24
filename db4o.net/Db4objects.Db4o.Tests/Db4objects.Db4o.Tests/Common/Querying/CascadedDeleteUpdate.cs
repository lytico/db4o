/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadedDeleteUpdate : AbstractDb4oTestCase
	{
		public class ParentItem
		{
			public object child;
		}

		public class ChildItem
		{
			public object parent1;

			public object parent2;
		}

		public static void Main(string[] arguments)
		{
			//		new CascadedDeleteUpdate().runSolo();
			new CascadedDeleteUpdate().RunNetworking();
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(CascadedDeleteUpdate.ParentItem)).CascadeOnDelete(true);
		}

		protected override void Store()
		{
			CascadedDeleteUpdate.ParentItem parentItem1 = new CascadedDeleteUpdate.ParentItem
				();
			CascadedDeleteUpdate.ParentItem parentItem2 = new CascadedDeleteUpdate.ParentItem
				();
			CascadedDeleteUpdate.ChildItem child = new CascadedDeleteUpdate.ChildItem();
			child.parent1 = parentItem1;
			child.parent2 = parentItem2;
			parentItem1.child = child;
			parentItem2.child = child;
			Db().Store(parentItem1);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestAllObjectStored()
		{
			AssertAllObjectStored();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestUpdate()
		{
			IQuery q = NewQuery(typeof(CascadedDeleteUpdate.ParentItem));
			IObjectSet objectSet = q.Execute();
			while (objectSet.HasNext())
			{
				Db().Store(objectSet.Next());
			}
			Db().Commit();
			AssertAllObjectStored();
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertAllObjectStored()
		{
			Reopen();
			IQuery q = NewQuery(typeof(CascadedDeleteUpdate.ParentItem));
			IObjectSet objectSet = q.Execute();
			while (objectSet.HasNext())
			{
				CascadedDeleteUpdate.ParentItem parentItem = (CascadedDeleteUpdate.ParentItem)objectSet
					.Next();
				Db().Refresh(parentItem, 3);
				Assert.IsNotNull(parentItem.child);
			}
		}
	}
}
