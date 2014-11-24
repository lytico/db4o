/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Fixtures
{
	/// <summary>
	/// TODO: experiment with ParallelTestRunner that uses a thread pool to run tests in parallel
	/// TODO: FixtureProviders must accept the index of a specific fixture to run with (to make it easy to reproduce a failure)
	/// </summary>
	public abstract class FixtureBasedTestSuite : ITestSuiteBuilder
	{
		private static readonly int[] AllCombinations = null;

		public abstract Type[] TestUnits();

		public abstract IFixtureProvider[] FixtureProviders();

		public virtual int[] CombinationToRun()
		{
			return AllCombinations;
		}

		public virtual IEnumerator GetEnumerator()
		{
			IFixtureProvider[] providers = FixtureProviders();
			IEnumerable decorators = FixtureDecoratorsFor(providers);
			IEnumerable testsXdecorators = Iterators.CrossProduct(new IEnumerable[] { Tests()
				, Iterators.CrossProduct(decorators) });
			return Iterators.Map(testsXdecorators, new _IFunction4_35(this)).GetEnumerator();
		}

		private sealed class _IFunction4_35 : IFunction4
		{
			public _IFunction4_35(FixtureBasedTestSuite _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object arg)
			{
				IEnumerator tuple = ((IEnumerable)arg).GetEnumerator();
				ITest test = (ITest)Iterators.Next(tuple);
				IEnumerable decorators = (IEnumerable)Iterators.Next(tuple);
				return this._enclosing.Decorate(test, decorators.GetEnumerator());
			}

			private readonly FixtureBasedTestSuite _enclosing;
		}

		private IEnumerable FixtureDecoratorsFor(IFixtureProvider[] providers)
		{
			int[] combination = CombinationToRun();
			return combination == AllCombinations ? AllFixtureDecoratorsFor(providers) : CombinationFixtureDecoratorsFor
				(providers, combination);
		}

		private IEnumerable CombinationFixtureDecoratorsFor(IFixtureProvider[] providers, 
			int[] combination)
		{
			Assert.AreEqual(providers.Length, combination.Length, "Number of indexes in combinationToRun should match number of providers"
				);
			IEnumerable decorators = Iterators.Map(Iterators.Enumerate(Iterators.Iterable(providers
				)), new _IFunction4_54(combination));
			return decorators;
		}

		private sealed class _IFunction4_54 : IFunction4
		{
			public _IFunction4_54(int[] combination)
			{
				this.combination = combination;
			}

			public object Apply(object arg)
			{
				EnumerateIterator.Tuple providerTuple = (EnumerateIterator.Tuple)arg;
				IFixtureProvider provider = (IFixtureProvider)providerTuple.value;
				int wantedIndex = combination[providerTuple.index];
				return Iterators.Map(Iterators.Enumerate(provider), new _IFunction4_59(wantedIndex
					, provider));
			}

			private sealed class _IFunction4_59 : IFunction4
			{
				public _IFunction4_59(int wantedIndex, IFixtureProvider provider)
				{
					this.wantedIndex = wantedIndex;
					this.provider = provider;
				}

				public object Apply(object arg)
				{
					EnumerateIterator.Tuple tuple = (EnumerateIterator.Tuple)arg;
					if (tuple.index != wantedIndex)
					{
						return Iterators.Skip;
					}
					return new FixtureDecorator(provider.Variable(), tuple.value, tuple.index);
				}

				private readonly int wantedIndex;

				private readonly IFixtureProvider provider;
			}

			private readonly int[] combination;
		}

		private IEnumerable AllFixtureDecoratorsFor(IFixtureProvider[] providers)
		{
			IEnumerable decorators = Iterators.Map(Iterators.Iterable(providers), new _IFunction4_74
				());
			return decorators;
		}

		private sealed class _IFunction4_74 : IFunction4
		{
			public _IFunction4_74()
			{
			}

			public object Apply(object arg)
			{
				IFixtureProvider provider = (IFixtureProvider)arg;
				return Iterators.Map(Iterators.Enumerate(provider), new _IFunction4_77(provider));
			}

			private sealed class _IFunction4_77 : IFunction4
			{
				public _IFunction4_77(IFixtureProvider provider)
				{
					this.provider = provider;
				}

				public object Apply(object arg)
				{
					EnumerateIterator.Tuple tuple = (EnumerateIterator.Tuple)arg;
					return new FixtureDecorator(provider.Variable(), tuple.value, tuple.index);
				}

				private readonly IFixtureProvider provider;
			}
		}

		private IEnumerable Tests()
		{
			Type[] units = TestUnits();
			if (units == null || units.Length == 0)
			{
				throw new InvalidOperationException(GetType() + " has no TestUnits.");
			}
			return new ReflectionTestSuiteBuilder(units);
		}

		private ITest Decorate(ITest test, IEnumerator decorators)
		{
			while (decorators.MoveNext())
			{
				test = ((ITestDecorator)decorators.Current).Decorate(test);
			}
			return test;
		}
	}
}
