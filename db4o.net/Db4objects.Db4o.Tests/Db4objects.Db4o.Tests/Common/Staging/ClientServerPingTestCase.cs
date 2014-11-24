/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.CS;
using Db4objects.Db4o.Tests.Common.Staging;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	/// <exclude></exclude>
	public class ClientServerPingTestCase : ClientServerTestCaseBase
	{
		private const int ItemCount = 100;

		public static void Main(string[] arguments)
		{
			new ClientServerPingTestCase().RunNetworking();
		}

		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().BatchMessages(false);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			if (IsEmbedded())
			{
				// This test really doesn't make sense for MTOC, there
				// is no client to ping.
				return;
			}
			IServerMessageDispatcher dispatcher = ServerDispatcher();
			ClientServerPingTestCase.PingThread pingThread = new ClientServerPingTestCase.PingThread
				(dispatcher);
			pingThread.Start();
			for (int i = 0; i < ItemCount; i++)
			{
				ClientServerPingTestCase.Item item = new ClientServerPingTestCase.Item(i);
				Store(item);
			}
			Assert.AreEqual(ItemCount, Db().QueryByExample(typeof(ClientServerPingTestCase.Item
				)).Count);
			pingThread.Close();
		}

		public class Item
		{
			public int data;

			public Item(int i)
			{
				data = i;
			}
		}

		internal class PingThread : Thread
		{
			internal IServerMessageDispatcher _dispatcher;

			internal bool _stop;

			private readonly object Lock = new object();

			public PingThread(IServerMessageDispatcher dispatcher)
			{
				_dispatcher = dispatcher;
			}

			public virtual void Close()
			{
				lock (Lock)
				{
					_stop = true;
				}
			}

			private bool NotStopped()
			{
				lock (Lock)
				{
					return !_stop;
				}
			}

			public override void Run()
			{
				while (NotStopped())
				{
					_dispatcher.Write(Msg.Ping);
					Runtime4.Sleep(1);
				}
			}
		}
	}
}
#endif // !SILVERLIGHT
