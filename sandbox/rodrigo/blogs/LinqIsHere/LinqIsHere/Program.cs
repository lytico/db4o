namespace LinqIsHere
{
	using System;
	using System.Linq;
	using Db4objects.Db4o;
	using Db4objects.Db4o.Linq;

	class Issue
	{	
		public string Status
		{
			get;
			set;
		}

		public string Priority
		{
			get;
			set;
		}

		public override string ToString()
		{
			return string.Format("Issue {{ Status = {0}, Priority = {1} }}", Status, Priority);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			System.IO.File.Delete(FileName);
			PopulateContainer();
			ExecuteQueries();
		}

		private static void ExecuteQueries()
		{
			Time(container =>
			{
				var openIssues = from Issue i in container where i.Status == "Open" select i;
				//PrintAll(openIssues);
				var criticalOpen = from i in openIssues where i.Priority == "Critical" select i;
				//PrintAll(criticalOpen);
				Console.WriteLine("Open: {0}, Critical Open: {1}", openIssues.Count(), criticalOpen.Count());
			});
		}

		private static void PrintAll(IDb4oLinqQuery<Issue> query)
		{
			foreach (var item in query)
			{
				Console.WriteLine(item);
			}
		}

		private static void PopulateContainer()
		{
			WithContainer(container =>
			{
				string[] statuses = { "Open", "Closed" };
				string[] priorities = { "Minor", "Major", "Critical" };
				var random = new Random();
				for (int i = 0; i < NumberOfItems; ++i)
				{
					var randomStatus = statuses[random.Next(statuses.Length)];
					var randomPriority = priorities[random.Next(priorities.Length)];
					container.Store(new Issue { Status = randomStatus, Priority = randomPriority });
				}
			});
		}

		private static void Time(Action<IObjectContainer> action)
		{
			WithContainer(container =>
			{
				DateTime start = DateTime.Now;
				action(container);
				Console.WriteLine("Time: " + (DateTime.Now - start));
			});
		}

		const int NumberOfItems = 1000;

		const string FileName = "linq.db4o";

		private static void WithContainer(Action<IObjectContainer> action)
		{
			var config = Db4oFactory.NewConfiguration();
			using (var container = Db4oFactory.OpenFile(config, FileName))
			{
				action(container);
			}
		}
	}
}
