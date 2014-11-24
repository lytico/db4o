/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class STOStringTestCase : SodaBaseTestCase
	{
		public string foo;

		public STOStringTestCase()
		{
		}

		public STOStringTestCase(string str)
		{
			this.foo = str;
		}

		public override string ToString()
		{
			return foo;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("bbb"), 
				new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("dod"), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase
				("aaa"), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("Xbb"), 
				new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("bbq") };
		}

		public virtual void TestAscending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			q.Descend("foo").OrderAscending();
			ExpectOrdered(q, new int[] { 0, 4, 3, 1, 5, 2 });
		}

		public virtual void TestDescending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			q.Descend("foo").OrderDescending();
			ExpectOrdered(q, new int[] { 2, 5, 1, 3, 4, 0 });
		}

		public virtual void TestAscendingLike()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			IQuery qStr = q.Descend("foo");
			qStr.Constrain("b").Like();
			qStr.OrderAscending();
			ExpectOrdered(q, new int[] { 4, 1, 5 });
		}

		public virtual void TestDescendingContains()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			IQuery qStr = q.Descend("foo");
			qStr.Constrain("b").Contains();
			qStr.OrderDescending();
			ExpectOrdered(q, new int[] { 5, 1, 4 });
		}
	}
}
