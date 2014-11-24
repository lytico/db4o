/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Simple
{
	public class STLongTestCase : SodaBaseTestCase
	{
		public long i_long;

		public STLongTestCase()
		{
		}

		private STLongTestCase(long a_long)
		{
			i_long = a_long;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase
				(long.MinValue), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase
				(-1), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase(0), new 
				Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase(long.MaxValue - 
				1) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase(long.MinValue
				));
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase
				(long.MinValue) });
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase(-
				1));
			q.Descend("i_long").Constraints().Greater();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase(1
				));
			q.Descend("i_long").Constraints().Smaller();
			Expect(q, new int[] { 0, 1, 2 });
		}

		public virtual void TestBetween()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase()
				);
			IQuery sub = q.Descend("i_long");
			sub.Constrain(System.Convert.ToInt64(-3)).Greater();
			sub.Constrain(System.Convert.ToInt64(3)).Smaller();
			Expect(q, new int[] { 1, 2 });
		}

		public virtual void TestAnd()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase()
				);
			IQuery sub = q.Descend("i_long");
			sub.Constrain(System.Convert.ToInt64(-3)).Greater().And(sub.Constrain(System.Convert.ToInt64
				(3)).Smaller());
			Expect(q, new int[] { 1, 2 });
		}

		public virtual void TestOr()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STLongTestCase()
				);
			IQuery sub = q.Descend("i_long");
			sub.Constrain(System.Convert.ToInt64(3)).Greater().Or(sub.Constrain(System.Convert.ToInt64
				(-3)).Smaller());
			Expect(q, new int[] { 0, 3 });
		}
	}
}
