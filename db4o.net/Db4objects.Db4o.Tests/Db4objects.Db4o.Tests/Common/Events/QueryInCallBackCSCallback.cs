/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	/// <author>roman.stoffel@gamlor.info</author>
	public class QueryInCallBackCSCallback : ITestCase, ITestLifeCycle
	{
		private IObjectServer server;

		public const int Port = 1337;

		public static readonly string UsernameAndPassword = "sa";

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(QueryInCallBackCSCallback)).Run();
		}

		public virtual void TestCreatingCallbackUnknownMetaData()
		{
			IObjectContainer client = OpenClient();
			AddListenerTo(client);
			client.Store(new QueryInCallBackCSCallback.ToStore());
		}

		private void AddListenerTo(IObjectContainer client)
		{
			EventRegistryFactory.ForObjectContainer(client).Creating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_42().OnEvent);
		}

		private sealed class _IEventListener4_42
		{
			public _IEventListener4_42()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 objectInfo)
			{
				IObjectContainer container = ((CancellableObjectEventArgs)objectInfo).ObjectContainer
					();
				// this crashes if the MetaInfoWithEnum-class is unknown!
				container.Query(typeof(QueryInCallBackCSCallback.MetaInfo));
			}
		}

		public virtual void TestCreatingCallbackWithKnownMetaData()
		{
			IObjectContainer client = OpenClient();
			EnsureMetaDataAreKnown(client);
			AddListenerTo(client);
			client.Store(new QueryInCallBackCSCallback.ToStore());
		}

		public virtual void TestUpdatingCallback()
		{
			IObjectContainer client = OpenClient();
			AddListenerTo(client);
			client.Store(new QueryInCallBackCSCallback.ToStore());
		}

		public virtual void TestActivating()
		{
			IObjectContainer client = OpenClient();
			AddListenerTo(client);
			client.Store(new QueryInCallBackCSCallback.ToStore());
		}

		private void EnsureMetaDataAreKnown(IObjectContainer client)
		{
			client.Store(new QueryInCallBackCSCallback.MetaInfo());
			client.Store(new QueryInCallBackCSCallback.ToStore());
			client.Commit();
		}

		private IObjectContainer OpenClient()
		{
			return Db4oClientServer.OpenClient("localhost", Port, UsernameAndPassword, UsernameAndPassword
				);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
			config.File.Storage = new MemoryStorage();
			this.server = Db4oClientServer.OpenServer(config, "InMemory:File", Port);
			this.server.GrantAccess(UsernameAndPassword, UsernameAndPassword);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			server.Close();
		}

		internal class ToStore
		{
		}

		internal class MetaInfo
		{
			internal int data;
		}
	}
}
#endif // !SILVERLIGHT
