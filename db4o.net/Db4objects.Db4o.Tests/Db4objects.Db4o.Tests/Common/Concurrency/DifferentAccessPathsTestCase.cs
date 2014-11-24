/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class DifferentAccessPathsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new DifferentAccessPathsTestCase().RunConcurrency();
		}

		public string foo;

		protected override void Store()
		{
			DifferentAccessPathsTestCase dap = new DifferentAccessPathsTestCase();
			dap.foo = "hi";
			Store(dap);
			dap = new DifferentAccessPathsTestCase();
			dap.foo = "hi too";
			Store(dap);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc(IExtObjectContainer oc)
		{
			DifferentAccessPathsTestCase dap = Query(oc);
			for (int i = 0; i < 10; i++)
			{
				Assert.AreSame(dap, Query(oc));
			}
			oc.Purge(dap);
			Assert.AreNotSame(dap, Query(oc));
		}

		private DifferentAccessPathsTestCase Query(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(DifferentAccessPathsTestCase));
			q.Descend("foo").Constrain("hi");
			IObjectSet os = q.Execute();
			Assert.AreEqual(1, os.Count);
			DifferentAccessPathsTestCase dap = (DifferentAccessPathsTestCase)os.Next();
			return dap;
		}
	}
}
#endif // !SILVERLIGHT
