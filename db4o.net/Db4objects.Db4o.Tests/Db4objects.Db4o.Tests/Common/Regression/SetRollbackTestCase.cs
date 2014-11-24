/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4objects.Db4o.Tests.Common.Regression;

namespace Db4objects.Db4o.Tests.Common.Regression
{
	public class SetRollbackTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new SetRollbackTestCase().RunNetworking();
		}

		public virtual void TestSetRollback()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			try
			{
				for (int i = 0; i < 1000; i++)
				{
					SimpleObject obj1 = new SimpleObject("oc " + i, i);
					SimpleObject obj2 = new SimpleObject("oc2 " + i, i);
					oc1.Store(obj1);
					oc2.Store(obj2);
					oc2.Rollback();
					obj2 = new SimpleObject("oc2.2 " + i, i);
					oc2.Store(obj2);
				}
				oc1.Commit();
				oc2.Rollback();
				Assert.AreEqual(1000, oc1.Query(typeof(SimpleObject)).Count);
				Assert.AreEqual(1000, oc2.Query(typeof(SimpleObject)).Count);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
			}
		}
	}
}
