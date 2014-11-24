/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	public class STDoubleWUTestCase : SodaBaseTestCase
	{
		public object i_double;

		public STDoubleWUTestCase()
		{
		}

		private STDoubleWUTestCase(double a_double)
		{
			i_double = a_double;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase
				(0), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase(0)
				, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase(1.01)
				, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase(99.99
				), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase(909.00
				) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase
				(0));
			// Primitive default values are ignored, so we need an 
			// additional constraint:
			q.Descend("i_double").Constrain(System.Convert.ToDouble(0));
			Expect(q, new int[] { 0, 1 });
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase
				(1));
			q.Descend("i_double").Constraints().Greater();
			Expect(q, new int[] { 2, 3, 4 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase
				(1));
			q.Descend("i_double").Constraints().Smaller();
			Expect(q, new int[] { 0, 1 });
		}

		public virtual void TestGreaterOrEqual()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			q.Descend("i_double").Constraints().Greater().Equal();
			Expect(q, new int[] { 2, 3, 4 });
		}

		public virtual void TestGreaterAndNot()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STDoubleWUTestCase
				());
			IQuery val = q.Descend("i_double");
			val.Constrain(System.Convert.ToDouble(0)).Greater();
			val.Constrain(99.99).Not();
			Expect(q, new int[] { 2, 4 });
		}
	}
}
