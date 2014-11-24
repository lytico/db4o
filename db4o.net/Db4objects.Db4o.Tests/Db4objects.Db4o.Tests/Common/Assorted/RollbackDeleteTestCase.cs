/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class RollbackDeleteTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new RollbackDeleteTestCase().RunNetworking();
		}

		protected override void Store()
		{
			Store(new SimpleObject("hello", 1));
		}

		public virtual void TestDRDC()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			IExtObjectContainer oc3 = OpenNewSession();
			try
			{
				SimpleObject o1 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc1, typeof(SimpleObject
					)));
				oc1.Delete(o1);
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
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Delete(o1);
				oc1.Commit();
				AssertOccurrences(oc3, typeof(SimpleObject), 0);
				AssertOccurrences(oc2, typeof(SimpleObject), 0);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
				oc3.Close();
			}
		}

		public virtual void TestSRDC()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			IExtObjectContainer oc3 = OpenNewSession();
			try
			{
				SimpleObject o1 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc1, typeof(SimpleObject
					)));
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
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Delete(o1);
				oc1.Commit();
				AssertOccurrences(oc3, typeof(SimpleObject), 0);
				AssertOccurrences(oc2, typeof(SimpleObject), 0);
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
