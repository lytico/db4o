/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ExceptionPropagationInEventsTestUnit : EventsTestCaseBase
	{
		public ExceptionPropagationInEventsTestUnit()
		{
			_eventFirer["insert"] = NewObjectInserter();
			_eventFirer["query"] = NewQueryRunner();
			_eventFirer["update"] = NewObjectUpdater();
			_eventFirer["delete"] = NewObjectDeleter();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new EventsTestCaseBase.Item(1));
			Store(new EventsTestCaseBase.Item(2));
		}

		public virtual void TestEvents()
		{
			EventInfo @event = EventToTest();
			if (IsEmbedded())
			{
				return;
			}
			if (IsNetworking() && !@event.IsClientServerEvent())
			{
				return;
			}
			AssertEventThrows(@event.EventFirerName(), ((ICodeBlock)_eventFirer[@event.EventFirerName
				()]), @event.ListenerSetter());
		}

		private EventInfo EventToTest()
		{
			return (EventInfo)ExceptionPropagationInEventsTestVariables.EventSelector.Value;
		}

		private void AssertEventThrows(string eventName, ICodeBlock codeBlock, IProcedure4
			 listenerSetter)
		{
			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(Db());
			listenerSetter.Apply(eventRegistry);
			Assert.Expect(typeof(EventException), typeof(NotImplementedException), codeBlock, 
				eventName);
		}

		private ICodeBlock NewObjectUpdater()
		{
			return new _ICodeBlock_50(this);
		}

		private sealed class _ICodeBlock_50 : ICodeBlock
		{
			public _ICodeBlock_50(ExceptionPropagationInEventsTestUnit _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				EventsTestCaseBase.Item item = this._enclosing.RetrieveItem(1);
				item.id = 10;
				this._enclosing.Db().Store(item);
				this._enclosing.Db().Commit();
			}

			private readonly ExceptionPropagationInEventsTestUnit _enclosing;
		}

		private ICodeBlock NewObjectDeleter()
		{
			return new _ICodeBlock_64(this);
		}

		private sealed class _ICodeBlock_64 : ICodeBlock
		{
			public _ICodeBlock_64(ExceptionPropagationInEventsTestUnit _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Delete(this._enclosing.RetrieveItem(1));
				this._enclosing.Db().Commit();
			}

			private readonly ExceptionPropagationInEventsTestUnit _enclosing;
		}

		private ICodeBlock NewQueryRunner()
		{
			return new _ICodeBlock_73(this);
		}

		private sealed class _ICodeBlock_73 : ICodeBlock
		{
			public _ICodeBlock_73(ExceptionPropagationInEventsTestUnit _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.RetrieveItem(1);
			}

			private readonly ExceptionPropagationInEventsTestUnit _enclosing;
		}

		private ICodeBlock NewObjectInserter()
		{
			return new _ICodeBlock_81(this);
		}

		private sealed class _ICodeBlock_81 : ICodeBlock
		{
			public _ICodeBlock_81(ExceptionPropagationInEventsTestUnit _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Store(new EventsTestCaseBase.Item());
				this._enclosing.Db().Commit();
			}

			private readonly ExceptionPropagationInEventsTestUnit _enclosing;
		}

		private EventsTestCaseBase.Item RetrieveItem(int id)
		{
			IQuery query = NewQuery(typeof(EventsTestCaseBase.Item));
			query.Descend("id").Constrain(id);
			IObjectSet results = query.Execute();
			Assert.AreEqual(1, results.Count);
			EventsTestCaseBase.Item found = ((EventsTestCaseBase.Item)results.Next());
			Assert.AreEqual(id, found.id);
			return found;
		}

		private Hashtable _eventFirer = new Hashtable();
	}
}
