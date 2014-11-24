using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Config;

namespace Spikes
{
	public class FarmSystem : IDisposable
	{
		public static readonly string DatabaseLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "farm.odb");

		private IObjectContainer _container;

		public FarmSystem()
			: this(Configurations.Faster(), DatabaseLocation)
		{
		}

		public FarmSystem(IConfiguration config)
			: this(config, DatabaseLocation)
		{
		}

		public FarmSystem(IConfiguration config, string fname)
		{
			_container = Db4oFactory.OpenFile(config, fname);
		}

		public IObjectContainer Container
		{
			get { return _container; }
		}

		public void Track(Cow cow)
		{
			_container.Set(cow);
		}

		public void Milked(Cow cow, DateTime date, float amount)
		{
			_container.Set(new Milking { Cow = cow, Date = date, Amount = amount });
		}

		public void CustomEvent(Cow cow, DateTime date, string id)
		{
			_container.Set(new CustomCowEvent { Cow = cow, Date = date, Id = id });
		}

		public IQueryable<Cow> Cows
		{
			get { return Query<Cow>(); }
		}

		public IQueryable<Milking> Milkings
		{
			get { return Query<Milking>(); }
		}

		public IQueryable<CustomCowEvent> Events
		{
			get { return Query<CustomCowEvent>(); }
		}

		public IQueryable<T> Query<T>()
		{
			// THIS ALLOWS LINQ TO BE USED WITH DB4O
			// IN UNOPTIMIZED MODE
			// WE ARE WORKING ON OPTIMIZED LINQ 
			return _container.Query<T>().AsQueryable();
		}

		public void Dispose()
		{
			_container.Dispose();
		}

		public DateTime? DateOfLastEvent(Cow c, string eventId)
		{
			return (from e in Events
					where e.Id == eventId && e.Cow == c
					orderby e.Date descending
					select new DateTime?(e.Date)).FirstOrDefault();
		}

		public float CowTotal(Cow cow, DateTime begin, DateTime end)
		{
			return (from m in MilkingsForPeriod(begin, end)
					where m.Cow == cow
					select m.Amount).Sum();
		}

		public float CowAverage(Cow cow, DateTime begin, DateTime end)
		{
			return (from m in MilkingsForPeriod(begin, end)
					where m.Cow == cow
					select m.Amount).Average();
		}

		public float HerdAverage(DateTime begin, DateTime end)
		{
			return (from m in MilkingsForPeriod(begin, end)
					select m.Amount).Average();
		}

		private IQueryable<Milking> MilkingsForPeriod(DateTime begin, DateTime end)
		{
			return from m in Milkings
				   where m.Date >= begin && m.Date <= end
				   select m;
		}

		internal Cow OptimizedSingleCow(string code)
		{
			IQuery q = SodaQueryFor(typeof(Cow));
			q.Descend("Code").Constrain(code);
			return (Cow)q.Execute().Next();
		}

		IQuery SodaQueryFor(Type type)
		{
			var q = Container.Query();
			q.Constrain(type);
			return q;
		}

		public DateTime? OptimizedDateOfLastEvent(Cow c, string eventId)
		{
			var q = SodaQueryFor(typeof(CustomCowEvent));
			q.Descend("Id").Constrain(eventId);
			q.Descend("Date").OrderDescending();
			var found = q.Execute();
			if (found.HasNext())
			{
				var e = (CustomCowEvent)found.Next();
				return new DateTime?(e.Date);
			}
			return null;
		}

		public IEnumerable<Cow> OptimizedCowsThatEverGaveBirth()
		{
			var q = SodaQueryFor(typeof(CustomCowEvent));
			q.Descend("Id").Constrain("give_birth");
			return (from CustomCowEvent e in q.Execute() select e.Cow).Distinct();
		}

		public float OptimizedCowAverage(Cow cow, DateTime begin, DateTime end)
		{
			return (from Milking m in OptimizedMilkings(cow, begin, end) select m.Amount).Average();
		}

		private IObjectSet OptimizedMilkings(Cow cow, DateTime begin, DateTime end)
		{
			var q = RangeQuery(typeof(Milking), begin, end);
			q.Descend("Cow").Constrain(cow).Identity();
			return q.Execute();
		}

		public float OptimizedCowTotal(Cow cow, DateTime begin, DateTime end)
		{
			return (from Milking m in OptimizedMilkings(cow, begin, end) select m.Amount).Sum();
		}

		internal System.Collections.IEnumerable OptimizedEventRange(string id, DateTime begin, DateTime end)
		{
			var q = RangeQuery(typeof(CustomCowEvent), begin, end);
			q.Descend("Id").Constrain(id);
			return q.Execute();
		}

		internal float OptimizedHerdAverage(DateTime begin, DateTime end)
		{
			return (from Milking m in OptimizedMilkingsForPeriod(begin, end)
					select m.Amount).Average();
		}

		internal System.Collections.IEnumerable OptimizedMilkingsForPeriod(DateTime begin, DateTime end)
		{
			return OptimizedRange(typeof(Milking), begin, end);
		}

		private System.Collections.IEnumerable OptimizedRange(Type type, DateTime begin, DateTime end)
		{
			var q = RangeQuery(type, begin, end);
			return q.Execute();
		}

		private IQuery RangeQuery(Type type, DateTime begin, DateTime end)
		{
			var q = SodaQueryFor(type);
			q.Descend("Date").Constrain(begin).Smaller().Not();
			q.Descend("Date").Constrain(end).Greater().Not();
			return q;
		}

	}


}
