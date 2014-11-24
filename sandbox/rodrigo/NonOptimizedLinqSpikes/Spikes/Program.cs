using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using System.Reflection;
using Db4objects.Db4o.Config;

namespace Spikes
{
	class Program
	{
		static void Main(string[] args)
		{
			int totalCows = 30;
			int totalDays = 365;
			new Program(totalCows, totalDays).Run();
		}

		class BenchmarkResult
		{
			public string Action { get; set;  }
			public TimeSpan NonOptimizedTime { get; set; }
			public TimeSpan OptimizedTime { get; set;  }
		}

		readonly List<BenchmarkResult> _summary = new List<BenchmarkResult>();
		readonly int _totalCows;
		readonly int _totalDays;

		public Program(int totalCows, int totalDays)
		{
			_totalCows = totalCows;
			_totalDays = totalDays;
		}

		public void Run()
		{
			var configurations = new[] { Configurations.IndexedFields(), Configurations.Faster() };
			foreach (var config in configurations)
			{
				PrepareDatabase(config);
				RunBenchmarks(config);
			}
			_summary.PrettyPrint();
		}

		private void RunBenchmarks(IConfiguration config)
		{
			using (var system = new FarmSystem(config))
			{
				var cow1 = RandomCow(system);
				var cow2 = RandomCow(system);


				var code = RandomCowCode();
				Benchmark("select a single cow",
					() => from c in system.Cows where c.Code == code select new { c.Code },
					() => from c in new[] { system.OptimizedSingleCow(code) } select new { c.Code });

				// “find all cows whose average milk during august was less than 0.75 of the herd’s average.”
				var august = new DateTime(2007, 8, 1);
				var september = august.AddMonths(1);

				Benchmark("find all cows whose average milk during august was less than 0.75 of the herd’s average.", () =>
				{
					var herdAverage = system.HerdAverage(august, september);
					var threshold = .90 * herdAverage;
					return from c in system.Cows
						   let CowAverage = system.CowAverage(c, august, september)
						   where CowAverage < threshold
						   select new { c.Code, CowAverage };
				}, () =>
				{
					var herdAverage = system.OptimizedHerdAverage(august, september);
					var threshold = .90 * herdAverage;
					return from c in system.Cows
						   let CowAverage = system.OptimizedCowAverage(c, august, september)
						   where CowAverage < threshold
						   select new { c.Code, CowAverage };
				});

				// The example queries:
				// A. For each cow, calculate total milk between date1 and date2
				Benchmark("For each cow, calculate total milk between date1 and date2",
					() => from c in system.Cows
						  let CowTotal = system.CowTotal(c, august, september)
						  select new { c.Code, CowTotal },
					() => from c in system.Cows
						  let CowTotal = system.OptimizedCowTotal(c, august, september)
						  select new { c.Code, CowTotal });


				// B. Find cows whose total(date1,date2) < X

				Benchmark("Find cows whose total(date1,date2) < X",
					() => from c in system.Cows
						  let CowTotal = system.CowTotal(c, august, september)
						  where CowTotal < 2
						  select new { c.Code, CowTotal },
					() => from c in system.Cows
						  let CowTotal = system.OptimizedCowTotal(c, august, september)
						  where CowTotal < 2
						  select new { c.Code, CowTotal });


				// C. Find cows whose total(date1,date2) < 0.75 * average total(date1,date2)

				// D. Days from last give_birth event for cows who are in active milking
				// [these are cows that have a "give_birth" event and no "drying" event
				// after it]

				Benchmark("Days in active milking",
					() => from c in system.Cows
						  let LastGaveBirth = system.DateOfLastEvent(c, "give_birth")
						  let LastDrying = system.DateOfLastEvent(c, "drying")
						  where LastDrying == null || LastGaveBirth > LastDrying
						  select new { c.Code, (DateTime.Now - LastGaveBirth.Value).TotalDays },
					() => from c in system.OptimizedCowsThatEverGaveBirth()
						  let LastGaveBirth = system.OptimizedDateOfLastEvent(c, "give_birth")
						  let LastDrying = system.OptimizedDateOfLastEvent(c, "drying")
						  where LastDrying == null || LastGaveBirth > LastDrying
						  select new { c.Code, (DateTime.Now - LastGaveBirth.Value).TotalDays });


				// E. How many cows gave birth in each month during the last year
				var lastYear = new DateTime(2007, 1, 1);
				var lastYearEnd = lastYear.AddYears(1);

				Benchmark("How many cows gave birth in each month",
					() => from e in system.Events
						  where e.Id == "give_birth" && e.Date >= lastYear && e.Date <= lastYearEnd
						  group e.Cow by e.Date.Month into g
						  select new
						  {
							  Month = g.Key,
							  Count = g.Distinct().Count()
						  },
					() => from CustomCowEvent e in system.OptimizedEventRange("give_birth", lastYear, lastYearEnd)
						  group e.Cow by e.Date.Month into g
						  select new
						  {
							  Month = g.Key,
							  Count = g.Distinct().Count()
						  });
			}
		}

