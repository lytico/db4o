/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class ComparatorSortTestCase : AbstractDb4oTestCase
	{
		[System.Serializable]
		public class AscendingIdComparator : IQueryComparator
		{
			public virtual int Compare(object first, object second)
			{
				return ((ComparatorSortTestCase.Item)first)._id - ((ComparatorSortTestCase.Item)second
					)._id;
			}
		}

		[System.Serializable]
		public class DescendingIdComparator : IQueryComparator
		{
			public virtual int Compare(object first, object second)
			{
				return ((ComparatorSortTestCase.Item)second)._id - ((ComparatorSortTestCase.Item)
					first)._id;
			}
		}

		[System.Serializable]
		public class OddEvenIdComparator : IQueryComparator
		{
			public virtual int Compare(object first, object second)
			{
				int idA = ((ComparatorSortTestCase.Item)first)._id;
				int idB = ((ComparatorSortTestCase.Item)second)._id;
				int modA = idA % 2;
				int modB = idB % 2;
				if (modA != modB)
				{
					return modA - modB;
				}
				return idA - idB;
			}
		}

		[System.Serializable]
		public class AscendingNameComparator : IQueryComparator
		{
			public virtual int Compare(object first, object second)
			{
				return Sharpen.Runtime.CompareOrdinal(((ComparatorSortTestCase.Item)first)._name, 
					((ComparatorSortTestCase.Item)second)._name);
			}
		}

		[System.Serializable]
		public class SmallerThanThreePredicate : Predicate
		{
			public virtual bool Match(ComparatorSortTestCase.Item candidate)
			{
				return candidate._id < 3;
			}
		}

		public class Item
		{
			public int _id;

			public string _name;

			public Item() : this(0, null)
			{
			}

			public Item(int id, string name)
			{
				this._id = id;
				this._name = name;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ExceptionsOnNotStorable(true);
		}

		protected override void Store()
		{
			for (int i = 0; i < 4; i++)
			{
				Store(new ComparatorSortTestCase.Item(i, (3 - i).ToString()));
			}
		}

		public virtual void TestByIdAscending()
		{
			AssertIdOrder(new ComparatorSortTestCase.AscendingIdComparator(), new int[] { 0, 
				1, 2, 3 });
		}

		public virtual void TestByIdAscendingConstrained()
		{
			IQuery query = NewItemQuery();
			query.Descend("_id").Constrain(3).Smaller();
			AssertIdOrder(query, new ComparatorSortTestCase.AscendingIdComparator(), new int[
				] { 0, 1, 2 });
		}

		public virtual void TestByIdAscendingNQ()
		{
			IObjectSet result = Db().Query(new ComparatorSortTestCase.SmallerThanThreePredicate
				(), new ComparatorSortTestCase.AscendingIdComparator());
			AssertIdOrder(result, new int[] { 0, 1, 2 });
		}

		public virtual void TestByIdDescending()
		{
			AssertIdOrder(new ComparatorSortTestCase.DescendingIdComparator(), new int[] { 3, 
				2, 1, 0 });
		}

		public virtual void TestByIdDescendingConstrained()
		{
			IQuery query = NewItemQuery();
			query.Descend("_id").Constrain(3).Smaller();
			AssertIdOrder(query, new ComparatorSortTestCase.DescendingIdComparator(), new int
				[] { 2, 1, 0 });
		}

		public virtual void TestByIdDescendingNQ()
		{
			IObjectSet result = Db().Query(new ComparatorSortTestCase.SmallerThanThreePredicate
				(), new ComparatorSortTestCase.DescendingIdComparator());
			AssertIdOrder(result, new int[] { 2, 1, 0 });
		}

		public virtual void TestByIdOddEven()
		{
			AssertIdOrder(new ComparatorSortTestCase.OddEvenIdComparator(), new int[] { 0, 2, 
				1, 3 });
		}

		public virtual void TestByIdOddEvenConstrained()
		{
			IQuery query = NewItemQuery();
			query.Descend("_id").Constrain(3).Smaller();
			AssertIdOrder(query, new ComparatorSortTestCase.OddEvenIdComparator(), new int[] 
				{ 0, 2, 1 });
		}

		public virtual void TestByIdOddEvenNQ()
		{
			IObjectSet result = Db().Query(new ComparatorSortTestCase.SmallerThanThreePredicate
				(), new ComparatorSortTestCase.OddEvenIdComparator());
			AssertIdOrder(result, new int[] { 0, 2, 1 });
		}

		public virtual void TestByNameAscending()
		{
			AssertIdOrder(new ComparatorSortTestCase.AscendingNameComparator(), new int[] { 3
				, 2, 1, 0 });
		}

		public virtual void TestByNameAscendingConstrained()
		{
			IQuery query = NewItemQuery();
			query.Descend("_id").Constrain(3).Smaller();
			AssertIdOrder(query, new ComparatorSortTestCase.AscendingNameComparator(), new int
				[] { 2, 1, 0 });
		}

		public virtual void TestByNameAscendingNQ()
		{
			IObjectSet result = Db().Query(new ComparatorSortTestCase.SmallerThanThreePredicate
				(), new ComparatorSortTestCase.AscendingNameComparator());
			AssertIdOrder(result, new int[] { 2, 1, 0 });
		}

		private void AssertIdOrder(IQueryComparator comparator, int[] ids)
		{
			IQuery query = NewItemQuery();
			AssertIdOrder(query, comparator, ids);
		}

		private IQuery NewItemQuery()
		{
			return NewQuery(typeof(ComparatorSortTestCase.Item));
		}

		private void AssertIdOrder(IQuery query, IQueryComparator comparator, int[] ids)
		{
			query.SortBy(comparator);
			IObjectSet result = query.Execute();
			AssertIdOrder(result, ids);
		}

		private void AssertIdOrder(IObjectSet result, int[] ids)
		{
			Assert.AreEqual(ids.Length, result.Count);
			for (int idx = 0; idx < ids.Length; idx++)
			{
				Assert.AreEqual(ids[idx], ((ComparatorSortTestCase.Item)result.Next())._id);
			}
		}
	}
}
