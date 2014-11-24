/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	/// <summary>Tests for COR-1007</summary>
	public class STOrderingTestCase : SodaBaseTestCase, IOptOutMultiSession
	{
		public static void Main(string[] args)
		{
			new STOrderingTestCase().RunSolo();
		}

		public override object[] CreateData()
		{
			return new object[] { new OrderTestSubject("Alexandr", 30, 5), new OrderTestSubject
				("Cris", 30, 5), new OrderTestSubject("Boris", 30, 5), new OrderTestSubject("Helen"
				, 25, 5), new OrderTestSubject("Zeus", 25, 3), new OrderTestSubject("Alexsandra"
				, 25, 3), new OrderTestSubject("Liza", 25, 4), new OrderTestSubject("Bred", 25, 
				3), new OrderTestSubject("Liza", 25, 3), new OrderTestSubject("Gregory", 25, 4) };
		}

		// 0
		// 1
		// 2
		// 3
		// 4
		// 5
		// 6
		// 7
		// 8
		// 9
		public virtual void TestFirstAndSecondFieldsAreIrrelevant()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_seniority").OrderAscending();
			q.Descend("_age").OrderAscending();
			q.Descend("_name").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 8, 4, 9, 6, 3, 0, 2, 1 });
		}

		public virtual void TestSecondAndThirdFieldsAreIrrelevant()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_age").OrderAscending();
			q.Descend("_name").OrderAscending();
			q.Descend("_seniority").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 9, 3, 8, 6, 4, 0, 2, 1 });
		}

		public virtual void TestOrderByNameAndAgeAscending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_age").OrderAscending();
			q.Descend("_name").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 9, 3, 6, 8, 4, 0, 2, 1 });
		}

		public virtual void TestAscendingOrderWithOutAge()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_seniority").OrderAscending();
			q.Descend("_name").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 8, 4, 9, 6, 0, 2, 1, 3 });
		}
	}
}
