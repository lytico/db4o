/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class EventCountTestCase : AbstractDb4oTestCase
	{
		private const int MaxChecks = 10;

		private const long WaitTime = 10;

		private EventCountTestCase.SafeCounter _activated = new EventCountTestCase.SafeCounter
			();

		private EventCountTestCase.SafeCounter _updated = new EventCountTestCase.SafeCounter
			();

		private EventCountTestCase.SafeCounter _deleted = new EventCountTestCase.SafeCounter
			();

		private EventCountTestCase.SafeCounter _created = new EventCountTestCase.SafeCounter
			();

		private EventCountTestCase.SafeCounter _committed = new EventCountTestCase.SafeCounter
			();

		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			new EventCountTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestEventRegistryCounts()
		{
			RegisterEventHandlers();
			for (int i = 0; i < 1000; i++)
			{
				EventCountTestCase.Item item = new EventCountTestCase.Item(i);
				Db().Store(item);
				Assert.IsTrue(Db().IsStored(item));
				if (((i + 1) % 100) == 0)
				{
					Db().Commit();
				}
			}
			AssertCount(_created, 1000, "created");
			AssertCount(_committed, 10, "commit");
			ReopenAndRegister();
			IObjectSet items = NewQuery(typeof(EventCountTestCase.Item)).Execute();
			Assert.AreEqual(1000, items.Count, "Wrong number of objects retrieved");
			while (items.HasNext())
			{
				EventCountTestCase.Item item = (EventCountTestCase.Item)items.Next();
				item._value++;
				Store(item);
			}
			AssertCount(_activated, 1000, "activated");
			AssertCount(_updated, 1000, "updated");
			items.Reset();
			while (items.HasNext())
			{
				object item = items.Next();
				Db().Delete(item);
				Assert.IsFalse(Db().IsStored(item));
			}
			AssertCount(_deleted, 1000, "deleted");
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertCount(EventCountTestCase.SafeCounter actual, int expected, string
			 name)
		{
			actual.AssertEquals(expected, MaxChecks);
		}

		/// <exception cref="System.Exception"></exception>
		private void ReopenAndRegister()
		{
			Reopen();
			RegisterEventHandlers();
		}

		private void RegisterEventHandlers()
		{
			IObjectContainer deletionEventSource = Db();
			if (Fixture() is IDb4oClientServerFixture)
			{
				IDb4oClientServerFixture clientServerFixture = (IDb4oClientServerFixture)Fixture(
					);
				deletionEventSource = clientServerFixture.Server().Ext().ObjectContainer();
			}
			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(Db());
			IEventRegistry deletionEventRegistry = EventRegistryFactory.ForObjectContainer(deletionEventSource
				);
			// No dedicated IncrementListener class due to sharpen event semantics
			deletionEventRegistry.Deleted += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_91(this).OnEvent);
			eventRegistry.Activated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_96(this).OnEvent);
			eventRegistry.Committed += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
				(new _IEventListener4_101(this).OnEvent);
			eventRegistry.Created += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_106(this).OnEvent);
			eventRegistry.Updated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_111(this).OnEvent);
		}

		private sealed class _IEventListener4_91
		{
			public _IEventListener4_91(EventCountTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing._deleted.Increment();
			}

			private readonly EventCountTestCase _enclosing;
		}

		private sealed class _IEventListener4_96
		{
			public _IEventListener4_96(EventCountTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing._activated.Increment();
			}

			private readonly EventCountTestCase _enclosing;
		}

		private sealed class _IEventListener4_101
		{
			public _IEventListener4_101(EventCountTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
			{
				this._enclosing._committed.Increment();
			}

			private readonly EventCountTestCase _enclosing;
		}

		private sealed class _IEventListener4_106
		{
			public _IEventListener4_106(EventCountTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing._created.Increment();
			}

			private readonly EventCountTestCase _enclosing;
		}

		private sealed class _IEventListener4_111
		{
			public _IEventListener4_111(EventCountTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing._updated.Increment();
			}

			private readonly EventCountTestCase _enclosing;
		}

		public class Item
		{
			public Item(int i)
			{
				_value = i;
			}

			public int _value;
		}

		private class SafeCounter
		{
			private int _value;

			private Lock4 _lock = new Lock4();

			public virtual void Increment()
			{
				_lock.Run(new _IClosure4_131(this));
			}

			private sealed class _IClosure4_131 : IClosure4
			{
				public _IClosure4_131(SafeCounter _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public object Run()
				{
					this._enclosing._value++;
					return null;
				}

				private readonly SafeCounter _enclosing;
			}

			public virtual void AssertEquals(int expected, int maxChecks)
			{
				IntByRef ret = new IntByRef();
				for (int checkCount = 0; checkCount < MaxChecks && ret.value != expected; checkCount
					++)
				{
					_lock.Run(new _IClosure4_140(this, expected, ret));
				}
				Assert.AreEqual(expected, ret.value);
			}

			private sealed class _IClosure4_140 : IClosure4
			{
				public _IClosure4_140(SafeCounter _enclosing, int expected, IntByRef ret)
				{
					this._enclosing = _enclosing;
					this.expected = expected;
					this.ret = ret;
				}

				public object Run()
				{
					if (this._enclosing._value != expected)
					{
						this._enclosing._lock.Snooze(EventCountTestCase.WaitTime);
					}
					ret.value = this._enclosing._value;
					return null;
				}

				private readonly SafeCounter _enclosing;

				private readonly int expected;

				private readonly IntByRef ret;
			}
		}
	}
}
