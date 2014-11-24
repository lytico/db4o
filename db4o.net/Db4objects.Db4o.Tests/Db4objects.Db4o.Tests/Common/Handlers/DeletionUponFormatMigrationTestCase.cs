/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class DeletionUponFormatMigrationTestCase : FormatMigrationTestCaseBase
	{
		private const int ItemsToKeepCount = 3;

		private static readonly string ChildToBeKept = "bar";

		private static readonly string ChildToBeDelete = "foo";

		private const int IdToBeDeleted = 42;

		private const int IdToBeKept = unchecked((int)(0xdb40));

		public class Item
		{
			public Item(int id, DeletionUponFormatMigrationTestCase.ChildItem child, DeletionUponFormatMigrationTestCase.Item
				[] items) : this(id)
			{
				_child = child;
				_array = items;
			}

			public Item(int id)
			{
				_id = id;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (!obj.GetType().Equals(typeof(DeletionUponFormatMigrationTestCase.Item)))
				{
					return false;
				}
				DeletionUponFormatMigrationTestCase.Item other = (DeletionUponFormatMigrationTestCase.Item
					)obj;
				return other._id == _id;
			}

			public DeletionUponFormatMigrationTestCase.Item[] _array;

			public object _child;

			public int _id;
		}

		public class ChildItem
		{
			public ChildItem(string name)
			{
				_name = name;
			}

			public string _name;
		}

		protected override void ConfigureForTest(IConfiguration config)
		{
			config.ObjectClass(typeof(DeletionUponFormatMigrationTestCase.Item)).CascadeOnDelete
				(true);
		}

		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			)
		{
			if (Db4oMajorVersion() < 5 || (Db4oMajorVersion() == 5 && Db4oMinorVersion() < 4))
			{
				return;
			}
			AssertChildItem(objectContainer, ChildToBeDelete, false);
			AssertChildItem(objectContainer, ChildToBeKept, true);
			AssertReferenceToDeletedObjectSetToNull(objectContainer);
			AssertCascadeDeletionOnArrays(objectContainer);
		}

		private void AssertCascadeDeletionOnArrays(IExtObjectContainer objectContainer)
		{
			IObjectSet keptItems = ItemByIdGreaterThan(objectContainer, IdToBeKept);
			Assert.AreEqual(0, keptItems.Count);
		}

		private void AssertReferenceToDeletedObjectSetToNull(IExtObjectContainer objectContainer
			)
		{
			DeletionUponFormatMigrationTestCase.Item item = ItemById(objectContainer, IdToBeKept
				);
			Assert.IsNotNull(item);
			Assert.AreEqual(1, item._array.Length);
			Assert.IsNull(item._array[0]);
		}

		protected override void AssertObjectDeletion(IExtObjectContainer objectContainer)
		{
			DeletionUponFormatMigrationTestCase.Item item = ItemById(objectContainer, IdToBeDeleted
				);
			Assert.IsNotNull(item._child);
			Assert.IsNotNull(item._array[0]);
			objectContainer.Delete(item);
		}

		private void AssertChildItem(IExtObjectContainer objectContainer, string name, bool
			 expectToBeFound)
		{
			IQuery query = objectContainer.Query();
			query.Constrain(typeof(DeletionUponFormatMigrationTestCase.ChildItem));
			query.Descend("_name").Constrain(name);
			IObjectSet result = query.Execute();
			Assert.AreEqual(expectToBeFound, result.HasNext(), name);
			if (expectToBeFound)
			{
				DeletionUponFormatMigrationTestCase.ChildItem childItem = (DeletionUponFormatMigrationTestCase.ChildItem
					)result.Next();
				Assert.AreEqual(name, childItem._name);
			}
		}

		private DeletionUponFormatMigrationTestCase.Item ItemById(IExtObjectContainer objectContainer
			, int id)
		{
			IQuery query = objectContainer.Query();
			query.Constrain(typeof(DeletionUponFormatMigrationTestCase.Item));
			query.Descend("_id").Constrain(id);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			return (DeletionUponFormatMigrationTestCase.Item)result.Next();
		}

		private IObjectSet ItemByIdGreaterThan(IExtObjectContainer objectContainer, int id
			)
		{
			IQuery query = objectContainer.Query();
			query.Constrain(typeof(DeletionUponFormatMigrationTestCase.Item));
			query.Descend("_id").Constrain(id).Greater();
			return query.Execute();
		}

		protected override string FileNamePrefix()
		{
			return "deletion-tests";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			DeletionUponFormatMigrationTestCase.Item item1 = new DeletionUponFormatMigrationTestCase.Item
				(IdToBeDeleted, new DeletionUponFormatMigrationTestCase.ChildItem(ChildToBeDelete
				), ItemsToKeep());
			objectContainer.Store(item1, 10);
			DeletionUponFormatMigrationTestCase.Item item2 = new DeletionUponFormatMigrationTestCase.Item
				(IdToBeKept, new DeletionUponFormatMigrationTestCase.ChildItem(ChildToBeKept), new 
				DeletionUponFormatMigrationTestCase.Item[] { item1 });
			objectContainer.Store(item2, 10);
		}

		private DeletionUponFormatMigrationTestCase.Item[] ItemsToKeep()
		{
			DeletionUponFormatMigrationTestCase.Item[] items = new DeletionUponFormatMigrationTestCase.Item
				[ItemsToKeepCount];
			for (int i = 1; i <= items.Length; i++)
			{
				items[i - 1] = new DeletionUponFormatMigrationTestCase.Item(IdToBeKept + i);
			}
			return items;
		}
	}
}
