/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ClassRegistrationEventsTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
		}

		private class EventFlag
		{
			public bool eventOccurred = false;
		}

		public virtual void TestClassRegistrationEvents()
		{
			ClassRegistrationEventsTestCase.EventFlag eventFlag = new ClassRegistrationEventsTestCase.EventFlag
				();
			IEventRegistry registry = EventRegistryFactory.ForObjectContainer(Db());
			registry.ClassRegistered += new System.EventHandler<Db4objects.Db4o.Events.ClassEventArgs>
				(new _IEventListener4_23(eventFlag).OnEvent);
			Store(new ClassRegistrationEventsTestCase.Data());
			Assert.IsTrue(eventFlag.eventOccurred);
		}

		private sealed class _IEventListener4_23
		{
			public _IEventListener4_23(ClassRegistrationEventsTestCase.EventFlag eventFlag)
			{
				this.eventFlag = eventFlag;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ClassEventArgs args)
			{
				ClassEventArgs classEventArgs = (ClassEventArgs)args;
				Assert.AreEqual(typeof(ClassRegistrationEventsTestCase.Data).FullName, CrossPlatformServices
					.SimpleName(classEventArgs.ClassMetadata().GetName()));
				eventFlag.eventOccurred = true;
			}

			private readonly ClassRegistrationEventsTestCase.EventFlag eventFlag;
		}
	}
}
