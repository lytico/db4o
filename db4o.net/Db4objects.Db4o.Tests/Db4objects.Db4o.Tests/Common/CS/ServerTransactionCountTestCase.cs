/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerTransactionCountTestCase : ITestCase
	{
		private const int Timeout = 100;

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
			config.TimeoutServerSocket = Timeout;
			config.File.Storage = new MemoryStorage();
			ObjectServerImpl server = (ObjectServerImpl)Db4oClientServer.OpenServer(config, string.Empty
				, Db4oClientServer.ArbitraryPort);
			Thread.Sleep(Timeout * 2);
			Assert.AreEqual(0, server.TransactionCount());
			server.Close();
		}
	}
}
#endif // !SILVERLIGHT
