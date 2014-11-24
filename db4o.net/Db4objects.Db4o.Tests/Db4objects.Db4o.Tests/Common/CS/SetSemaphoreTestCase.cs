/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class SetSemaphoreTestCase : Db4oClientServerTestCase, IOptOutSolo
	{
		private static readonly string SemaphoreName = "hi";

		public static void Main(string[] args)
		{
			new SetSemaphoreTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Storage = new MemoryStorage();
		}

		public virtual void TestSemaphoreReentrancy()
		{
			IExtObjectContainer container = Db();
			Assert.IsTrue(container.SetSemaphore(SemaphoreName, 0));
			Assert.IsTrue(container.SetSemaphore(SemaphoreName, 0));
			container.ReleaseSemaphore(SemaphoreName);
		}

		public virtual void TestOwnedSemaphoreCannotBeTaken()
		{
			IExtObjectContainer client1 = OpenNewSession();
			try
			{
				Assert.IsTrue(Db().SetSemaphore(SemaphoreName, 0));
				Assert.IsFalse(client1.SetSemaphore(SemaphoreName, 0));
			}
			finally
			{
				client1.Close();
			}
		}

		public virtual void TestPreviouslyOwnedSemaphoreCannotBeTaken()
		{
			IExtObjectContainer client1 = OpenNewSession();
			try
			{
				Assert.IsTrue(Db().SetSemaphore(SemaphoreName, 0));
				Assert.IsFalse(client1.SetSemaphore(SemaphoreName, 0));
				Db().ReleaseSemaphore(SemaphoreName);
				EnsureMessageProcessed(Db());
				Assert.IsTrue(client1.SetSemaphore(SemaphoreName, 0));
				Assert.IsFalse(Db().SetSemaphore(SemaphoreName, 0));
			}
			finally
			{
				client1.Close();
			}
		}

		public virtual void TestClosingClientReleasesSemaphores()
		{
			IExtObjectContainer client1 = OpenNewSession();
			Assert.IsTrue(client1.SetSemaphore(SemaphoreName, 0));
			Assert.IsFalse(Db().SetSemaphore(SemaphoreName, 0));
			if (IsNetworking())
			{
				CloseConnectionInNetworkingCS(client1);
			}
			else
			{
				client1.Close();
			}
			Assert.IsTrue(Db().SetSemaphore(SemaphoreName, 0));
		}

		private void CloseConnectionInNetworkingCS(IExtObjectContainer client)
		{
			BooleanByRef eventWasRaised = new BooleanByRef();
			Lock4 clientDisconnectedLock = new Lock4();
			IObjectServerEvents serverEvents = (IObjectServerEvents)ClientServerFixture().Server
				();
			serverEvents.ClientDisconnected += new System.EventHandler<Db4objects.Db4o.Events.StringEventArgs>
				(new _IEventListener4_85(clientDisconnectedLock, eventWasRaised).OnEvent);
			clientDisconnectedLock.Run(new _IClosure4_96(client, clientDisconnectedLock));
			Assert.IsTrue(eventWasRaised.value, "ClientDisconnected event was not raised.");
		}

		private sealed class _IEventListener4_85
		{
			public _IEventListener4_85(Lock4 clientDisconnectedLock, BooleanByRef eventWasRaised
				)
			{
				this.clientDisconnectedLock = clientDisconnectedLock;
				this.eventWasRaised = eventWasRaised;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.StringEventArgs args)
			{
				clientDisconnectedLock.Run(new _IClosure4_87(eventWasRaised, clientDisconnectedLock
					));
			}

			private sealed class _IClosure4_87 : IClosure4
			{
				public _IClosure4_87(BooleanByRef eventWasRaised, Lock4 clientDisconnectedLock)
				{
					this.eventWasRaised = eventWasRaised;
					this.clientDisconnectedLock = clientDisconnectedLock;
				}

				public object Run()
				{
					eventWasRaised.value = true;
					clientDisconnectedLock.Awake();
					return null;
				}

				private readonly BooleanByRef eventWasRaised;

				private readonly Lock4 clientDisconnectedLock;
			}

			private readonly Lock4 clientDisconnectedLock;

			private readonly BooleanByRef eventWasRaised;
		}

		private sealed class _IClosure4_96 : IClosure4
		{
			public _IClosure4_96(IExtObjectContainer client, Lock4 clientDisconnectedLock)
			{
				this.client = client;
				this.clientDisconnectedLock = clientDisconnectedLock;
			}

			public object Run()
			{
				client.Close();
				clientDisconnectedLock.Snooze(30000);
				return null;
			}

			private readonly IExtObjectContainer client;

			private readonly Lock4 clientDisconnectedLock;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestMultipleThreads()
		{
			IExtObjectContainer[] clients = new IExtObjectContainer[5];
			clients[0] = Db();
			for (int i = 1; i < clients.Length; i++)
			{
				clients[i] = OpenNewSession();
			}
			Assert.IsTrue(clients[1].SetSemaphore(SemaphoreName, 50));
			Thread[] threads = new Thread[clients.Length];
			for (int i = 0; i < clients.Length; i++)
			{
				threads[i] = StartGetAndReleaseThread(clients[i]);
			}
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i].Join();
			}
			for (int i = 0; i < threads.Length; i++)
			{
				EnsureMessageProcessed(clients[i]);
			}
			Assert.IsTrue(clients[0].SetSemaphore(SemaphoreName, 0));
			clients[0].Close();
			threads[2] = StartGetAndReleaseThread(clients[2]);
			threads[1] = StartGetAndReleaseThread(clients[1]);
			threads[1].Join();
			threads[2].Join();
			for (int i = 1; i < clients.Length - 1; i++)
			{
				clients[i].Close();
			}
			clients[4].SetSemaphore(SemaphoreName, 1000);
			clients[4].Close();
		}

		private Thread StartGetAndReleaseThread(IExtObjectContainer client)
		{
			Thread t = new Thread(new SetSemaphoreTestCase.GetAndRelease(client), "SetSemaphoreTestCase.startGetAndReleaseThread"
				);
			t.Start();
			return t;
		}

		private static void EnsureMessageProcessed(IExtObjectContainer client)
		{
			client.Commit();
		}

		internal class GetAndRelease : IRunnable
		{
			private IExtObjectContainer _client;

			public GetAndRelease(IExtObjectContainer client)
			{
				_client = client;
			}

			public virtual void Run()
			{
				Assert.IsTrue(_client.SetSemaphore(SemaphoreName, 50000));
				EnsureMessageProcessed(_client);
				_client.ReleaseSemaphore(SemaphoreName);
			}
		}
	}
}
#endif // !SILVERLIGHT
