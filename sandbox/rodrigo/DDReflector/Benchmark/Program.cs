using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Benchmark
{
	public class Item
	{
		private string _name;
		private int _value;

		public Item(string name, int value)
		{
			_name = name;
			_value = value;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			for (int i = 0; i < 3; ++i)
			{
				Benchmark("Default reflector", NewConfiguration());
				Benchmark("DDReflector", DDReflectorConfig());
			}
		}

		private static IConfiguration DDReflectorConfig()
		{
			IConfiguration config = NewConfiguration();
			config.ReflectWith(new DDReflector.DDReflector());
			return config;
		}

		private static IConfiguration NewConfiguration()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Io(new MemoryIoAdapter());
			return config;
		}

		private static void Benchmark(string label, IConfiguration config)
		{
			TimeSpan elapsed = Benchmark(config);
			Console.WriteLine("{0}: {1}", label, elapsed);
		}

		private static TimeSpan Benchmark(IConfiguration configuration)
		{
			const string fname = "default.db4o";
			System.IO.File.Delete(fname);

			using (IObjectContainer container = Db4oFactory.OpenFile(configuration, fname))
			{
				DateTime start = DateTime.Now;
				const int objects = 5000;
				for (int i = 0; i < objects; ++i)
				{
					container.Set(new Item("Item " + i, i));
				}
				const int queries = objects / 10;
				for (int i = 0; i < queries; ++i)
				{
					container.Ext().Purge();
					foreach (Item item in container.Query<Item>())
					{
						item.ToString();
					}
				}
				return DateTime.Now - start;
			}
		}
	}
}
