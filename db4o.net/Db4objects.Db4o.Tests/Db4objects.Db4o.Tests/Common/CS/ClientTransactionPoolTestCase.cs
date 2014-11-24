/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ClientTransactionPoolTestCase : ITestLifeCycle
	{
		public virtual void TestPool()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Storage = new MemoryStorage();
			LocalObjectContainer db = (LocalObjectContainer)Db4oFactory.OpenFile(config, ClientTransactionTestUtil
				.MainfileName);
			ClientTransactionPool pool = new ClientTransactionPool(db);
			try
			{
				Assert.AreEqual(0, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
				Transaction trans1 = pool.Acquire(ClientTransactionTestUtil.MainfileName);
				Assert.AreEqual(db, trans1.Container());
				Assert.AreEqual(1, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
				Transaction trans2 = pool.Acquire(ClientTransactionTestUtil.FilenameA);
				Assert.AreNotEqual(db, trans2.Container());
				Assert.AreEqual(2, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				Transaction trans3 = pool.Acquire(ClientTransactionTestUtil.FilenameA);
				Assert.AreEqual(trans2.Container(), trans3.Container());
				Assert.AreEqual(3, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				pool.Release(ShutdownMode.Normal, trans3, true);
				Assert.AreEqual(2, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				pool.Release(ShutdownMode.Normal, trans2, true);
				Assert.AreEqual(1, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
			}
			finally
			{
				Assert.IsFalse(db.IsClosed());
				Assert.IsFalse(pool.IsClosed());
				pool.Close();
				Assert.IsTrue(db.IsClosed());
				Assert.IsTrue(pool.IsClosed());
				Assert.AreEqual(0, pool.OpenFileCount());
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			ClientTransactionTestUtil.DeleteFiles();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			ClientTransactionTestUtil.DeleteFiles();
		}
	}
}
#endif // !SILVERLIGHT
