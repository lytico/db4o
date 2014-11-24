/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class NullIdentityConstraintTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
			public NullIdentityConstraintTestCase.Data _prev;

			public Data(NullIdentityConstraintTestCase.Data prev)
			{
				this._prev = prev;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			NullIdentityConstraintTestCase.Data a = new NullIdentityConstraintTestCase.Data(null
				);
			NullIdentityConstraintTestCase.Data b = new NullIdentityConstraintTestCase.Data(a
				);
			Store(b);
		}

		public virtual void TestNullIdentity()
		{
			IQuery query = NewQuery(typeof(NullIdentityConstraintTestCase.Data));
			query.Descend("_prev").Constrain(null).Identity();
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
