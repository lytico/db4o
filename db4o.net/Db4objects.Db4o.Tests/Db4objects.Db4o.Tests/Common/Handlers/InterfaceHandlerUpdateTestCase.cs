/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class InterfaceHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		public interface IItemInterface
		{
		}

		public class ItemContainer
		{
			internal InterfaceHandlerUpdateTestCase.IItemInterface _item;

			internal InterfaceHandlerUpdateTestCase.IItemInterface[] _items;

			internal object[] _objects;

			internal object _object;

			public static InterfaceHandlerUpdateTestCase.ItemContainer CreateNew()
			{
				InterfaceHandlerUpdateTestCase.ItemContainer itemContainer = new InterfaceHandlerUpdateTestCase.ItemContainer
					();
				itemContainer._item = StoredItem();
				itemContainer._items = NewItemInterfaceArray();
				itemContainer._objects = NewItemInterfaceArray();
				itemContainer._object = NewItemInterfaceArray();
				return itemContainer;
			}

			private static InterfaceHandlerUpdateTestCase.IItemInterface[] NewItemInterfaceArray
				()
			{
				return new InterfaceHandlerUpdateTestCase.IItemInterface[] { StoredItem() };
			}
		}

		public class Item : InterfaceHandlerUpdateTestCase.IItemInterface
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is InterfaceHandlerUpdateTestCase.Item))
				{
					return false;
				}
				return _name.Equals(((InterfaceHandlerUpdateTestCase.Item)obj)._name);
			}

			public override string ToString()
			{
				return "Item " + _name;
			}
		}

		protected override object[] CreateValues()
		{
			return new object[] { InterfaceHandlerUpdateTestCase.ItemContainer.CreateNew() };
		}

		protected override object CreateArrays()
		{
			return InterfaceHandlerUpdateTestCase.ItemContainer.CreateNew();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			if (Db4oMajorVersion() == 4)
			{
				return;
			}
			AssertItemInterfaceArrays(StoredItemName(), obj);
		}

		private void AssertItemInterfaceArrays(string name, object itemContainerObject)
		{
			InterfaceHandlerUpdateTestCase.ItemContainer itemContainer = (InterfaceHandlerUpdateTestCase.ItemContainer
				)itemContainerObject;
			AssertItemInterfaceArray(name, itemContainer._items);
			AssertItemInterfaceArray(name, itemContainer._objects);
			AssertItemInterfaceArray(name, (object[])itemContainer._object);
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			if (Db4oMajorVersion() == 4)
			{
				return;
			}
			AssertItem(StoredItemName(), ItemFromValues(values));
		}

		protected override void UpdateValues(object[] values)
		{
			if (Db4oMajorVersion() == 4)
			{
				return;
			}
			UpdateItem(ItemFromValues(values));
		}

		private void UpdateItem(InterfaceHandlerUpdateTestCase.Item item)
		{
			item._name = UpdatedItemName();
		}

		private string UpdatedItemName()
		{
			return "updated";
		}

		protected override void AssertUpdatedValues(object[] values)
		{
			if (Db4oMajorVersion() == 4)
			{
				return;
			}
			AssertItem(UpdatedItemName(), ItemFromValues(values));
		}

		protected override void UpdateArrays(object obj)
		{
			if (Db4oMajorVersion() == 4)
			{
				return;
			}
			InterfaceHandlerUpdateTestCase.ItemContainer itemContainer = (InterfaceHandlerUpdateTestCase.ItemContainer
				)obj;
			UpdateItemInterfaceArray(itemContainer._items);
			UpdateItemInterfaceArray(itemContainer._objects);
			UpdateItemInterfaceArray((object[])itemContainer._object);
		}

		protected override void AssertUpdatedArrays(object obj)
		{
			if (Db4oMajorVersion() == 4)
			{
				return;
			}
			AssertItemInterfaceArrays(UpdatedItemName(), obj);
		}

		private InterfaceHandlerUpdateTestCase.Item ItemFromValues(object[] values)
		{
			InterfaceHandlerUpdateTestCase.ItemContainer itemContainer = (InterfaceHandlerUpdateTestCase.ItemContainer
				)values[0];
			InterfaceHandlerUpdateTestCase.IItemInterface item = itemContainer._item;
			return (InterfaceHandlerUpdateTestCase.Item)item;
		}

		private void AssertItem(string name, object item)
		{
			Assert.IsInstanceOf(typeof(InterfaceHandlerUpdateTestCase.Item), item);
			Assert.AreEqual(name, ((InterfaceHandlerUpdateTestCase.Item)item)._name);
		}

		private void AssertItemInterfaceArray(string itemName, object[] items)
		{
			AssertItem(itemName, items[0]);
		}

		private void UpdateItemInterfaceArray(object[] items)
		{
			UpdateItem((InterfaceHandlerUpdateTestCase.Item)items[0]);
		}

		protected override string TypeName()
		{
			return "interface";
		}

		public static InterfaceHandlerUpdateTestCase.Item StoredItem()
		{
			return new InterfaceHandlerUpdateTestCase.Item(StoredItemName());
		}

		private static string StoredItemName()
		{
			return "stored";
		}
	}
}
