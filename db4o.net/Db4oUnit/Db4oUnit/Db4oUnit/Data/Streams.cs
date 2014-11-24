/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit.Data;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Data
{
	/// <summary>Factory for infinite sequences of values.</summary>
	/// <remarks>Factory for infinite sequences of values.</remarks>
	public class Streams
	{
		private static readonly Random random = new Random();

		public static IEnumerable RandomIntegers()
		{
			return Iterators.Series(null, new _IFunction4_15());
		}

		private sealed class _IFunction4_15 : IFunction4
		{
			public _IFunction4_15()
			{
			}

			public object Apply(object arg)
			{
				return Streams.random.Next();
			}
		}

		public static IEnumerable RandomNaturals(int ceiling)
		{
			return Iterators.Series(null, new _IFunction4_23(ceiling));
		}

		private sealed class _IFunction4_23 : IFunction4
		{
			public _IFunction4_23(int ceiling)
			{
				this.ceiling = ceiling;
			}

			public object Apply(object arg)
			{
				return Streams.random.Next(ceiling);
			}

			private readonly int ceiling;
		}

		public static IEnumerable RandomStrings()
		{
			int maxLength = 42;
			return Iterators.Map(RandomNaturals(maxLength), new _IFunction4_32());
		}

		private sealed class _IFunction4_32 : IFunction4
		{
			public _IFunction4_32()
			{
			}

			public object Apply(object arg)
			{
				int length = ((int)arg);
				return Streams.RandomString(length);
			}
		}

		private static string RandomString(int length)
		{
			return Iterators.Join(Generators.Take(length, PrintableCharacters()), string.Empty
				);
		}

		public static IEnumerable PrintableCharacters()
		{
			return Iterators.Filter(RandomCharacters(), new _IPredicate4_45());
		}

		private sealed class _IPredicate4_45 : IPredicate4
		{
			public _IPredicate4_45()
			{
			}

			public bool Match(object candidate)
			{
				char character = (char)candidate;
				return this.IsPrintable(character);
			}

			private bool IsPrintable(char value)
			{
				if (value >= 'a' && value <= 'z')
				{
					return true;
				}
				if (value >= 'A' && value <= 'Z')
				{
					return true;
				}
				if (value >= '0' && value <= '9')
				{
					return true;
				}
				switch (value)
				{
					case '_':
					case ' ':
					case '\r':
					case '\n':
					{
						return true;
					}
				}
				return false;
			}
		}

		public static IEnumerable RandomCharacters()
		{
			char maxCharInclusive = 'z';
			return Iterators.Map(RandomNaturals(1 + (int)maxCharInclusive), new _IFunction4_75
				());
		}

		private sealed class _IFunction4_75 : IFunction4
		{
			public _IFunction4_75()
			{
			}

			public object Apply(object value)
			{
				return (char)((int)value);
			}
		}
	}
}
