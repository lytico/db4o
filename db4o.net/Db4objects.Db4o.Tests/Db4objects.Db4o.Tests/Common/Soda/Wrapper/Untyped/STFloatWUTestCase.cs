/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	public class STFloatWUTestCase : SodaBaseTestCase
	{
		public object i_float;

		public STFloatWUTestCase()
		{
		}

		private STFloatWUTestCase(float a_float)
		{
			i_float = a_float;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STFloatWUTestCase
				(float.MinValue), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STFloatWUTestCase
				((float)0.0000123), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STFloatWUTestCase
				((float)1.345), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STFloatWUTestCase
				(float.MaxValue) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[0]);
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STFloatWUTestCase
				((float)0.1));
			q.Descend("i_float").Constraints().Greater();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STFloatWUTestCase
				((float)1.5));
			q.Descend("i_float").Constraints().Smaller();
			Expect(q, new int[] { 0, 1, 2 });
		}
	}
}
