/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class GetUUIDTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new GetUUIDTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new GetUUIDTestCase.Item("Item to be deleted"));
		}

		public virtual void TestGetUUIDInCommittedCallbacks()
		{
			Db4oUUID itemUUID = GetItemUUID();
			ServerEventRegistry().Committed += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
				(new _IEventListener4_34(itemUUID).OnEvent);
			DeleteAll(typeof(GetUUIDTestCase.Item));
			Db().Commit();
		}

		private sealed class _IEventListener4_34
		{
			public _IEventListener4_34(Db4oUUID itemUUID)
			{
				this.itemUUID = itemUUID;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
			{
				CommitEventArgs commitEventArgs = (CommitEventArgs)args;
				IEnumerator deletedObjectInfoCollection = commitEventArgs.Deleted.GetEnumerator();
				while (deletedObjectInfoCollection.MoveNext())
				{
					IObjectInfo objectInfo = (IObjectInfo)deletedObjectInfoCollection.Current;
					Assert.AreEqual(itemUUID, objectInfo.GetUUID());
				}
			}

			private readonly Db4oUUID itemUUID;
		}

		private Db4oUUID GetItemUUID()
		{
			return GetItemInfo().GetUUID();
		}

		private IObjectInfo GetItemInfo()
		{
			return Db().Ext().GetObjectInfo(((GetUUIDTestCase.Item)RetrieveOnlyInstance(typeof(
				GetUUIDTestCase.Item))));
		}

		public virtual void TestGetUUIDInCommittingCallbacks()
		{
			ServerEventRegistry().Committing += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
				(new _IEventListener4_59().OnEvent);
			DeleteAll(typeof(GetUUIDTestCase.Item));
			Db().Commit();
		}

		private sealed class _IEventListener4_59
		{
			public _IEventListener4_59()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
			{
				CommitEventArgs commitEventArgs = (CommitEventArgs)args;
				IEnumerator deletedObjectInfoCollection = commitEventArgs.Deleted.GetEnumerator();
				while (deletedObjectInfoCollection.MoveNext())
				{
					IObjectInfo objectInfo = (IObjectInfo)deletedObjectInfoCollection.Current;
					Assert.IsNotNull(objectInfo.GetUUID());
				}
			}
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return _name;
			}
		}
	}
}
