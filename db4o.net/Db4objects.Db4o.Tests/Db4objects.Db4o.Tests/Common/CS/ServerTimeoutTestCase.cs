/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	/// <exclude></exclude>
	public class ServerTimeoutTestCase : ClientServerTestCaseBase
	{
		public static void Main(string[] arguments)
		{
			new ServerTimeoutTestCase().RunNetworking();
		}

		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().TimeoutClientSocket(1);
			config.ClientServer().TimeoutServerSocket(1);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void _test()
		{
			ObjectServerImpl serverImpl = (ObjectServerImpl)ClientServerFixture().Server();
			IEnumerator iter = serverImpl.IterateDispatchers();
			iter.MoveNext();
			IServerMessageDispatcher serverDispatcher = (IServerMessageDispatcher)iter.Current;
			IClientMessageDispatcher clientDispatcher = ((ClientObjectContainer)Db()).MessageDispatcher
				();
			clientDispatcher.Close();
			Runtime4.Sleep(1000);
			Assert.IsFalse(serverDispatcher.IsMessageDispatcherAlive());
		}
	}
}
#endif // !SILVERLIGHT
