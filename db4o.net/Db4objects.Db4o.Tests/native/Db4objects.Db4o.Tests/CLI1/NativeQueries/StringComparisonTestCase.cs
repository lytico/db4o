/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class NamedThing
	{
		string _name;

		public NamedThing(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}

		public override string ToString()
		{
			return _name;
		}
	}

	class NameStartsWith : Predicate
	{
		string _s;

		public NameStartsWith(string s)
		{
			_s = s;
		}

		public bool Match(NamedThing thing)
		{
			return thing.Name.StartsWith(_s);
		}
	}

	class NameEndsWith : Predicate
	{
		string _s;

		public NameEndsWith(string s)
		{
			_s = s;
		}

		public bool Match(NamedThing thing)
		{
			return thing.Name.EndsWith(_s);
		}
	}

	class NameEquals : Predicate
	{
		string _s;

		public NameEquals(string s)
		{
			_s = s;
		}

		public bool Match(NamedThing thing)
		{
			return _s.Equals(thing.Name);
		}
	}

#if !CF
	class NameContains : Predicate
	{
		string _s;

		public NameContains(string s)
		{
			_s = s;
		}

		public bool Match(NamedThing thing)
		{
			return thing.Name.Contains(_s);
		}
	}
#endif

	/// <summary>
	/// </summary>
	public class StringComparisonTestCase : AbstractNativeQueriesTestCase
	{
#if !SILVERLIGHT
		private NamedThing _robinson;
		private NamedThing _frisbee;
		private NamedThing _bee;
		private NamedThing _friday;
		private NamedThing _round;

		void SetUpData()
		{	
			Store(_frisbee = new NamedThing("Frisbee"));
			Store(_bee = new NamedThing("Bee"));
			Store(_friday = new NamedThing("Friday"));
			Store(_robinson = new NamedThing("Robinson Crusoe"));
			Store(_round = new NamedThing("Round Robin"));
		}

		public void TestStartsWith()
		{
			SetUpData();
			AssertNQResult(new NameStartsWith("Fri"), _frisbee, _friday);
			AssertNQResult(new NameStartsWith("Bee"), _bee);
			AssertNQResult(new NameStartsWith("r"));
			AssertNQResult(new NameStartsWith("R"), _robinson, _round);
		}

#if !CF
		public void TestContains()
		{
			SetUpData();
			AssertNQResult(new NameContains("Fri"), _frisbee, _friday);
			AssertNQResult(new NameContains("ee"), _frisbee, _bee);
			AssertNQResult(new NameContains("r"), _frisbee, _friday, _robinson);
			AssertNQResult(new NameContains("R"), _robinson, _round);
		}
#endif

		public void TestEndsWith()
		{
			SetUpData();
			AssertNQResult(new NameEndsWith("ee"), _frisbee, _bee);
			AssertNQResult(new NameEndsWith("day"), _friday);
			AssertNQResult(new NameEndsWith("Y"));
			AssertNQResult(new NameEndsWith("r"));
			AssertNQResult(new NameEndsWith("e"), _frisbee, _bee, _robinson);
		}

		public void TestEquals()
		{
			SetUpData();
			AssertNQResult(new NameEquals("Bee"), _bee);
			AssertNQResult(new NameEquals("Round Robin"), _round);
			AssertNQResult(new NameEquals("ee"));
			AssertNQResult(new NameEquals("Round"));
		}
#endif
	}
}
