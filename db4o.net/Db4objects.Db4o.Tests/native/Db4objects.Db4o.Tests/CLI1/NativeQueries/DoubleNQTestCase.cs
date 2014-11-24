using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	class DoubleItem
	{
		private string _name;
		private double _value;

		public DoubleItem(string name, double value)
		{
			_name = name;
			_value = value;
		}

		public string Name
		{
			get { return _name;  }
		}

		public double Value
		{
			get { return _value;  }
		}


		public override string ToString()
		{
			return "DoubleItem(" + Name + ", " + Value + ")";
		}
	}

	class DoublePredicate
	{
		public bool Match(DoubleItem item)
		{
			return item.Value == 41.99;
		}
	}

	class DoubleNQTestCase : AbstractNativeQueriesTestCase
	{
#if !SILVERLIGHT
		protected override void Store()
		{
			Store(new DoubleItem("foo", 11.5));
			Store(new DoubleItem("bar", 41.99));
		}

		public void Test()
		{
			AssertNQResult(new DoublePredicate(), ItemByName("bar"));
		}

		private object ItemByName(string name)
		{
			IQuery query = NewQuery(typeof(DoubleItem));
			query.Descend("_name").Constrain(name);
			return query.Execute().Next();
		}
#endif
	}
}
