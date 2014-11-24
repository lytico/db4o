/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class CollectionHandlerImplTest : DrsTestCase
	{
		private CollectionHandlerImpl _collectionHandler;

		public virtual void TestVector()
		{
			ArrayList vector = new ArrayList();
			Assert.IsTrue(CollectionHandler().CanHandle(vector));
			Assert.IsTrue(CollectionHandler().CanHandleClass(ReplicationReflector().ForObject
				(vector)));
			Assert.IsTrue(CollectionHandler().CanHandleClass(typeof(ArrayList)));
		}

		public virtual void TestMap()
		{
			IDictionary map = new Hashtable();
			Assert.IsTrue(CollectionHandler().CanHandle(map));
			Assert.IsTrue(CollectionHandler().CanHandleClass(ReplicationReflector().ForObject
				(map)));
			Assert.IsTrue(CollectionHandler().CanHandleClass(typeof(IDictionary)));
		}

		public virtual void TestString()
		{
			string str = "abc";
			Assert.IsTrue(!CollectionHandler().CanHandle(str));
			Assert.IsTrue(!CollectionHandler().CanHandleClass(ReplicationReflector().ForObject
				(str)));
			Assert.IsTrue(!CollectionHandler().CanHandleClass(typeof(string)));
		}

		private Db4objects.Drs.Inside.ICollectionHandler CollectionHandler()
		{
			if (_collectionHandler == null)
			{
				_collectionHandler = new CollectionHandlerImpl(ReplicationReflector());
			}
			return _collectionHandler;
		}
	}
}
