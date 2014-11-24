using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Query;

namespace ConsumingDb4oPerformanceCounters
{
	class Program
	{
		static void Main(string[] args)
		{
			// Here we assume that the performance counters were already installed.
			string databaseFile = Path.GetTempFileName();
			using (var db = Db4oEmbedded.OpenFile(databaseFile))
			{
				db.Store(new Item("foo"));
				db.Store(new Item("bar"));
			}

			using (var db = Db4oEmbedded.OpenFile(NewConfiguration(), databaseFile))
			{
				PerformanceCounter bytesReadPerSec = Db4oPerformanceCounters.CounterFor(PerformanceCounterSpec.QueriesPerSec, db);

				PrintCounter(bytesReadPerSec);
				RunQuery(db);
				RunQuery(db);
				RunQuery(db);
				Thread.Sleep(1000); // Wait one second...
				PrintCounter(bytesReadPerSec);
			}

			File.Delete(databaseFile);
		}

		private static void RunQuery(IObjectContainer container)
		{
			IQuery query = container.Query();
			query.Constrain(typeof (Item));

			foreach(var obj in query.Execute())
			{
			}
		}

		private static void PrintCounter(PerformanceCounter counter)
		{
			Console.WriteLine("Bytes reads/sec\r\n\tRaw Value: {0}\r\n\tValue: {1}", counter.RawValue, counter.NextValue());
		}

		private static IEmbeddedConfiguration NewConfiguration()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.Common.Add(new QueryMonitoringSupport());
			
			return config;
		}
	}

	internal class Item
	{
		public Item(string name)
		{
			_name = name;
		}

		public string Name 
		{ 
			get { return _name;}
		}
		
		private string _name;
	}
}
