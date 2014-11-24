/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public class Db4oClientServerTestCase : TestWithTempFile
	{
		public virtual void TestClientServerApi()
		{
			IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
			IObjectServer server = Db4oClientServer.OpenServer(config, TempFile(), unchecked(
				(int)(0xdb40)));
			try
			{
				server.GrantAccess("user", "password");
				IClientConfiguration clientConfig = Db4oClientServer.NewClientConfiguration();
				IObjectContainer client1 = Db4oClientServer.OpenClient(clientConfig, "localhost", 
					unchecked((int)(0xdb40)), "user", "password");
				try
				{
				}
				finally
				{
					Assert.IsTrue(client1.Close());
				}
			}
			finally
			{
				Assert.IsTrue(server.Close());
			}
		}

		public virtual void TestConfigurationHierarchy()
		{
			Assert.IsInstanceOf(typeof(INetworkingConfigurationProvider), Db4oClientServer.NewClientConfiguration
				());
			Assert.IsInstanceOf(typeof(INetworkingConfigurationProvider), Db4oClientServer.NewServerConfiguration
				());
		}
	}
}
#endif // !SILVERLIGHT
