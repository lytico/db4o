/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class STOIntegerWTTestCase : SodaBaseTestCase
	{
		public int i_int;

		public STOIntegerWTTestCase()
		{
		}

		private STOIntegerWTTestCase(int a_int)
		{
			i_int = a_int;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				(99), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(1), new 
				Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(909), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				(1001), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(0), new 
				Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(1010), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				() };
		}

		public virtual void TestDescending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				));
			q.Descend("i_int").OrderDescending();
			ExpectOrdered(q, new int[] { 5, 3, 2, 0, 1, 4, 6 });
		}

		public virtual void TestAscendingGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				));
			IQuery qInt = q.Descend("i_int");
			qInt.Constrain(100).Greater();
			qInt.OrderAscending();
			ExpectOrdered(q, new int[] { 2, 3, 5 });
		}
	}
}
