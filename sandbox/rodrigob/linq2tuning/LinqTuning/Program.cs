
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Linq;
using Db4oUnit;
using Db4oUnit.Util;
using LinqTuning.Reference1;
using LinqTuning.Reference2;

namespace LinqTuning
{
	class Program
	{
		static void Main(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(System.Console.Out));

			for (int i = 0; i < 3; ++i)
			{
				ReportTime("Query over single property on single assembly",
				           container => from Item1 item in container where item.Id != -1 select item);

				ReportTime("Query over two properties in different assemblies",
				           container => from Item2 item in container where item.Id != -1 && item.Name == "foo" select item);

				ReportTime("Query over two properties twice in different assemblies",
						   container => from Item2 item in container
										where item.Id != -1
											&& item.Name == "foo"
											&& item.Id != 42
											&& item.Name != "bar"
										select item);
			}

		}

		private static void ReportTime<T>(string label, Func<ISodaQueryFactory, IEnumerable<T>> query)
		{
			System.Console.WriteLine("{0}: {1}", label, Time(query));
		}

		static TimeSpan Time<T>(Func<ISodaQueryFactory, IEnumerable<T>> query)
		{
			var queryFactory = new NullSodaQueryFactory();
			var stopWatch = new StopWatch();
			stopWatch.Start();

			for (int i = 0; i < 100000; ++i)
				Assert.AreEqual(0, query(queryFactory).Count());

			stopWatch.Stop();
			return stopWatch.Elapsed();
		}
	}
}
