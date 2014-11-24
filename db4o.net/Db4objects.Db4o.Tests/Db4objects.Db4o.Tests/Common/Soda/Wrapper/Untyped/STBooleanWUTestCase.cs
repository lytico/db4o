/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	public class STBooleanWUTestCase : SodaBaseTestCase
	{
		internal static readonly string Descendant = "i_boolean";

		public object i_boolean;

		public STBooleanWUTestCase()
		{
		}

		private STBooleanWUTestCase(bool a_boolean)
		{
			i_boolean = a_boolean;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(false), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(true), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(false), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(false), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				() };
		}

		public virtual void TestEqualsTrue()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(true));
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(true));
		}

		public virtual void TestEqualsFalse()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				(false));
			q.Descend(Descendant).Constrain(false);
			Expect(q, new int[] { 0, 2, 3 });
		}

		public virtual void TestNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				());
			q.Descend(Descendant).Constrain(null);
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				());
		}

		public virtual void TestNullOrTrue()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				());
			IQuery qd = q.Descend(Descendant);
			qd.Constrain(null).Or(qd.Constrain(true));
			Expect(q, new int[] { 1, 4 });
		}

		public virtual void TestNotNullAndFalse()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STBooleanWUTestCase
				());
			IQuery qd = q.Descend(Descendant);
			qd.Constrain(null).Not().And(qd.Constrain(false));
			Expect(q, new int[] { 0, 2, 3 });
		}
	}
}
