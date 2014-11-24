/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class CallbackTrackingTestCase : AbstractDb4oTestCase, IOptOutAllButNetworkingCS
	{
		/// <exception cref="System.Exception"></exception>
		public virtual void TestStaticFields()
		{
			AssertQueryOnCallBack(typeof(CallbackTrackingTestCase.ItemWithStaticField));
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertQueryOnCallBack(Type classConstraint)
		{
			Lock4 Lock = new Lock4();
			Db().Store(new CallbackTrackingTestCase.Item(42));
			IEventRegistry eventRegistry = EventRegistry();
			eventRegistry.Committed += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
				(new _IEventListener4_30(classConstraint, Lock).OnEvent);
			lock (Lock)
			{
				Db().Commit();
				Lock.Snooze(5000);
			}
		}

		private sealed class _IEventListener4_30
		{
			public _IEventListener4_30(Type classConstraint, Lock4 Lock)
			{
				this.classConstraint = classConstraint;
				this.Lock = Lock;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
			{
				try
				{
					((CommitEventArgs)args).ObjectContainer().Query(classConstraint);
				}
				finally
				{
					Lock.Run(new _IClosure4_39(Lock));
				}
			}

			private sealed class _IClosure4_39 : IClosure4
			{
				public _IClosure4_39(Lock4 Lock)
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

			private readonly Type classConstraint;

			private readonly Lock4 Lock;
		}

		private class ItemWithStaticField
		{
			public static int i;
		}

		private class Item
		{
			public int id;

			public Item(int id)
			{
				this.id = id;
			}
		}
	}
}
