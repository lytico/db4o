/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class UpdateInCallbackThrowsTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new UpdateInCallbackThrowsTestCase().RunAll();
		}

		public class Item
		{
			public string _name;

			public UpdateInCallbackThrowsTestCase.Item _child;

			public Item(string name) : this(name, null)
			{
			}

			public Item(string name, UpdateInCallbackThrowsTestCase.Item child)
			{
				_name = name;
				_child = child;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UpdateInCallbackThrowsTestCase.Item("foo", new UpdateInCallbackThrowsTestCase.Item
				("bar")));
		}

		public virtual void TestUpdatingInDeletingCallback()
		{
			bool isNetworking = IsNetworking();
			EventRegistryFor(FileSession()).Deleting += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_42(isNetworking).OnEvent);
			Db().Delete(ItemByName("foo"));
			Assert.IsNotNull(ItemByName("bar*"));
		}

		private sealed class _IEventListener4_42
		{
			public _IEventListener4_42(bool isNetworking)
			{
				this.isNetworking = isNetworking;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				object obj = ((CancellableObjectEventArgs)args).Object;
				if (!(obj is UpdateInCallbackThrowsTestCase.Item))
				{
					return;
				}
				Transaction transaction = (Transaction)((CancellableObjectEventArgs)args).Transaction
					();
				IObjectContainer container = transaction.ObjectContainer();
				UpdateInCallbackThrowsTestCase.Item foo = (UpdateInCallbackThrowsTestCase.Item)obj;
				UpdateInCallbackThrowsTestCase.Item child = foo._child;
				if (isNetworking)
				{
					container.Activate(child, 1);
				}
				child._name += "*";
				container.Store(child);
			}

			private readonly bool isNetworking;
		}

		public virtual void TestReentrantUpdateAfterActivationThrows()
		{
			UpdateInCallbackThrowsTestCase.Item foo = ItemByName("foo");
			Db().Deactivate(foo);
			EventRegistry().Activated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_71(this).OnEvent);
			Db().Activate(foo, 1);
		}

		private sealed class _IEventListener4_71
		{
			public _IEventListener4_71(UpdateInCallbackThrowsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				object obj = ((ObjectInfoEventArgs)args).Object;
				if (!(obj is UpdateInCallbackThrowsTestCase.Item))
				{
					return;
				}
				UpdateInCallbackThrowsTestCase.Item item = (UpdateInCallbackThrowsTestCase.Item)obj;
				if (!item._name.Equals("foo"))
				{
					return;
				}
				Assert.Expect(typeof(Db4oIllegalStateException), new _ICodeBlock_83(this, item));
			}

			private sealed class _ICodeBlock_83 : ICodeBlock
			{
				public _ICodeBlock_83(_IEventListener4_71 _enclosing, UpdateInCallbackThrowsTestCase.Item
					 item)
				{
					this._enclosing = _enclosing;
					this.item = item;
				}

				public void Run()
				{
					item._child = new UpdateInCallbackThrowsTestCase.Item("baz");
					this._enclosing._enclosing.Store(item);
				}

				private readonly _IEventListener4_71 _enclosing;

				private readonly UpdateInCallbackThrowsTestCase.Item item;
			}

			private readonly UpdateInCallbackThrowsTestCase _enclosing;
		}

		private UpdateInCallbackThrowsTestCase.Item ItemByName(string name)
		{
			return ((UpdateInCallbackThrowsTestCase.Item)QueryItemsByName(name).Next());
		}

		public virtual void TestReentrantUpdateThrows()
		{
			ByRef updatedTriggered = new ByRef();
			updatedTriggered.value = false;
			IEventRegistry registry = EventRegistryFactory.ForObjectContainer(Db());
			registry.Updated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_102(this, updatedTriggered).OnEvent);
			IObjectSet items = QueryItemsByName("foo");
			Assert.AreEqual(1, items.Count);
			Assert.IsFalse((((bool)updatedTriggered.value)));
			Store(items.Next());
			Assert.IsTrue((((bool)updatedTriggered.value)));
		}

		private sealed class _IEventListener4_102
		{
			public _IEventListener4_102(UpdateInCallbackThrowsTestCase _enclosing, ByRef updatedTriggered
				)
			{
				this._enclosing = _enclosing;
				this.updatedTriggered = updatedTriggered;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				object obj = ((ObjectInfoEventArgs)args).Object;
				if (!(obj is UpdateInCallbackThrowsTestCase.Item))
				{
					return;
				}
				UpdateInCallbackThrowsTestCase.Item item = (UpdateInCallbackThrowsTestCase.Item)obj;
				if (!item._name.Equals("foo"))
				{
					return;
				}
				updatedTriggered.value = true;
				Assert.Expect(typeof(Db4oIllegalStateException), new _ICodeBlock_116(this, item));
			}

			private sealed class _ICodeBlock_116 : ICodeBlock
			{
				public _ICodeBlock_116(_IEventListener4_102 _enclosing, UpdateInCallbackThrowsTestCase.Item
					 item)
				{
					this._enclosing = _enclosing;
					this.item = item;
				}

				public void Run()
				{
					item._child = new UpdateInCallbackThrowsTestCase.Item("baz");
					this._enclosing._enclosing.Store(item);
				}

				private readonly _IEventListener4_102 _enclosing;

				private readonly UpdateInCallbackThrowsTestCase.Item item;
			}

			private readonly UpdateInCallbackThrowsTestCase _enclosing;

			private readonly ByRef updatedTriggered;
		}

		private IObjectSet QueryItemsByName(string name)
		{
			IQuery query = NewQuery(typeof(UpdateInCallbackThrowsTestCase.Item));
			query.Descend("_name").Constrain(name);
			return query.Execute();
		}
	}
}
