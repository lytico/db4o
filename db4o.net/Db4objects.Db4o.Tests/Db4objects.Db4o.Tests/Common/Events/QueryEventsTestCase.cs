/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class QueryEventsTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int id;
		}

		private bool queryStarted;

		private bool queryFinished;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			IEventRegistry events = EventRegistry();
			events.QueryStarted += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
				(new _IEventListener4_23(this).OnEvent);
			events.QueryFinished += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
				(new _IEventListener4_29(this).OnEvent);
		}

		private sealed class _IEventListener4_23
		{
			public _IEventListener4_23(QueryEventsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
			{
				this._enclosing.queryStarted = true;
			}

			private readonly QueryEventsTestCase _enclosing;
		}

		private sealed class _IEventListener4_29
		{
			public _IEventListener4_29(QueryEventsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
			{
				this._enclosing.queryFinished = true;
			}

			private readonly QueryEventsTestCase _enclosing;
		}

		public virtual void TestSodaQueryLifeCycleEvents()
		{
			IQuery query = NewQuery(typeof(QueryEventsTestCase.Item));
			query.Descend("id").Constrain(42);
			query.Execute();
			AssertEventsRaised();
		}

		public virtual void TestClassOnlyQueryLifeCycleEvents()
		{
			AssertClassOnlyQuery(typeof(QueryEventsTestCase.Item));
		}

		public virtual void TestUntypedClassOnlyQueryLifeCycleEvents()
		{
			AssertClassOnlyQuery(typeof(object));
		}

		private void AssertClassOnlyQuery(Type clazz)
		{
			IQuery query = NewQuery(clazz);
			query.Execute();
			AssertEventsRaised();
		}

		private void AssertEventsRaised()
		{
			Assert.IsTrue(queryStarted);
			Assert.IsTrue(queryFinished);
		}
	}
}
