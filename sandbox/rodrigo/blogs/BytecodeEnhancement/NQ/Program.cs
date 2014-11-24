using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;

namespace NQ
{
	class Item
	{
		private string _name;

		public Item(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			System.IO.File.Delete(FileName);
			PopulateContainer();
			ExecuteNQ();
			Unoptimizable();
			UnoptimizableRewritten();
		}

		private static void PopulateContainer()
		{
			WithContainer(container =>
			{
				for (var i = 0; i < NumberOfItems; ++i)
				{
					container.Store(new Item("Item " + i));
				}
				container.Store(new Item("Foo"));
			});
		}

		private static void ExecuteNQ()
		{
			Time(container =>
			{
				var randomItemName = "Item " + new Random().Next(NumberOfItems);
				var query = container.Query<Item>(item => item.Name == randomItemName);
				PrintAll(query);
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

		private static void Unoptimizable()
		{
			Time(container =>
			{
				var query = container.Query<Item>(item => item.Name[0] == 'F');
				PrintAll(query);
			});
		}

		private static void UnoptimizableRewritten()
		{
			Time(container =>
			{
				var query = container.Query<Item>(item => item.Name.StartsWith("F"));
				PrintAll(query);
			});
		}

		private static void PrintAll(IList<Item> query)
		{
			foreach (var item in query)
			{
				Console.WriteLine(item.Name);
			}
		}

		const int NumberOfItems = 50000;

		const string FileName = "nq.db4o";

		private static void WithContainer(Action<IObjectContainer> action)
		{
			var config = Db4oFactory.NewConfiguration();
			config.OptimizeNativeQueries(true);
			config.ObjectClass(typeof(Item)).ObjectField("_name").Indexed(true);

			using (var container = Db4oFactory.OpenFile(config, FileName))
			{
				action(container);
			}
		}
	}
}
