/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !CF && !SILVERLIGHT
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Optional.Monitoring.Samples;

namespace Db4objects.Db4o.Tests.Optional.Monitoring.Samples
{
	public class MonitoringDemo
	{
		private const bool ClientServer = true;

		private static readonly string DatabaseFileName = "mydb.db4o";

		private const int PermanentObjectCount = 10000;

		private const int TemporaryObjectCount = 1000;

		private const int QueryCount = 10;

		private IObjectServer _server;

		public class Item
		{
			public string name;

			public Item(string name)
			{
				this.name = name;
			}
		}

		public static void Main(string[] args)
		{
			new MonitoringDemo().Run();
		}

		public virtual void Run()
		{
			Sharpen.Runtime.Out.WriteLine("MonitoringDemo will run forever to allow you to see JMX/Perfmon statistics."
				);
			Sharpen.Runtime.Out.WriteLine("Cancel running with CTRL + C");
			File4.Delete(DatabaseFileName);
			IObjectContainer objectContainer = OpenContainer();
			StorePermanentObjects(objectContainer);
			try
			{
				while (true)
				{
					StoreTemporaryObjects(objectContainer);
					ExecuteQueries(objectContainer);
					DeleteTemporaryObjects(objectContainer);
				}
			}
			finally
			{
				Close(objectContainer);
			}
		}

		private void Close(IObjectContainer objectContainer)
		{
			objectContainer.Close();
			if (_server != null)
			{
				_server.Close();
				_server = null;
			}
		}

		private IObjectContainer OpenContainer()
		{
			string user = "db4o";
			string password = "db4o";
			_server = Db4oClientServer.OpenServer(((IServerConfiguration)Configure(Db4oClientServer
				.NewServerConfiguration(), "db4o server(" + DatabaseFileName + ")")), DatabaseFileName
				, Db4oClientServer.ArbitraryPort);
			_server.GrantAccess(user, password);
			return Db4oClientServer.OpenClient(((IClientConfiguration)Configure(Db4oClientServer
				.NewClientConfiguration(), "db4o client(localhost:" + _server.Ext().Port() + ")"
				)), "localhost", _server.Ext().Port(), user, password);
			return Db4oEmbedded.OpenFile(((IEmbeddedConfiguration)Configure(Db4oEmbedded.NewConfiguration
				(), "db4o(" + DatabaseFileName + ")")), DatabaseFileName);
		}

		private void ExecuteQueries(IObjectContainer objectContainer)
		{
			for (int i = 0; i < QueryCount; i++)
			{
				ExecuteSodaQuery(objectContainer);
				ExecuteOptimizedNativeQuery(objectContainer);
				ExecuteUnOptimizedNativeQuery(objectContainer);
			}
		}

		private void ExecuteSodaQuery(IObjectContainer objectContainer)
		{
			IQuery query = objectContainer.Query();
			query.Constrain(typeof(MonitoringDemo.Item));
			query.Descend("name").Constrain("1");
			query.Execute();
		}

		private void ExecuteOptimizedNativeQuery(IObjectContainer objectContainer)
		{
			objectContainer.Query(new _Predicate_98());
		}

		private sealed class _Predicate_98 : Predicate
		{
			public _Predicate_98()
			{
			}

			public bool Match(MonitoringDemo.Item candidate)
			{
				return candidate.name.Equals("name1");
			}
		}

		private void ExecuteUnOptimizedNativeQuery(IObjectContainer objectContainer)
		{
			objectContainer.Query(new _Predicate_106());
		}

		private sealed class _Predicate_106 : Predicate
		{
			public _Predicate_106()
			{
			}

			public bool Match(MonitoringDemo.Item candidate)
			{
				return candidate.name[0] == 'q';
			}
		}

		private void DeleteTemporaryObjects(IObjectContainer objectContainer)
		{
			IQuery query = objectContainer.Query();
			query.Constrain(typeof(MonitoringDemo.Item));
			query.Descend("name").Constrain("temp");
			IObjectSet objectSet = query.Execute();
			while (objectSet.HasNext())
			{
				objectContainer.Delete(((MonitoringDemo.Item)objectSet.Next()));
			}
			objectContainer.Commit();
		}

		private void StoreTemporaryObjects(IObjectContainer objectContainer)
		{
			for (int i = 0; i < TemporaryObjectCount; i++)
			{
				objectContainer.Store(new MonitoringDemo.Item("temp"));
			}
			objectContainer.Commit();
		}

		private void StorePermanentObjects(IObjectContainer objectContainer)
		{
			for (int i = 0; i < PermanentObjectCount; i++)
			{
				objectContainer.Store(new MonitoringDemo.Item(string.Empty + i));
			}
			objectContainer.Commit();
		}

		private ICommonConfigurationProvider Configure(ICommonConfigurationProvider config
			, string name)
		{
			((ICommonConfigurationProvider)config).Common.ObjectClass(typeof(MonitoringDemo.Item
				)).ObjectField("name").Indexed(true);
			((ICommonConfigurationProvider)config).Common.NameProvider(new SimpleNameProvider
				(name));
			new AllMonitoringSupport().Apply(config);
			return config;
		}
	}
}
#endif // !CF && !SILVERLIGHT
