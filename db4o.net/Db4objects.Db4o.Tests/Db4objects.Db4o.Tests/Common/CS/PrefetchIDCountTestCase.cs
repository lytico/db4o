/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class PrefetchIDCountTestCase : TestWithTempFile
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(PrefetchIDCountTestCase)).Run();
		}

		private const int PrefetchIdCount = 100;

		private static readonly string User = "db4o";

		private static readonly string Password = "db4o";

		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			ObjectServerImpl server = (ObjectServerImpl)Db4oClientServer.OpenServer(TempFile(
				), Db4oClientServer.ArbitraryPort);
			Lock4 Lock = new Lock4();
			server.ClientDisconnected += new System.EventHandler<Db4objects.Db4o.Events.StringEventArgs>
				(new _IEventListener4_39(Lock).OnEvent);
			server.GrantAccess(User, Password);
			IObjectContainer client = OpenClient(server.Port());
			ServerMessageDispatcherImpl msgDispatcher = FirstMessageDispatcherFor(server);
			Transaction transaction = msgDispatcher.Transaction();
			ITransactionalIdSystem idSystem = transaction.IdSystem();
			int prefetchedID = idSystem.PrefetchID();
			Assert.IsGreater(0, prefetchedID);
			Lock.Run(new _IClosure4_58(client, Lock, idSystem, prefetchedID));
			// This wont work with the PointerBasedIdSystem
			server.Close();
		}

		private sealed class _IEventListener4_39
		{
			public _IEventListener4_39(Lock4 Lock)
			{
				this.Lock = Lock;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.StringEventArgs args)
			{
				Lock.Run(new _IClosure4_40(Lock));
			}

			private sealed class _IClosure4_40 : IClosure4
			{
				public _IClosure4_40(Lock4 Lock)
				{
					this.Lock = Lock;
				}

				public object Run()
				{
					Lock.Awake();
					return null;
				}

				private readonly Lock4 Lock;
			}

			private readonly Lock4 Lock;
		}

		private sealed class _IClosure4_58 : IClosure4
		{
			public _IClosure4_58(IObjectContainer client, Lock4 Lock, ITransactionalIdSystem 
				idSystem, int prefetchedID)
			{
				this.client = client;
				this.Lock = Lock;
				this.idSystem = idSystem;
				this.prefetchedID = prefetchedID;
			}

			public object Run()
			{
				client.Close();
				Lock.Snooze(100000);
				Assert.Expect(typeof(InvalidIDException), new _ICodeBlock_63(idSystem, prefetchedID
					));
				return null;
			}

			private sealed class _ICodeBlock_63 : ICodeBlock
			{
				public _ICodeBlock_63(ITransactionalIdSystem idSystem, int prefetchedID)
				{
					this.idSystem = idSystem;
					this.prefetchedID = prefetchedID;
				}

				/// <exception cref="System.Exception"></exception>
				public void Run()
				{
					idSystem.CommittedSlot(prefetchedID);
				}

				private readonly ITransactionalIdSystem idSystem;

				private readonly int prefetchedID;
			}

			private readonly IObjectContainer client;

			private readonly Lock4 Lock;

			private readonly ITransactionalIdSystem idSystem;

			private readonly int prefetchedID;
		}

		private ServerMessageDispatcherImpl FirstMessageDispatcherFor(ObjectServerImpl server
			)
		{
			IEnumerator dispatchers = server.IterateDispatchers();
			Assert.IsTrue(dispatchers.MoveNext());
			ServerMessageDispatcherImpl msgDispatcher = (ServerMessageDispatcherImpl)dispatchers
				.Current;
			return msgDispatcher;
		}

		private IObjectContainer OpenClient(int port)
		{
			IClientConfiguration config = Db4oClientServer.NewClientConfiguration();
			config.PrefetchIDCount = PrefetchIdCount;
			return Db4oClientServer.OpenClient(config, "localhost", port, User, Password);
		}

		public class DebugFreespaceManager : AbstractFreespaceManager
		{
			public DebugFreespaceManager(LocalObjectContainer file) : base(null, 0, 0)
			{
			}

			private readonly IList _freedSlots = new ArrayList();

			public virtual bool WasFreed(int id)
			{
				return _freedSlots.Contains(id);
			}

			public override Slot AllocateSlot(int length)
			{
				return null;
			}

			public override Slot AllocateSafeSlot(int length)
			{
				return null;
			}

			public override void BeginCommit()
			{
			}

			// TODO Auto-generated method stub
			public override void Commit()
			{
			}

			// TODO Auto-generated method stub
			public override void EndCommit()
			{
			}

			// TODO Auto-generated method stub
			public override void Free(Slot slot)
			{
				_freedSlots.Add(slot.Address());
			}

			public override void FreeSelf()
			{
			}

			// TODO Auto-generated method stub
			public override void FreeSafeSlot(Slot slot)
			{
			}

			// TODO Auto-generated method stub
			public override void Listener(IFreespaceListener listener)
			{
			}

			// TODO Auto-generated method stub
			public override void MigrateTo(IFreespaceManager fm)
			{
			}

			// TODO Auto-generated method stub
			public override int SlotCount()
			{
				// TODO Auto-generated method stub
				return 0;
			}

			public override void Start(int id)
			{
			}

			// TODO Auto-generated method stub
			public override byte SystemType()
			{
				// TODO Auto-generated method stub
				return 0;
			}

			public override int TotalFreespace()
			{
				// TODO Auto-generated method stub
				return 0;
			}

			public override void Traverse(IVisitor4 visitor)
			{
			}

			// TODO Auto-generated method stub
			public override void Write(LocalObjectContainer container)
			{
			}

			public override bool IsStarted()
			{
				return false;
			}

			public override Slot AllocateTransactionLogSlot(int length)
			{
				// TODO Auto-generated method stub
				return null;
			}

			public override void Read(LocalObjectContainer container, Slot slot)
			{
			}
			// TODO Auto-generated method stub
		}
	}
}
#endif // !SILVERLIGHT
