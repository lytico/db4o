/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class OwnCommittedCallbacksFixture
	{
		public interface IContainerFactory : ILabeled
		{
			IObjectContainer OpenClient();

			void Open();

			void Close();
		}

		public abstract class CommitAction : ILabeled
		{
			public virtual void CommitItem(object item, IObjectContainer clientA, IObjectContainer
				 clientB)
			{
				IObjectContainer client = SelectClient(clientA, clientB);
				client.Store(item);
				client.Commit();
			}

			public abstract bool SelectsFirstClient();

			protected abstract IObjectContainer SelectClient(IObjectContainer clientA, IObjectContainer
				 clientB);

			public abstract string Label();
		}

		public class NetworkingCSContainerFactory : OwnCommittedCallbacksFixture.IContainerFactory
		{
			private static readonly string Host = "localhost";

			private static readonly string User = "db4o";

			private static readonly string Pass = "db4o";

			private IObjectServer _server;

			public virtual void Open()
			{
				IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
				config.File.Storage = new MemoryStorage();
				_server = Db4oClientServer.OpenServer(config, string.Empty, Db4oClientServer.ArbitraryPort
					);
				_server.GrantAccess(User, Pass);
			}

			public virtual IObjectContainer OpenClient()
			{
				return Db4oClientServer.OpenClient(Host, _server.Ext().Port(), User, Pass);
			}

			public virtual void Close()
			{
				_server.Close();
			}

			public virtual string Label()
			{
				return "Networking C/S";
			}
		}

		public class EmbeddedCSContainerFactory : OwnCommittedCallbacksFixture.IContainerFactory
		{
			private IObjectServer _server;

			public virtual void Open()
			{
				IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
				config.File.Storage = new MemoryStorage();
				_server = Db4oClientServer.OpenServer(config, string.Empty, 0);
			}

			public virtual IObjectContainer OpenClient()
			{
				return _server.OpenClient();
			}

			public virtual void Close()
			{
				_server.Close();
			}

			public virtual string Label()
			{
				return "Embedded C/S";
			}
		}

		public class EmbeddedSessionContainerFactory : OwnCommittedCallbacksFixture.IContainerFactory
		{
			private IEmbeddedObjectContainer _server;

			public virtual void Open()
			{
				IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
				config.File.Storage = new MemoryStorage();
				_server = Db4oEmbedded.OpenFile(config, string.Empty);
			}

			public virtual IObjectContainer OpenClient()
			{
				return _server.Ext().OpenSession();
			}

			public virtual void Close()
			{
				_server.Close();
			}

			public virtual string Label()
			{
				return "Embedded Session";
			}
		}

		public class ClientACommitAction : OwnCommittedCallbacksFixture.CommitAction
		{
			protected override IObjectContainer SelectClient(IObjectContainer clientA, IObjectContainer
				 clientB)
			{
				return clientA;
			}

			public override bool SelectsFirstClient()
			{
				return true;
			}

			public override string Label()
			{
				return "Client A";
			}
		}

		public class ClientBCommitAction : OwnCommittedCallbacksFixture.CommitAction
		{
			protected override IObjectContainer SelectClient(IObjectContainer clientA, IObjectContainer
				 clientB)
			{
				return clientB;
			}

			public override bool SelectsFirstClient()
			{
				return false;
			}

			public override string Label()
			{
				return "Client B";
			}
		}

		public class OwnCommitCallbackFlaggedTestUnit : ITestCase
		{
			private const long Timeout = 1000;

			#if !CF
			/// <exception cref="System.Exception"></exception>
			public virtual void TestCommittedCallbacks()
			{
				Lock4 lockObject = new Lock4();
				BooleanByRef ownEvent = new BooleanByRef(false);
				BooleanByRef gotEvent = new BooleanByRef(false);
				BooleanByRef shallListen = new BooleanByRef(false);
				OwnCommittedCallbacksFixture.IContainerFactory factory = ((OwnCommittedCallbacksFixture.IContainerFactory
					)Factory.Value);
				OwnCommittedCallbacksFixture.CommitAction action = ((OwnCommittedCallbacksFixture.CommitAction
					)Action.Value);
				factory.Open();
				IObjectContainer clientA = factory.OpenClient();
				IObjectContainer clientB = factory.OpenClient();
				IEventRegistry registry = EventRegistryFactory.ForObjectContainer(clientA);
				registry.Committed += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
					(new _IEventListener4_153(shallListen, gotEvent, ownEvent, lockObject).OnEvent);
				lockObject.Run(new _IClosure4_170(shallListen, action, clientA, clientB, lockObject
					));
				shallListen.value = false;
				clientB.Close();
				clientA.Close();
				factory.Close();
				Assert.IsTrue(gotEvent.value);
				Assert.AreEqual(action.SelectsFirstClient(), ownEvent.value);
			}
			#endif // !CF

			private sealed class _IEventListener4_153
			{
				public _IEventListener4_153(BooleanByRef shallListen, BooleanByRef gotEvent, BooleanByRef
					 ownEvent, Lock4 lockObject)
				{
					this.shallListen = shallListen;
					this.gotEvent = gotEvent;
					this.ownEvent = ownEvent;
					this.lockObject = lockObject;
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
				{
					if (!shallListen.value)
					{
						return;
					}
					Assert.IsFalse(gotEvent.value);
					gotEvent.value = true;
					ownEvent.value = ((CommitEventArgs)args).IsOwnCommit();
					lockObject.Run(new _IClosure4_161(lockObject));
				}

				private sealed class _IClosure4_161 : IClosure4
				{
					public _IClosure4_161(Lock4 lockObject)
					{
						this.lockObject = lockObject;
					}

					public object Run()
					{
						lockObject.Awake();
						return null;
					}

					private readonly Lock4 lockObject;
				}

				private readonly BooleanByRef shallListen;

				private readonly BooleanByRef gotEvent;

				private readonly BooleanByRef ownEvent;

				private readonly Lock4 lockObject;
			}

			private sealed class _IClosure4_170 : IClosure4
			{
				public _IClosure4_170(BooleanByRef shallListen, OwnCommittedCallbacksFixture.CommitAction
					 action, IObjectContainer clientA, IObjectContainer clientB, Lock4 lockObject)
				{
					this.shallListen = shallListen;
					this.action = action;
					this.clientA = clientA;
					this.clientB = clientB;
					this.lockObject = lockObject;
				}

				public object Run()
				{
					shallListen.value = true;
					action.CommitItem(new OwnCommitCallbackFlaggedNetworkingTestSuite.Item(42), clientA
						, clientB);
					lockObject.Snooze(OwnCommittedCallbacksFixture.OwnCommitCallbackFlaggedTestUnit.Timeout
						);
					return null;
				}

				private readonly BooleanByRef shallListen;

				private readonly OwnCommittedCallbacksFixture.CommitAction action;

				private readonly IObjectContainer clientA;

				private readonly IObjectContainer clientB;

				private readonly Lock4 lockObject;
			}
		}

		public static readonly FixtureVariable Factory = FixtureVariable.NewInstance("mode"
			);

		public static readonly FixtureVariable Action = FixtureVariable.NewInstance("client"
			);
	}
}
#endif // !SILVERLIGHT
