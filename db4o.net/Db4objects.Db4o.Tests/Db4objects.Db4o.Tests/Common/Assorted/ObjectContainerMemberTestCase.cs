/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class ObjectContainerMemberTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public IObjectContainer _typedObjectContainer;

			public object _untypedObjectContainer;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IEventRegistry eventRegistryFactory = EventRegistryFactory.ForObjectContainer(Db(
				));
			eventRegistryFactory.Creating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_23().OnEvent);
			ObjectContainerMemberTestCase.Item item = new ObjectContainerMemberTestCase.Item(
				);
			item._typedObjectContainer = Db();
			item._untypedObjectContainer = Db();
			Store(item);
			// Special case: Cascades activation to existing ObjectContainer member
			Db().QueryByExample(typeof(ObjectContainerMemberTestCase.Item)).Next();
		}

		private sealed class _IEventListener4_23
		{
			public _IEventListener4_23()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				object obj = ((CancellableObjectEventArgs)args).Object;
				Assert.IsFalse(obj is IObjectContainer);
			}
		}
	}
}
