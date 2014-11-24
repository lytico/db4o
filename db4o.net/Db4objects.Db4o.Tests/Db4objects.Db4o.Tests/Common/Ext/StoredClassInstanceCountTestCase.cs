/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class StoredClassInstanceCountTestCase : AbstractDb4oTestCase
	{
		public class ItemA
		{
		}

		public class ItemB
		{
		}

		private const int CountA = 5;

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int idx = 0; idx < CountA; idx++)
			{
				Store(new StoredClassInstanceCountTestCase.ItemA());
			}
			Store(new StoredClassInstanceCountTestCase.ItemB());
		}

		public virtual void TestInstanceCount()
		{
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemA), CountA);
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemB), 1);
			Store(new StoredClassInstanceCountTestCase.ItemA());
			DeleteAll(typeof(StoredClassInstanceCountTestCase.ItemB));
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemA), CountA + 1);
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemB), 0);
		}

		public virtual void TestTransactionalInstanceCount()
		{
			if (!IsMultiSession())
			{
				return;
			}
			IExtObjectContainer otherClient = OpenNewSession();
			Store(new StoredClassInstanceCountTestCase.ItemA());
			DeleteAll(typeof(StoredClassInstanceCountTestCase.ItemB));
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemA), CountA 
				+ 1);
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 0);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemA), 
				CountA);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				1);
			Db().Commit();
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemA), CountA 
				+ 1);
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 0);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemA), 
				CountA + 1);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				0);
			otherClient.Commit();
			otherClient.Store(new StoredClassInstanceCountTestCase.ItemB());
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 0);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				1);
			otherClient.Commit();
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 1);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				1);
			otherClient.Close();
		}

		private void AssertInstanceCount(Type clazz, int expectedCount)
		{
			AssertInstanceCount(Db(), clazz, expectedCount);
		}

		private void AssertInstanceCount(IExtObjectContainer container, Type clazz, int expectedCount
			)
		{
			IStoredClass storedClazz = container.Ext().StoredClass(clazz);
			Assert.AreEqual(expectedCount, storedClazz.InstanceCount());
		}

		public static void Main(string[] args)
		{
			new StoredClassInstanceCountTestCase().RunAll();
		}
	}
}
