/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Api;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ObjectServerTestCase : TestWithTempFile
	{
		private IExtObjectServer server;

		private string fileName;

		public virtual void TestClientCount()
		{
			AssertClientCount(0);
			IObjectContainer client1 = OpenClient();
			try
			{
				AssertClientCount(1);
				IObjectContainer client2 = OpenClient();
				try
				{
					AssertClientCount(2);
				}
				finally
				{
					client2.Close();
				}
			}
			finally
			{
				client1.Close();
			}
		}

		// closing is asynchronous, relying on completion is hard
		// That's why there is no test here. 
		// ClientProcessesTestCase tests closing.
		public virtual void TestClientDisconnectedEvent()
		{
			ClientObjectContainer client = (ClientObjectContainer)OpenClient();
			string clientName = client.UserName;
			BooleanByRef eventRaised = new BooleanByRef();
			IObjectServerEvents events = (IObjectServerEvents)server;
			Lock4 Lock = new Lock4();
			events.ClientDisconnected += new System.EventHandler<Db4objects.Db4o.Events.StringEventArgs>
				(new _IEventListener4_51(clientName, eventRaised, Lock).OnEvent);
			Lock.Run(new _IClosure4_58(client, eventRaised, Lock));
		}

		private sealed class _IEventListener4_51
		{
			public _IEventListener4_51(string clientName, BooleanByRef eventRaised, Lock4 Lock
				)
			{
				this.clientName = clientName;
				this.eventRaised = eventRaised;
				this.Lock = Lock;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.StringEventArgs args)
			{
				Assert.AreEqual(clientName, ((StringEventArgs)args).Message);
				eventRaised.value = true;
				Lock.Awake();
			}

			private readonly string clientName;

			private readonly BooleanByRef eventRaised;

			private readonly Lock4 Lock;
		}

		private sealed class _IClosure4_58 : IClosure4
		{
			public _IClosure4_58(ClientObjectContainer client, BooleanByRef eventRaised, Lock4
				 Lock)
			{
				this.client = client;
				this.eventRaised = eventRaised;
				this.Lock = Lock;
			}

			public object Run()
			{
				client.Close();
				long startTime = Runtime.CurrentTimeMillis();
				long currentTime = startTime;
				int timeOut = 1000;
				long timePassed = currentTime - startTime;
				while (timePassed < timeOut && !eventRaised.value)
				{
					Lock.Snooze(timeOut - timePassed);
					currentTime = Runtime.CurrentTimeMillis();
					timePassed = currentTime - startTime;
				}
				Assert.IsTrue(eventRaised.value);
				return null;
			}

			private readonly ClientObjectContainer client;

			private readonly BooleanByRef eventRaised;

			private readonly Lock4 Lock;
		}

		public virtual void TestClientConnectedEvent()
		{
			ArrayList connections = new ArrayList();
			IObjectServerEvents events = (IObjectServerEvents)server;
			events.ClientConnected += new System.EventHandler<ClientConnectionEventArgs>(new 
				_IEventListener4_83(connections).OnEvent);
			IObjectContainer client = OpenClient();
			try
			{
				Assert.AreEqual(1, connections.Count);
				Iterator4Assert.AreEqual(ServerMessageDispatchers(), Iterators.Iterator(connections
					));
			}
			finally
			{
				client.Close();
			}
		}

		private sealed class _IEventListener4_83
		{
			public _IEventListener4_83(ArrayList connections)
			{
				this.connections = connections;
			}

			public void OnEvent(object sender, ClientConnectionEventArgs args)
			{
				connections.Add(((ClientConnectionEventArgs)args).Connection);
			}

			private readonly ArrayList connections;
		}

		public virtual void TestServerClosedEvent()
		{
			BooleanByRef receivedEvent = new BooleanByRef(false);
			IObjectServerEvents events = (IObjectServerEvents)server;
			events.Closed += new System.EventHandler<ServerClosedEventArgs>(new _IEventListener4_101
				(receivedEvent).OnEvent);
			server.Close();
			Assert.IsTrue(receivedEvent.value);
		}

		private sealed class _IEventListener4_101
		{
			public _IEventListener4_101(BooleanByRef receivedEvent)
			{
				this.receivedEvent = receivedEvent;
			}

			public void OnEvent(object sender, ServerClosedEventArgs args)
			{
				receivedEvent.value = true;
			}

			private readonly BooleanByRef receivedEvent;
		}

		private IEnumerator ServerMessageDispatchers()
		{
			return ((ObjectServerImpl)server).IterateDispatchers();
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			fileName = TempFile();
			server = Db4oClientServer.OpenServer(fileName, -1).Ext();
			server.GrantAccess(Credentials(), Credentials());
		}

		/// <exception cref="System.Exception"></exception>
		public override void TearDown()
		{
			server.Close();
			base.TearDown();
		}

		private IObjectContainer OpenClient()
		{
			return Db4oClientServer.OpenClient("localhost", Port(), Credentials(), Credentials
				());
		}

		private void AssertClientCount(int count)
		{
			Assert.AreEqual(count, server.ClientCount());
		}

		private int Port()
		{
			return server.Port();
		}

		private string Credentials()
		{
			return "DB4O";
		}
	}
}
#endif // !SILVERLIGHT
