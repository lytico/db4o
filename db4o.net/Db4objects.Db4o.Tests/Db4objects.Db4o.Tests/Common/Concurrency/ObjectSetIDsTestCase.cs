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
	public class ObjectSetIDsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ObjectSetIDsTestCase().RunConcurrency();
		}

		internal const int Count = 11;

		protected override void Store()
		{
			for (int i = 0; i < Count; i++)
			{
				Store(new ObjectSetIDsTestCase());
			}
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(this.GetType());
			IObjectSet res = q.Execute();
			Assert.AreEqual(Count, res.Count);
			long[] ids1 = new long[res.Count];
			int i = 0;
			while (res.HasNext())
			{
				ids1[i++] = oc.GetID(res.Next());
			}
			res.Reset();
			long[] ids2 = res.Ext().GetIDs();
			Assert.AreEqual(Count, ids1.Length);
			Assert.AreEqual(Count, ids2.Length);
			for (int j = 0; j < ids1.Length; j++)
			{
				bool found = false;
				for (int k = 0; k < ids2.Length; k++)
				{
					if (ids1[j] == ids2[k])
					{
						found = true;
						break;
					}
				}
				Assert.IsTrue(found);
			}
		}
	}
}
#endif // !SILVERLIGHT
