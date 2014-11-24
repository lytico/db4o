/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class EventDispatchersTestCase : AbstractInMemoryDb4oTestCase
	{
		public class NoCallbacks
		{
			// FIXME: implement all cases
		}

		public virtual void TestNoCallbacks()
		{
			IEventDispatcher dispatcher = EventDispatcherFor(typeof(EventDispatchersTestCase.NoCallbacks
				));
			Assert.AreSame(EventDispatchers.NullDispatcher, dispatcher);
		}

		private IEventDispatcher EventDispatcherFor(Type clazz)
		{
			return EventDispatchers.ForClass(Container(), ReflectClass(clazz));
		}

		public class SingleCallback
		{
			internal virtual bool ObjectCanDelete(IObjectContainer container)
			{
				return false;
			}
		}

		public class AllCallbacks
		{
			public virtual bool ObjectCanDelete(IObjectContainer container)
			{
				return false;
			}

			protected virtual void ObjectOnDelete(IObjectContainer container)
			{
			}

			private void ObjectOnActivate(IObjectContainer container)
			{
			}

			internal virtual void ObjectOnDeactivate(IObjectContainer container)
			{
			}

			private void ObjectOnNew(IObjectContainer container)
			{
			}

			private void ObjectOnUpdate(IObjectContainer container)
			{
			}

			private bool ObjectCanActivate(IObjectContainer container)
			{
				return false;
			}

			private bool ObjectCanDeactivate(IObjectContainer container)
			{
				return false;
			}

			private bool ObjectCanNew(IObjectContainer container)
			{
				return false;
			}

			private bool ObjectCanUpdate(IObjectContainer container)
			{
				return false;
			}
		}
	}
}
