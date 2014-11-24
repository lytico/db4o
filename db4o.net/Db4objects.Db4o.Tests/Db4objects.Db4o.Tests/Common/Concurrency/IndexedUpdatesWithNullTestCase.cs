/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class IndexedUpdatesWithNullTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase().RunConcurrency
				();
		}

		public string str;

		public IndexedUpdatesWithNullTestCase()
		{
		}

		public IndexedUpdatesWithNullTestCase(string str)
		{
			this.str = str;
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(this).ObjectField("str").Indexed(true);
		}

		protected override void Store()
		{
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				("one"));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				("two"));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				("three"));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				(null));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				(null));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				(null));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				(null));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				("four"));
		}

		public virtual void Conc1(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				));
			q.Descend("str").Constrain(null);
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(4, objectSet.Count);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc2(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				));
			q.Descend("str").Constrain(null);
			IObjectSet objectSet = q.Execute();
			if (objectSet.Count == 0)
			{
				// already set by other threads
				return;
			}
			Assert.AreEqual(4, objectSet.Count);
			// wait for other threads
			Thread.Sleep(500);
			while (objectSet.HasNext())
			{
				Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase iuwn = (Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
					)objectSet.Next();
				iuwn.str = "hi";
				oc.Store(iuwn);
				Thread.Sleep(100);
			}
		}

		public virtual void Check2(IExtObjectContainer oc)
		{
			IQuery q1 = oc.Query();
			q1.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				));
			q1.Descend("str").Constrain(null);
			IObjectSet objectSet1 = q1.Execute();
			Assert.AreEqual(0, objectSet1.Count);
			IQuery q2 = oc.Query();
			q2.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.IndexedUpdatesWithNullTestCase
				));
			q2.Descend("str").Constrain("hi");
			IObjectSet objectSet2 = q2.Execute();
			Assert.AreEqual(4, objectSet2.Count);
		}
	}
}
#endif // !SILVERLIGHT
