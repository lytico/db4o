/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Soda 
{
	public class STValueTypeOrderByTestSuite : FixtureBasedTestSuite
	{
		public static FixtureVariable VALUE_TYPE_TYPE_VARIABLE = FixtureVariable.NewInstance("Type");

		public override Type[] TestUnits()
		{
			return new Type[]
			       	{
			       		typeof(STValueTypesOrderByTestCase)
			       	};
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
						{
			       			new SimpleFixtureProvider(
										VALUE_TYPE_TYPE_VARIABLE, 
										new object[]
										{
#if NET_3_5
											new ValueTypeFixture<DateTimeOffset>(delegate(int i) { return DateTimeOffset.Now.AddHours(i); }),
#endif
#if !SILVERLIGHT
											new ValueTypeFixture<Guid>(delegate(int i) { return new Guid(i, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); }),
											new ValueTypeFixture<TimeSpan>(delegate(int i) { return TimeSpan.FromHours(i); }),
#endif
										}),
						};
		}
	}

	public class Thing<T> where T : struct, IComparable<T>
	{
		public T _value;
		public string _name;

		public Thing()
		{
		}

		public Thing(string name, T value)
		{
			_value = value;
			_name = name;
		}

		public override bool Equals(object obj)
		{
			Thing<T> other = obj as Thing<T>;
			if (other == null) return false;

			return _value.CompareTo(other._value) == 0;
		}

		public override string ToString()
		{
			return "Thing(" + _name + ", " + _value + ")";
		}
	}
}
