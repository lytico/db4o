/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class EventsTestCaseBase : AbstractDb4oTestCase
	{
		public sealed class Item
		{
			public int id;

			public Item()
			{
			}

			public Item(int id)
			{
				this.id = id;
			}
		}

		protected sealed class EventLog
		{
			public bool xing;

			public bool xed;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new EventsTestCaseBase.Item(1));
		}

		protected virtual void AssertClientTransaction(EventArgs args)
		{
			Assert.AreSame(Trans(), ((TransactionalEventArgs)args).Transaction());
		}
	}
}
