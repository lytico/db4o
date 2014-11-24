/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class ConjunctiveQbETestCase : AbstractDb4oTestCase
	{
		public class Sup
		{
			public bool _flag;

			public Sup(bool flag)
			{
				this._flag = flag;
			}

			public virtual IObjectSet Query(IObjectContainer db)
			{
				IQuery query = db.Query();
				query.Constrain(this);
				query.Descend("_flag").Constrain(true).Not();
				return query.Execute();
			}
		}

		public class Sub1 : ConjunctiveQbETestCase.Sup
		{
			public Sub1(bool flag) : base(flag)
			{
			}
		}

		public class Sub2 : ConjunctiveQbETestCase.Sup
		{
			public Sub2(bool flag) : base(flag)
			{
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ConjunctiveQbETestCase.Sub1(false));
			Store(new ConjunctiveQbETestCase.Sub1(true));
			Store(new ConjunctiveQbETestCase.Sub2(false));
			Store(new ConjunctiveQbETestCase.Sub2(true));
		}

		public virtual void TestAndedQbE()
		{
			Assert.AreEqual(1, new ConjunctiveQbETestCase.Sub1(false).Query(Db()).Count);
		}
	}
}
