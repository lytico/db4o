/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	/// <summary>COR-1188</summary>
	public class OrderFollowedByConstraintTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
			public int _id;

			public Data(int id)
			{
				_id = id;
			}
		}

		private static readonly int[] Ids = new int[] { 42, 47, 11, 1, 50, 2 };

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int idIdx = 0; idIdx < Ids.Length; idIdx++)
			{
				Store(new OrderFollowedByConstraintTestCase.Data(Ids[idIdx]));
			}
		}

		public virtual void TestOrderFollowedByConstraint()
		{
			IQuery query = NewQuery(typeof(OrderFollowedByConstraintTestCase.Data));
			query.Descend("_id").OrderAscending();
			query.Descend("_id").Constrain(0).Greater();
			AssertOrdered(query.Execute());
		}

		public virtual void TestLastOrderWins()
		{
			IQuery query = NewQuery(typeof(OrderFollowedByConstraintTestCase.Data));
			query.Descend("_id").OrderDescending();
			query.Descend("_id").OrderAscending();
			query.Descend("_id").Constrain(0).Greater();
			AssertOrdered(query.Execute());
		}

		private void AssertOrdered(IObjectSet result)
		{
			Assert.AreEqual(Ids.Length, result.Count);
			int previousId = 0;
			while (result.HasNext())
			{
				OrderFollowedByConstraintTestCase.Data data = (OrderFollowedByConstraintTestCase.Data
					)result.Next();
				Assert.IsTrue(previousId < data._id);
				previousId = data._id;
			}
		}

		public static void Main(string[] args)
		{
			new OrderFollowedByConstraintTestCase().RunSolo();
		}
	}
}