		private Cow RandomCow(FarmSystem system)
		{
			var code = RandomCowCode();
			return (from c in system.Cows where c.Code == code select c).First();
		}

		private string RandomCowCode()
		{
			return "Cow " + new Random().Next(_totalCows);
		}

		delegate IEnumerable<T> QueryBlock<T>();

		private void Benchmark<T>(string label, QueryBlock<T> linq, QueryBlock<T> optimized)
		{
			var nonOptimizedTime = Time("LINQ => " + label, () => linq().PrettyPrint());
			var optimizedTime = Time("Optimized => " + label, () => optimized().PrettyPrint());
			AddToSummary(label.Substring(0, 4) + "...", nonOptimizedTime, optimizedTime);
		}

		private void AddToSummary(string label, TimeSpan nonOptimizedTime, TimeSpan optimizedTime)
		{
			_summary.Add(new BenchmarkResult { Action = label, NonOptimizedTime = nonOptimizedTime, OptimizedTime = optimizedTime });
		}

		delegate void CodeBlock();

		private static TimeSpan Time(string label, CodeBlock code)
		{
			Console.WriteLine(" ======  {0} ======= ", label);
			DateTime start = DateTime.Now;
			code();
			var time = DateTime.Now - start;
			Console.WriteLine("'{0}' took {1}ms", label, time.TotalMilliseconds);
			return time;
		}

		private void PrepareDatabase(IConfiguration config)
		{
			File.Delete(FarmSystem.DatabaseLocation);
			
			var elapsed = Time("Database generation", ()=> GenerateBigFile(config));

			var objectCount = _totalCows + (_totalCows * 4 * _totalDays);
			AddToSummary(objectCount.ToString() + " objects", elapsed, elapsed);
		}

		static readonly string[] EventIds = new[] { "give_birth", "drying", "sick" };

		void GenerateBigFile(IConfiguration config)
		{
			var random = new Random();

			using (var system = new FarmSystem(config))
			{
				Cow[] cows = new Cow[_totalCows];
				for (int i = 0; i < _totalCows; ++i)
				{
					cows[i] = new Cow { Code = "Cow " + i };
					system.Track(cows[i]);
				}

				int days = 0;
				foreach (DateTime day in (DateTime.Now - TimeSpan.FromDays(_totalDays)).DaysUntil(DateTime.Now))
				{
					Console.Write("{0} ", ++days);
					foreach (Cow cow in cows)
					{
						system.Milked(cow, day, (float)(2 * random.NextDouble()));
						system.Milked(cow, day.AddHours(8), (float)(2 * random.NextDouble()));
						system.Milked(cow, day.AddHours(8), (float)(2 * random.NextDouble()));
						system.CustomEvent(cow, day, EventIds[random.Next(EventIds.Length)]);
					}
				}
				Console.WriteLine();
			}
		}
	}
}

