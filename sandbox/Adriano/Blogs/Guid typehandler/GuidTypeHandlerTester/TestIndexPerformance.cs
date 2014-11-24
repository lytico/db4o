using System;
using System.Collections.Generic;
using System.Diagnostics;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4oTestRunner;

namespace GuidTypeHandlerTester
{
	public class TestIndexPerformance : AbstractDb4oTesterBase
	{
		protected override void Run()
		{
			const int objectCount = 10000;
			
			IList<Guid> tbq = new List<Guid>();
			TrackingElapsedTime(delegate
			{
				for (int i = 1; i < objectCount; i++)
				{
					Guid guid = Guid.NewGuid();
					Db().Store(new GuidItem("foo #" + i, guid));

			        if (i % 137 == 0)
					{
						tbq.Add(guid);
					}
				}
			}, "Inserting {0} objects into database.", objectCount);

			Reopen();

			_logger.LogMessage("Quering.");

			TrackingElapsedTime(delegate
			{
				foreach (var candidate in tbq)
				{
					IQuery query = Db().Query();
					query.Constrain(typeof(GuidItem));
					query.Descend("_guid").Constrain(candidate);
					IObjectSet result = query.Execute();
				}
			}, "Querying for {0} guids", tbq.Count);
		}

		private void TrackingElapsedTime(Action action, string message, params object[] args)
		{
			_logger.LogMessage(message, args);

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
				action();
			}
			finally
			{
				stopwatch.Stop();
				_logger.LogMessage("Elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
			}
		}

		protected override Db4objects.Db4o.Config.IConfiguration Configure(Db4objects.Db4o.Config.IConfiguration config)
		{
			config.ObjectClass(typeof(GuidItem)).ObjectField("_guid").Indexed(true);
			return config;
		}
	}
}
