/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class DeleteOnDeletingCallbackTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		public class RootItem
		{
			public DeleteOnDeletingCallbackTestCase.Item child;

			public RootItem()
			{
			}

			public virtual void ObjectOnDelete(IObjectContainer container)
			{
				container.Delete(child);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new DeleteOnDeletingCallbackTestCase.RootItem());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			BooleanByRef disconnected = new BooleanByRef();
			Lock4 Lock = new Lock4();
			if (IsNetworking())
			{
				IDb4oClientServerFixture clientServerFixture = (IDb4oClientServerFixture)Fixture(
					);
				IObjectServerEvents objectServerEvents = (IObjectServerEvents)clientServerFixture
					.Server();
				objectServerEvents.ClientDisconnected += new System.EventHandler<Db4objects.Db4o.Events.StringEventArgs>
					(new _IEventListener4_46(Lock, disconnected).OnEvent);
			}
			DeleteOnDeletingCallbackTestCase.RootItem root = ((DeleteOnDeletingCallbackTestCase.RootItem
				)RetrieveOnlyInstance(typeof(DeleteOnDeletingCallbackTestCase.RootItem)));
			root.child = new DeleteOnDeletingCallbackTestCase.Item();
			Db().Store(root);
			Db().Delete(root);
			Reopen();
			if (IsNetworking())
			{
				Lock.Run(new _IClosure4_63(disconnected, Lock));
			}
			AssertClassIndexIsEmpty();
		}

		private sealed class _IEventListener4_46
		{
			public _IEventListener4_46(Lock4 Lock, BooleanByRef disconnected)
			{
				this.Lock = Lock;
				this.disconnected = disconnected;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.StringEventArgs args)
			{
				Lock.Run(new _IClosure4_47(disconnected, Lock));
			}

			private sealed class _IClosure4_47 : IClosure4
			{
				public _IClosure4_47(BooleanByRef disconnected, Lock4 Lock)
				{
					this.disconnected = disconnected;
					this.Lock = Lock;
				}

				public object Run()
				{
					disconnected.value = true;
					Lock.Awake();
					return null;
				}

				private readonly BooleanByRef disconnected;

				private readonly Lock4 Lock;
			}

			private readonly Lock4 Lock;

			private readonly BooleanByRef disconnected;
		}

		private sealed class _IClosure4_63 : IClosure4
		{
			public _IClosure4_63(BooleanByRef disconnected, Lock4 Lock)
			{
				this.disconnected = disconnected;
				this.Lock = Lock;
			}

			public object Run()
			{
				if (!disconnected.value)
				{
					Lock.Snooze(1000000);
				}
				return null;
			}

			private readonly BooleanByRef disconnected;

			private readonly Lock4 Lock;
		}

		private void AssertClassIndexIsEmpty()
		{
			Iterator4Assert.AreEqual(new object[0], GetAllIds());
		}

		private IIntIterator4 GetAllIds()
		{
			return FileSession().GetAll(FileSession().Transaction, QueryEvaluationMode.Immediate
				).IterateIDs();
		}
	}
}
#endif // !SILVERLIGHT
