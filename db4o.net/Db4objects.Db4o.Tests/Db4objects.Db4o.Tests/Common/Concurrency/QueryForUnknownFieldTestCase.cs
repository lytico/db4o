/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class QueryForUnknownFieldTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.QueryForUnknownFieldTestCase().RunConcurrency
				();
		}

		public string _name;

		public QueryForUnknownFieldTestCase()
		{
		}

		public QueryForUnknownFieldTestCase(string name)
		{
			_name = name;
		}

		protected override void Store()
		{
			_name = "name";
			Store(this);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.QueryForUnknownFieldTestCase
				));
			q.Descend("_name").Constrain("name");
			Assert.AreEqual(1, q.Execute().Count);
			q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.QueryForUnknownFieldTestCase
				));
			q.Descend("name").Constrain("name");
			Assert.AreEqual(0, q.Execute().Count);
		}
	}
}
#endif // !SILVERLIGHT
