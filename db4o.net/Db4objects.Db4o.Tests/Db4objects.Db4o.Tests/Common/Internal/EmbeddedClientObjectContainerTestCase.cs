/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class EmbeddedClientObjectContainerTestCase : Db4oTestWithTempFile
	{
		private static readonly string FieldName = "_name";

		private LocalObjectContainer _server;

		protected IExtObjectContainer _client1;

		protected IExtObjectContainer _client2;

		private static readonly string OriginalName = "original";

		private static readonly string ChangedName = "changed";

		public class ItemHolder
		{
			public EmbeddedClientObjectContainerTestCase.Item _item;

			public ItemHolder(EmbeddedClientObjectContainerTestCase.Item item)
			{
				_item = item;
			}
		}

		public class Item
		{
			public string _name;

			public Item()
			{
			}

			public Item(string name)
			{
				_name = name;
			}
		}

		public virtual void TestReferenceSystemIsolation()
		{
			EmbeddedClientObjectContainerTestCase.Item item = new EmbeddedClientObjectContainerTestCase.Item
				("one");
			_client1.Store(item);
			_client1.Commit();
			EmbeddedClientObjectContainerTestCase.Item client2Item = RetrieveItemFromClient2(
				);
			Assert.AreNotSame(item, client2Item);
		}

		public virtual void TestSetAndCommitIsolation()
		{
			EmbeddedClientObjectContainerTestCase.Item item = new EmbeddedClientObjectContainerTestCase.Item
				("one");
			_client1.Store(item);
			AssertItemCount(_client2, 0);
			_client1.Commit();
			AssertItemCount(_client2, 1);
		}

		public virtual void TestActivate()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			long id = _client1.GetID(storedItem);
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = (EmbeddedClientObjectContainerTestCase.Item
				)_client2.GetByID(id);
			Assert.IsNull(retrievedItem._name);
			Assert.IsFalse(_client2.IsActive(retrievedItem));
			_client2.Activate(retrievedItem, 1);
			Assert.AreEqual(OriginalName, retrievedItem._name);
			Assert.IsTrue(_client2.IsActive(retrievedItem));
		}

		public virtual void TestBackup()
		{
			Assert.Expect(typeof(NotSupportedException), new _ICodeBlock_84(this));
		}

		private sealed class _ICodeBlock_84 : ICodeBlock
		{
			public _ICodeBlock_84(EmbeddedClientObjectContainerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing._client1.Backup(string.Empty);
			}

			private readonly EmbeddedClientObjectContainerTestCase _enclosing;
		}

		public virtual void TestBindIsolation()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			long id = _client1.GetID(storedItem);
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = RetrieveItemFromClient2
				();
			EmbeddedClientObjectContainerTestCase.Item boundItem = new EmbeddedClientObjectContainerTestCase.Item
				(ChangedName);
			_client1.Bind(boundItem, id);
			Assert.AreSame(boundItem, _client1.GetByID(id));
			Assert.AreSame(retrievedItem, _client2.GetByID(id));
		}

		public virtual void TestClose()
		{
			Transaction trans = null;
			lock (_server.Lock())
			{
				trans = _server.NewUserTransaction();
			}
			IReferenceSystem referenceSystem = trans.ReferenceSystem();
			ObjectContainerSession client = new ObjectContainerSession(_server, trans);
			// FIXME: Need to unregister reference system also
			//        for crashed clients that never get closed. 
			client.Close();
			// should have been removed on close.
			bool wasNotRemovedYet = _server.ReferenceSystemRegistry().RemoveReferenceSystem(referenceSystem
				);
			Assert.IsFalse(wasNotRemovedYet);
		}

		public virtual void TestCommitOnClose()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			storedItem._name = ChangedName;
			_client1.Store(storedItem);
			_client1.Close();
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = RetrieveItemFromClient2
				();
			Assert.AreEqual(ChangedName, retrievedItem._name);
		}

		public virtual void TestConfigure()
		{
			Assert.IsNotNull(_client1.Configure());
		}

		public virtual void TestDeactivate()
		{
			EmbeddedClientObjectContainerTestCase.Item item = StoreItemToClient1AndCommit();
			EmbeddedClientObjectContainerTestCase.ItemHolder holder = new EmbeddedClientObjectContainerTestCase.ItemHolder
				(item);
			_client1.Store(holder);
			_client1.Commit();
			_client1.Deactivate(holder, 1);
			Assert.IsNull(holder._item);
		}

		public virtual void TestDelete()
		{
			EmbeddedClientObjectContainerTestCase.Item item = StoreItemToClient1AndCommit();
			Assert.IsTrue(_client1.IsStored(item));
			_client1.Delete(item);
			Assert.IsFalse(_client1.IsStored(item));
		}

		public virtual void TestDescendIsolation()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			storedItem._name = ChangedName;
			_client1.Store(storedItem);
			int id = (int)_client1.GetID(storedItem);
			object retrievedItem = _client2.GetByID(id);
			Assert.IsNotNull(retrievedItem);
			object descendValue = _client2.Descend(retrievedItem, new string[] { FieldName });
			Assert.AreEqual(OriginalName, descendValue);
			_client1.Commit();
			descendValue = _client2.Descend(retrievedItem, new string[] { FieldName });
			Assert.AreEqual(ChangedName, descendValue);
		}

		public virtual void TestExt()
		{
			Assert.IsInstanceOf(typeof(IExtObjectContainer), _client1.Ext());
		}

		public virtual void TestGet()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			object retrievedItem = _client1.QueryByExample(new EmbeddedClientObjectContainerTestCase.Item
				()).Next();
			Assert.AreSame(storedItem, retrievedItem);
		}

		public virtual void TestGetID()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			long id = _client1.GetID(storedItem);
			Assert.IsGreater(1, id);
		}

		public virtual void TestGetByID()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			long id = _client1.GetID(storedItem);
			Assert.AreSame(storedItem, _client1.GetByID(id));
		}

		public virtual void TestGetObjectInfo()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			IObjectInfo objectInfo = _client1.GetObjectInfo(storedItem);
			Assert.IsNotNull(objectInfo);
		}

		public virtual void TestGetByUUID()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			IObjectInfo objectInfo = _client1.GetObjectInfo(storedItem);
			object retrievedItem = _client1.GetByUUID(objectInfo.GetUUID());
			Assert.AreSame(storedItem, retrievedItem);
			retrievedItem = _client2.GetByUUID(objectInfo.GetUUID());
			Assert.AreNotSame(storedItem, retrievedItem);
		}

		public virtual void TestIdenity()
		{
			Db4oDatabase identity1 = _client1.Identity();
			Assert.IsNotNull(identity1);
			Db4oDatabase identity2 = _client2.Identity();
			Assert.IsNotNull(identity2);
			// TODO: Db4oDatabase is shared between embedded clients.
			// This should work, since there is an automatic bind
			// replacement. Replication test cases will tell.
			Assert.AreSame(identity1, identity2);
		}

		public virtual void TestIsCached()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			long id = _client1.GetID(storedItem);
			Assert.IsFalse(_client2.IsCached(id));
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = (EmbeddedClientObjectContainerTestCase.Item
				)_client2.GetByID(id);
			Assert.IsNotNull(retrievedItem);
			Assert.IsTrue(_client2.IsCached(id));
		}

		public virtual void TestIsClosed()
		{
			_client1.Close();
			Assert.IsTrue(_client1.IsClosed());
		}

		public virtual void TestIsStored()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			Assert.IsTrue(_client1.IsStored(storedItem));
			Assert.IsFalse(_client2.IsStored(storedItem));
		}

		public virtual void TestKnownClasses()
		{
			IReflectClass[] knownClasses = _client1.KnownClasses();
			IReflectClass itemClass = _client1.Reflector().ForClass(typeof(EmbeddedClientObjectContainerTestCase.Item
				));
			ArrayAssert.ContainsByIdentity(knownClasses, new IReflectClass[] { itemClass });
		}

		public virtual void TestLock()
		{
			Assert.AreSame(_server.Lock(), _client1.Lock());
		}

		public virtual void TestPeekPersisted()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			storedItem._name = ChangedName;
			_client1.Store(storedItem);
			EmbeddedClientObjectContainerTestCase.Item peekedItem = (EmbeddedClientObjectContainerTestCase.Item
				)((EmbeddedClientObjectContainerTestCase.Item)_client1.PeekPersisted(storedItem, 
				2, true));
			Assert.IsNotNull(peekedItem);
			Assert.AreNotSame(peekedItem, storedItem);
			Assert.AreEqual(OriginalName, peekedItem._name);
			peekedItem = (EmbeddedClientObjectContainerTestCase.Item)((EmbeddedClientObjectContainerTestCase.Item
				)_client1.PeekPersisted(storedItem, 2, false));
			Assert.IsNotNull(peekedItem);
			Assert.AreNotSame(peekedItem, storedItem);
			Assert.AreEqual(ChangedName, peekedItem._name);
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = RetrieveItemFromClient2
				();
			peekedItem = (EmbeddedClientObjectContainerTestCase.Item)((EmbeddedClientObjectContainerTestCase.Item
				)_client2.PeekPersisted(retrievedItem, 2, false));
			Assert.IsNotNull(peekedItem);
			Assert.AreNotSame(peekedItem, retrievedItem);
			Assert.AreEqual(OriginalName, peekedItem._name);
		}

		public virtual void TestPurge()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			Assert.IsTrue(_client1.IsStored(storedItem));
			_client1.Purge(storedItem);
			Assert.IsFalse(_client1.IsStored(storedItem));
		}

		public virtual void TestReflector()
		{
			Assert.IsNotNull(_client1.Reflector());
		}

		public virtual void TestRefresh()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			storedItem._name = ChangedName;
			_client1.Refresh(storedItem, 2);
			Assert.AreEqual(OriginalName, storedItem._name);
		}

		public virtual void TestRollback()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			storedItem._name = ChangedName;
			_client1.Store(storedItem);
			_client1.Rollback();
			_client1.Commit();
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = RetrieveItemFromClient2
				();
			Assert.AreEqual(OriginalName, retrievedItem._name);
		}

		public virtual void TestSetSemaphore()
		{
			string semaphoreName = "sem";
			Assert.IsTrue(_client1.SetSemaphore(semaphoreName, 0));
			Assert.IsFalse(_client2.SetSemaphore(semaphoreName, 0));
			_client1.ReleaseSemaphore(semaphoreName);
			Assert.IsTrue(_client2.SetSemaphore(semaphoreName, 0));
			_client2.Close();
			Assert.IsTrue(_client1.SetSemaphore(semaphoreName, 0));
		}

		public virtual void TestSetWithDepth()
		{
			EmbeddedClientObjectContainerTestCase.Item item = StoreItemToClient1AndCommit();
			EmbeddedClientObjectContainerTestCase.ItemHolder holder = new EmbeddedClientObjectContainerTestCase.ItemHolder
				(item);
			_client1.Store(holder);
			_client1.Commit();
			item._name = ChangedName;
			_client1.Store(holder, 3);
			_client1.Refresh(holder, 3);
			Assert.AreEqual(ChangedName, item._name);
		}

		public virtual void TestStoredFieldIsolation()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = StoreItemToClient1AndCommit
				();
			storedItem._name = ChangedName;
			_client1.Store(storedItem);
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = RetrieveItemFromClient2
				();
			IStoredClass storedClass = _client2.StoredClass(typeof(EmbeddedClientObjectContainerTestCase.Item
				));
			IStoredField storedField = storedClass.StoredField(FieldName, null);
			object retrievedName = storedField.Get(retrievedItem);
			Assert.AreEqual(OriginalName, retrievedName);
			_client1.Commit();
			retrievedName = storedField.Get(retrievedItem);
			Assert.AreEqual(ChangedName, retrievedName);
		}

		public virtual void TestStoredClasses()
		{
			StoreItemToClient1AndCommit();
			IStoredClass[] storedClasses = _client1.StoredClasses();
			IStoredClass storedClass = _client1.StoredClass(typeof(EmbeddedClientObjectContainerTestCase.Item
				));
			ArrayAssert.ContainsByEquality(storedClasses, new object[] { storedClass });
		}

		public virtual void TestSystemInfo()
		{
			ISystemInfo systemInfo = _client1.SystemInfo();
			Assert.IsNotNull(systemInfo);
			Assert.IsGreater(1, systemInfo.TotalSize());
		}

		public virtual void TestVersion()
		{
			StoreItemToClient1AndCommit();
			Assert.IsGreater(1, _client1.Version());
		}

		private void AssertItemCount(IExtObjectContainer client, int count)
		{
			IQuery query = client.Query();
			query.Constrain(typeof(EmbeddedClientObjectContainerTestCase.Item));
			IObjectSet result = query.Execute();
			Assert.AreEqual(count, result.Count);
		}

		protected virtual EmbeddedClientObjectContainerTestCase.Item StoreItemToClient1AndCommit
			()
		{
			EmbeddedClientObjectContainerTestCase.Item storedItem = new EmbeddedClientObjectContainerTestCase.Item
				(OriginalName);
			_client1.Store(storedItem);
			_client1.Commit();
			return storedItem;
		}

		private EmbeddedClientObjectContainerTestCase.Item RetrieveItemFromClient2()
		{
			IQuery query = _client2.Query();
			query.Constrain(typeof(EmbeddedClientObjectContainerTestCase.Item));
			IObjectSet objectSet = query.Execute();
			EmbeddedClientObjectContainerTestCase.Item retrievedItem = (EmbeddedClientObjectContainerTestCase.Item
				)objectSet.Next();
			return retrievedItem;
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.Common.ObjectClass(typeof(EmbeddedClientObjectContainerTestCase.Item)).GenerateUUIDs
				(true);
			_server = (LocalObjectContainer)Db4oEmbedded.OpenFile(config, TempFile());
			_client1 = _server.OpenSession().Ext();
			_client2 = _server.OpenSession().Ext();
		}

		/// <exception cref="System.Exception"></exception>
		public override void TearDown()
		{
			_client1.Close();
			_client2.Close();
			_server.Close();
			base.TearDown();
		}
	}
}
