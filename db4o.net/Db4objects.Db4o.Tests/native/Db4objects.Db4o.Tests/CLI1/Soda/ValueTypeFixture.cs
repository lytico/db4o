/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Soda
{
	interface IValueTypeFixture
	{
		object New(int index);
		Type Type();
		int Compare(object lhs, object rhs);
	}

	class ValueTypeFixture<T> : ILabeled, IValueTypeFixture where T : struct, IComparable<T>
	{
		private readonly Func<int, T> _valueExtractor;

		public ValueTypeFixture(Func<int, T> extractor)
		{
			_valueExtractor = extractor;
		}

		public object New(int index)
		{
			return new Thing<T>("Item #" + index, _valueExtractor(index));
		}

		public Type Type()
		{
			return typeof(Thing<T>);
		}

		public int Compare(object lhs, object rhs)
		{
			Thing<T> lhsThing = (Thing<T>)lhs;
			Thing<T> rhsThing = (Thing<T>)rhs;

			return lhsThing._value.CompareTo(rhsThing._value);
		}

		public string Label()
		{
			string genericTypeName = Type().Name;
			return genericTypeName.Substring(0, genericTypeName.Length - 2) + "<" + Type().GetGenericArguments()[0].Name + ">";
		}
	}
}
