/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class RollbackUpdateTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new RollbackUpdateTestCase().RunNetworking();
		}

		protected override void Store()
		{
			Store(new SimpleObject("hello", 1));
		}

		public virtual void Test()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			IExtObjectContainer oc3 = OpenNewSession();
			try
			{
				SimpleObject o1 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc1, typeof(SimpleObject
					)));
				o1.SetS("o1");
				oc1.Store(o1);
				SimpleObject o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject
					)));
				Assert.AreEqual("hello", o2.GetS());
				oc1.Rollback();
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Commit();
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Store(o1);
				oc1.Commit();
				oc2.Refresh(o2, int.MaxValue);
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				Assert.AreEqual("o1", o2.GetS());
				SimpleObject o3 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc3, typeof(SimpleObject
					)));
				Assert.AreEqual("o1", o3.GetS());
			}
			finally
			{
				oc1.Close();
				oc2.Close();
				oc3.Close();
			}
		}
	}
}
