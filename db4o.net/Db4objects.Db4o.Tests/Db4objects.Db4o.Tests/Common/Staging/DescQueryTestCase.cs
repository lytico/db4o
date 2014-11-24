/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class DescQueryTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int _id;

			public DescQueryTestCase.Item _child;

			public Item(int id) : this(id, null)
			{
			}

			public Item(int id, DescQueryTestCase.Item child)
			{
				// COR-2141
				this._id = id;
				this._child = child;
			}

			public override bool Equals(object other)
			{
				if (other == this)
				{
					return true;
				}
				if (other == null || GetType() != other.GetType())
				{
					return false;
				}
				DescQueryTestCase.Item item = (DescQueryTestCase.Item)other;
				if (_id != item._id)
				{
					return false;
				}
				return _child == null ? item._child == null : _child.Equals(item._child);
			}

			public override int GetHashCode()
			{
				return _id;
			}

			public override string ToString()
			{
				return _id + "[" + (_child == null ? string.Empty : _child.ToString()) + "]";
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new DescQueryTestCase.Item(42, new DescQueryTestCase.Item(43)));
			Store(new DescQueryTestCase.Item(42, new DescQueryTestCase.Item(44)));
			Store(new DescQueryTestCase.Item(45, new DescQueryTestCase.Item(46)));
		}

		public virtual void TestDescQuery()
		{
			IQuery query = NewQuery(typeof(DescQueryTestCase.Item));
			IQuery sq = query.Descend("_child");
			query.Descend("_id").Constrain(42).And(sq.Descend("_id").Constrain(43).Greater());
			IObjectSet result = sq.Execute();
			Iterator4Assert.SameContent(Iterators.Iterate(new DescQueryTestCase.Item[] { new 
				DescQueryTestCase.Item(44) }), Iterators.Iterator(result));
		}
	}
}
