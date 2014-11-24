/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class STOIntegerTestCase : SodaBaseTestCase
	{
		public int i_int;

		public STOIntegerTestCase()
		{
		}

		private STOIntegerTestCase(int a_int)
		{
			i_int = a_int;
		}

		public override string ToString()
		{
			return "STInteger: " + i_int;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase
				(1001), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase(99), new 
				Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase(1), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase
				(909), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase(1001), new 
				Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase(0), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase
				(1010) };
		}

		public virtual void TestAscending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase));
			q.Descend("i_int").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 2, 1, 3, 0, 4, 6 });
		}

		public virtual void TestDescending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase));
			q.Descend("i_int").OrderDescending();
			ExpectOrdered(q, new int[] { 6, 4, 0, 3, 1, 2, 5 });
		}

		public virtual void TestAscendingGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerTestCase));
			IQuery qInt = q.Descend("i_int");
			qInt.Constrain(100).Greater();
			qInt.OrderAscending();
			ExpectOrdered(q, new int[] { 3, 0, 4, 6 });
		}
	}
}
