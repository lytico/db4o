/* Copyright (C) 2010  Versant Inc.  http://www.db4o.com */
using System.Linq;

namespace Db4objects.Db4o.Linq.Tests
{
    public class Item
	{
		public Item(string id) : this(id, null)
		{
		}

		public Item(string id, Item parent)
		{
			Parent = parent;
			Id = id;
		}

		public override bool Equals(object obj)
		{
			Item other = obj as Item;
			if (other == null) return false;

			bool sameId = Id == other.Id;
			return Parent != null ? sameId && Parent.Equals(other.Parent) : sameId;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ (Parent != null ? Parent.GetHashCode() : 37);
		}
		
		public Item Parent;
		public string Id;
	}

	public class NullConstantTestCase : AbstractDb4oLinqTestCase
	{
		private static Item[] _items = new[]
		                               	{
		                               		new Item("foo", new Item("bar")), 
		                               		new Item("baz"), 
										};

		protected override void Store()
		{
			foreach (var item in _items)
			{
				Store(item);
			}
		}

		public void Test()
		{
			AssertQuery(
				from Item item in Db()
				where item.Parent != null
				select item,

				"(Item(Parent not null))",
				
				from item in _items
				where item.Parent != null
				select item);
		}
	}
}
