/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped
{
	public class STOrUTestCase : SodaBaseTestCase
	{
		public static void Main(string[] arguments)
		{
			new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase().RunAll();
		}

		public object orInt;

		public object orString;

		public STOrUTestCase()
		{
		}

		private STOrUTestCase(int a_int, string a_string)
		{
			orInt = a_int;
			orString = a_string;
		}

		private STOrUTestCase(object a_int, string a_string)
		{
			orInt = a_int;
			orString = a_string;
		}

		public override string ToString()
		{
			return "STOr: int:" + orInt + " str:" + orString;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase
				(0, "hi"), new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(5, 
				null), new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(1000, "joho"
				), new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(30000, "osoo"
				), new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(int.MaxValue
				 - 1, null) };
		}

		public virtual void TestSmallerGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			IQuery sub = q.Descend("orInt");
			sub.Constrain(30000).Greater().Or(sub.Constrain(5).Smaller());
			Expect(q, new int[] { 0, 4 });
		}

		public virtual void TestGreaterGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			IQuery sub = q.Descend("orInt");
			sub.Constrain(30000).Greater().Or(sub.Constrain(5).Greater());
			Expect(q, new int[] { 2, 3, 4 });
		}

		public virtual void TestGreaterEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			IQuery sub = q.Descend("orInt");
			sub.Constrain(1000).Greater().Or(sub.Constrain(0));
			Expect(q, new int[] { 0, 3, 4 });
		}

		public virtual void TestEqualsNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(1000
				, null));
			q.Descend("orInt").Constraints().Or(q.Descend("orString").Constrain(null));
			Expect(q, new int[] { 1, 2, 4 });
		}

		public virtual void TestAndOrAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(null
				, null));
			(q.Descend("orInt").Constrain(5).And(q.Descend("orString").Constrain(null))).Or(q
				.Descend("orInt").Constrain(1000).And(q.Descend("orString").Constrain("joho")));
			Expect(q, new int[] { 1, 2 });
		}

		public virtual void TestOrAndOr()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(null
				, null));
			(q.Descend("orInt").Constrain(5).Or(q.Descend("orString").Constrain(null))).And(q
				.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho")));
			SodaTestUtil.ExpectOne(q, _array[4]);
		}

		public virtual void TestOrOrAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			(q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho"))).Or(q.Descend("orInt").Constrain(5).And(q.Descend("orString").Constrain
				(null)));
			Expect(q, new int[] { 1, 2, 4 });
		}

		public virtual void TestMultiOrAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			((q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho"))).Or(q.Descend("orInt").Constrain(5).And(q.Descend("orString").Constrain
				("joho")))).Or((q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString"
				).Constrain(null))).And(q.Descend("orInt").Constrain(5).Or(q.Descend("orString")
				.Constrain(null))));
			Expect(q, new int[] { 1, 2, 4 });
		}

		public virtual void TestNotSmallerGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			IQuery sub = q.Descend("orInt");
			(sub.Constrain(30000).Greater().Or(sub.Constrain(5).Smaller())).Not();
			Expect(q, new int[] { 1, 2, 3 });
		}

		public virtual void TestNotGreaterGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			IQuery sub = q.Descend("orInt");
			(sub.Constrain(30000).Greater().Or(sub.Constrain(5).Greater())).Not();
			Expect(q, new int[] { 0, 1 });
		}

		public virtual void TestNotGreaterEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			IQuery sub = q.Descend("orInt");
			(sub.Constrain(1000).Greater().Or(sub.Constrain(0))).Not();
			Expect(q, new int[] { 1, 2 });
		}

		public virtual void TestNotEqualsNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase(1000
				, null));
			(q.Descend("orInt").Constraints().Or(q.Descend("orString").Constrain(null))).Not(
				);
			Expect(q, new int[] { 0, 3 });
		}

		public virtual void TestNotAndOrAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			(q.Descend("orInt").Constrain(5).And(q.Descend("orString").Constrain(null))).Or(q
				.Descend("orInt").Constrain(1000).And(q.Descend("orString").Constrain("joho"))).
				Not();
			Expect(q, new int[] { 0, 3, 4 });
		}

		public virtual void TestNotOrAndOr()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			(q.Descend("orInt").Constrain(5).Or(q.Descend("orString").Constrain(null))).And(q
				.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho"))).Not();
			Expect(q, new int[] { 0, 1, 2, 3 });
		}

		public virtual void TestNotOrOrAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			(q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho"))).Or(q.Descend("orInt").Constrain(5).And(q.Descend("orString").Constrain
				(null))).Not();
			Expect(q, new int[] { 0, 3 });
		}

		public virtual void TestNotMultiOrAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			((q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho"))).Or(q.Descend("orInt").Constrain(5).And(q.Descend("orString").Constrain
				("joho")))).Or((q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString"
				).Constrain(null))).And(q.Descend("orInt").Constrain(5).Or(q.Descend("orString")
				.Constrain(null)))).Not();
			Expect(q, new int[] { 0, 3 });
		}

		public virtual void TestOrNotAndOr()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			(q.Descend("orInt").Constrain(int.MaxValue - 1).Or(q.Descend("orString").Constrain
				("joho"))).Not().And(q.Descend("orInt").Constrain(5).Or(q.Descend("orString").Constrain
				(null)));
			Expect(q, new int[] { 1 });
		}

		public virtual void TestAndNotAndAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped.STOrUTestCase());
			(q.Descend("orInt").Constrain(int.MaxValue - 1).And(q.Descend("orString").Constrain
				(null))).Not().And(q.Descend("orInt").Constrain(5).Or(q.Descend("orString").Constrain
				("osoo")));
			Expect(q, new int[] { 1, 3 });
		}
	}
}
