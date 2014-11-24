/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ActivationEventsTestCase : EventsTestCaseBase
	{
		protected override void Configure(IConfiguration config)
		{
			config.ActivationDepth(1);
		}

		public virtual void TestActivationEvents()
		{
			EventsTestCaseBase.EventLog activationLog = new EventsTestCaseBase.EventLog();
			EventRegistry().Activating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_19(this, activationLog).OnEvent);
			EventRegistry().Activated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_25(this, activationLog).OnEvent);
			RetrieveOnlyInstance(typeof(EventsTestCaseBase.Item));
			Assert.IsTrue(activationLog.xing);
			Assert.IsTrue(activationLog.xed);
		}

		private sealed class _IEventListener4_19
		{
			public _IEventListener4_19(ActivationEventsTestCase _enclosing, EventsTestCaseBase.EventLog
				 activationLog)
			{
				this._enclosing = _enclosing;
				this.activationLog = activationLog;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				this._enclosing.AssertClientTransaction(((CancellableObjectEventArgs)args));
				activationLog.xing = true;
			}

			private readonly ActivationEventsTestCase _enclosing;

			private readonly EventsTestCaseBase.EventLog activationLog;
		}

		private sealed class _IEventListener4_25
		{
			public _IEventListener4_25(ActivationEventsTestCase _enclosing, EventsTestCaseBase.EventLog
				 activationLog)
			{
				this._enclosing = _enclosing;
				this.activationLog = activationLog;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing.AssertClientTransaction(((ObjectInfoEventArgs)args));
				activationLog.xed = true;
			}

			private readonly ActivationEventsTestCase _enclosing;

			private readonly EventsTestCaseBase.EventLog activationLog;
		}
	}
}
