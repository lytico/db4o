/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeOnActivate : AbstractDb4oTestCase, IOptOutTA
	{
		public string name;

		public CascadeOnActivate child;

		protected override void Configure(IConfiguration conf)
		{
			conf.ObjectClass(this).CascadeOnActivate(true);
		}

		protected override void Store()
		{
			CascadeOnActivate coa = new CascadeOnActivate();
			coa.name = "1";
			coa.child = new CascadeOnActivate();
			coa.child.name = "2";
			coa.child.child = new CascadeOnActivate();
			coa.child.child.name = "3";
			Db().Store(coa);
		}

		public virtual void Test()
		{
			IQuery q = NewQuery(GetType());
			q.Descend("name").Constrain("1");
			IObjectSet os = q.Execute();
			CascadeOnActivate coa = (CascadeOnActivate)os.Next();
			CascadeOnActivate coa3 = coa.child.child;
			Assert.AreEqual("3", coa3.name);
			Db().Deactivate(coa, int.MaxValue);
			Assert.IsNull(coa3.name);
			Db().Activate(coa, 1);
			Assert.AreEqual("3", coa3.name);
		}
	}
}
