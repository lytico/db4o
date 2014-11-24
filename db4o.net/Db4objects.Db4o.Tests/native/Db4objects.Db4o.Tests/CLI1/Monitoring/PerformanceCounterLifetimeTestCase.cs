/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Monitoring;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Api;
using Db4oUnit;
using Db4objects.Db4o.Linq;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class PerformanceCounterLifetimeTestCase : TestWithTempFile
	{
		public void TestIOCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new IOMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));
			});
		}

		public void TestQueryCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new QueryMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));
				IQuery query = db.Query();
				query.Constrain(typeof(Item));
				query.Descend("name").Equals("foo");
				
				foreach(Object obj in query.Execute())
				{
				}
			});
		}

		public void TestUnoptimizedNativeQueryCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new NativeQueryMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));

                foreach (Item item in db.Query<Item>(delegate(Item candidate) { return candidate.Name == "foo"; }))
				{
				}
			});
		}

		public void TestOptimizedNativeQueryCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new NativeQueryMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));

                foreach (Item item in db.Query<Item>(delegate(Item candidate) { return candidate.name == "foo"; }))
                {
				}
			});
		}

        public void TestObjectLifecycleCounters()
        {
            AssertPerformanceCounterInstanceLifetime(new ObjectLifecycleMonitoringSupport(), delegate(IObjectContainer db)
            {
                Item item = new Item("foo");
                db.Store(item);
                db.Deactivate(item,1);
                db.Activate(item,1);
                db.Delete(item);
            });
        }

		public void TestOptimizedLINQCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new NativeQueryMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));
				var result = from Item candidate in db where candidate.name == "foo" select candidate;
				foreach (var item in result)
				{
				}
			});
		}

		public void TestUnoptimizedLINQCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new NativeQueryMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));
				var result = from Item candidate in db where candidate.Name == "foo" select candidate;
				foreach (var item in result)
				{
				}
			});
		}

		public void TestReferenceSystemCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new ReferenceSystemMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));
			});
		}

		public void TestFreespaceCounters()
		{
			AssertPerformanceCounterInstanceLifetime(new FreespaceMonitoringSupport(), delegate(IObjectContainer db)
			{
				db.Store(new Item("foo"));
			});
		}

		public void TestNetworkingCounters()
		{
			IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
			config.Common.Add(new NetworkingMonitoringSupport());

			using (IObjectServer server = Db4oClientServer.OpenServer(config, TempFile(), Db4oClientServer.ArbitraryPort))
			{
				const string userName = "db4o";
				const string password = userName;

				server.GrantAccess(userName, password);
				IObjectContainer client = Db4oClientServer.OpenClient("localhost", server.Ext().Port(), userName, password);
				client.Close();

				Assert.IsTrue(PerformanceCounterCategory.InstanceExists(TempFile(), Db4oPerformanceCounters.CategoryName));
			}
			Assert.IsFalse(PerformanceCounterCategory.InstanceExists(TempFile(), Db4oPerformanceCounters.CategoryName));
		}

		private void AssertPerformanceCounterInstanceLifetime(IConfigurationItem support, Action<IObjectContainer> action)
		{
			using (IObjectContainer db = Db4oEmbedded.OpenFile(NewEmbeddedConfiguration(support), TempFile()))
			{
				action(db);
				Assert.IsTrue(PerformanceCounterCategory.InstanceExists(TempFile(), Db4oPerformanceCounters.CategoryName));
			}

			Assert.IsFalse(PerformanceCounterCategory.InstanceExists(TempFile(), Db4oPerformanceCounters.CategoryName));
		}

		private static IEmbeddedConfiguration NewEmbeddedConfiguration(IConfigurationItem support)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();

			config.Common.Add(support);
			return config;
		}
	}

	class Item
	{
		public Item(string name)
		{
			this.name = name;
		}

		public string Name
		{
			get
			{
				if (Debugger.IsAttached)
				{
					return name + " under debugger, I am not optimizable!";
				}
				return name;
			}
		}
		
		public string name;
	}
}

#endif