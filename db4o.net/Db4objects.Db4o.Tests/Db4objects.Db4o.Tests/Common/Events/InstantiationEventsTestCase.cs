/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class InstantiationEventsTestCase : EventsTestCaseBase
	{
		protected override void Configure(IConfiguration config)
		{
			config.ActivationDepth(0);
		}

		public virtual void TestInstantiationEvents()
		{
			EventsTestCaseBase.EventLog instantiatedLog = new EventsTestCaseBase.EventLog();
			EventRegistry().Instantiated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_20(this, instantiatedLog).OnEvent);
			RetrieveOnlyInstance(typeof(EventsTestCaseBase.Item));
			Assert.IsFalse(instantiatedLog.xing);
			Assert.IsTrue(instantiatedLog.xed);
		}

		private sealed class _IEventListener4_20
		{
			public _IEventListener4_20(InstantiationEventsTestCase _enclosing, EventsTestCaseBase.EventLog
				 instantiatedLog)
			{
				this._enclosing = _enclosing;
				this.instantiatedLog = instantiatedLog;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing.AssertClientTransaction(((ObjectInfoEventArgs)args));
				instantiatedLog.xed = true;
				object obj = ((ObjectInfoEventArgs)args).Object;
				ObjectReference objectReference = this._enclosing.Trans().ReferenceSystem().ReferenceForObject
					(obj);
				Assert.IsNotNull(objectReference);
				Assert.AreSame(objectReference, ((ObjectInfoEventArgs)args).Info);
			}

			private readonly InstantiationEventsTestCase _enclosing;

			private readonly EventsTestCaseBase.EventLog instantiatedLog;
		}
	}
}
