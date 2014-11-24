/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class LazyObjectReferenceTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] arguments)
		{
			new LazyObjectReferenceTestCase().RunSolo();
		}

		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.ObjectClass(typeof(LazyObjectReferenceTestCase.Item)).GenerateUUIDs(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < 10; i++)
			{
				Store(new LazyObjectReferenceTestCase.Item());
			}
		}

		public virtual void Test()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(LazyObjectReferenceTestCase.Item));
			IObjectSet objectSet = q.Execute();
			long[] ids = objectSet.Ext().GetIDs();
			IObjectInfo[] infos = new IObjectInfo[ids.Length];
			LazyObjectReferenceTestCase.Item[] items = new LazyObjectReferenceTestCase.Item[ids
				.Length];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = (LazyObjectReferenceTestCase.Item)Db().GetByID(ids[i]);
				infos[i] = new LazyObjectReference(Trans(), (int)ids[i]);
			}
			AssertInfosAreConsistent(ids, infos);
			for (int i = 0; i < items.Length; i++)
			{
				Db().Purge(items[i]);
			}
			Db().Purge();
			AssertInfosAreConsistent(ids, infos);
		}

		private void AssertInfosAreConsistent(long[] ids, IObjectInfo[] infos)
		{
			for (int i = 0; i < infos.Length; i++)
			{
				IObjectInfo info = Db().GetObjectInfo(Db().GetByID(ids[i]));
				Assert.AreEqual(info.GetInternalID(), infos[i].GetInternalID());
				Assert.AreEqual(info.GetUUID().GetLongPart(), infos[i].GetUUID().GetLongPart());
				Assert.AreSame(info.GetObject(), infos[i].GetObject());
			}
		}
	}
}
