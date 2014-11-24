/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerQueryEventsTestCase : ClientServerTestCaseBase
	{
		public virtual void TestConstrainedQuery()
		{
			AssertQueryEvents(new _IRunnable_16(this));
		}

		private sealed class _IRunnable_16 : IRunnable
		{
			public _IRunnable_16(ServerQueryEventsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				IQuery query = this._enclosing.NewQuery(typeof(ServerQueryEventsTestCase.Item));
				query.Descend("id").Constrain(42);
				query.Execute();
			}

			private readonly ServerQueryEventsTestCase _enclosing;
		}

		public virtual void TestClassOnlyQuery()
		{
			AssertQueryEvents(new _IRunnable_24(this));
		}

		private sealed class _IRunnable_24 : IRunnable
		{
			public _IRunnable_24(ServerQueryEventsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.NewQuery(typeof(ServerQueryEventsTestCase.Item)).Execute();
			}

			private readonly ServerQueryEventsTestCase _enclosing;
		}

		public virtual void TestGetAllQuery()
		{
			AssertQueryEvents(new _IRunnable_31(this));
		}

		private sealed class _IRunnable_31 : IRunnable
		{
			public _IRunnable_31(ServerQueryEventsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.NewQuery().Execute();
			}

			private readonly ServerQueryEventsTestCase _enclosing;
		}

		private void AssertQueryEvents(IRunnable query)
		{
			ArrayList events = new ArrayList();
			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(FileSession
				());
			eventRegistry.QueryStarted += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
				(new _IEventListener4_40(events).OnEvent);
			eventRegistry.QueryFinished += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
				(new _IEventListener4_45(events).OnEvent);
			query.Run();
			string[] expected = new string[] { QueryStarted, QueryFinished };
			Iterator4Assert.AreEqual(expected, Iterators.Iterator(events));
		}

		private sealed class _IEventListener4_40
		{
			public _IEventListener4_40(ArrayList events)
			{
				this.events = events;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
			{
				events.Add(ServerQueryEventsTestCase.QueryStarted);
			}

			private readonly ArrayList events;
		}

		private sealed class _IEventListener4_45
		{
			public _IEventListener4_45(ArrayList events)
			{
				this.events = events;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
			{
				events.Add(ServerQueryEventsTestCase.QueryFinished);
			}

			private readonly ArrayList events;
		}

		private static readonly string QueryFinished = "query finished";

		private static readonly string QueryStarted = "query started";

		public sealed class Item
		{
			public int id;
		}
	}
}
#endif // !SILVERLIGHT
