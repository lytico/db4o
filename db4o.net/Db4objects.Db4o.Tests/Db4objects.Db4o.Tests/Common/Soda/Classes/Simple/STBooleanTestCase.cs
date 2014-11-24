/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Simple
{
	public class STBooleanTestCase : SodaBaseTestCase
	{
		public bool i_boolean;

		public STBooleanTestCase()
		{
		}

		private STBooleanTestCase(bool a_boolean)
		{
			i_boolean = a_boolean;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase
				(false), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase(
				true), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase(false
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase(false)
				 };
		}

		public virtual void TestEqualsTrue()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase
				(true));
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase
				(true));
		}

		public virtual void TestEqualsFalse()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STBooleanTestCase
				(false));
			q.Descend("i_boolean").Constrain(false);
			Expect(q, new int[] { 0, 2, 3 });
		}
	}
}
