/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class EventArgsTransactionTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		public virtual void TestTransactionInEventArgs()
		{
			IEventRegistry factory = EventRegistryFactory.ForObjectContainer(Db());
			BooleanByRef called = new BooleanByRef();
			ObjectByRef foundTrans = new ObjectByRef();
			factory.Creating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_20(called, foundTrans).OnEvent);
			Db().Store(new EventArgsTransactionTestCase.Item());
			Db().Commit();
			Assert.IsTrue(called.value);
			Assert.AreSame(Trans(), foundTrans.value);
		}

		private sealed class _IEventListener4_20
		{
			public _IEventListener4_20(BooleanByRef called, ObjectByRef foundTrans)
			{
				this.called = called;
				this.foundTrans = foundTrans;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				called.value = true;
				foundTrans.value = ((TransactionalEventArgs)args).Transaction();
			}

			private readonly BooleanByRef called;

			private readonly ObjectByRef foundTrans;
		}

		public static void Main(string[] args)
		{
			new EventArgsTransactionTestCase().RunAll();
		}
	}
}
