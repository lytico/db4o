/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

import db4ounit.*;

/**
 * TODO: experiment with ParallelTestRunner that uses a thread pool to run tests in parallel
 * 
 * TODO: FixtureProviders must accept the index of a specific fixture to run with (to make it easy to reproduce a failure)
 * 
 */
public abstract class FixtureBasedTestSuite implements TestSuiteBuilder {

	private static final int[] ALL_COMBINATIONS = null;

	public abstract Class[] testUnits();

	public abstract FixtureProvider[] fixtureProviders();
	
	public int[] combinationToRun() {
		return ALL_COMBINATIONS;
	}

	public Iterator4 iterator() {
		final FixtureProvider[] providers = fixtureProviders();
		
		final Iterable4 decorators = fixtureDecoratorsFor(providers);
		final Iterable4 testsXdecorators = Iterators.crossProduct(new Iterable4[] {
			tests(),
			Iterators.crossProduct(decorators)
		});
		return Iterators.map(testsXdecorators, new Function4() {
			public Object apply(Object arg) {
				Iterator4 tuple = ((Iterable4)arg).iterator();
				Test test = (Test)Iterators.next(tuple);
				Iterable4 decorators = (Iterable4)Iterators.next(tuple);
				return decorate(test, decorators.iterator());
			}
		}).iterator();
	}

	private Iterable4 fixtureDecoratorsFor(final FixtureProvider[] providers) {
		final int[] combination = combinationToRun();
		return combination == ALL_COMBINATIONS
			? allFixtureDecoratorsFor(providers)
			: combinationFixtureDecoratorsFor(providers, combination);
    }

	private Iterable4 combinationFixtureDecoratorsFor(FixtureProvider[] providers, final int[] combination) {
		Assert.areEqual(providers.length, combination.length, "Number of indexes in combinationToRun should match number of providers");
		final Iterable4 decorators = Iterators.map(Iterators.enumerate(Iterators.iterable(providers)), new Function4() {
			public Object apply(final Object arg) {
				EnumerateIterator.Tuple providerTuple = (EnumerateIterator.Tuple)arg;
				final FixtureProvider provider = (FixtureProvider)providerTuple.value;
				final int wantedIndex = combination[providerTuple.index];
				return Iterators.map(Iterators.enumerate(provider), new Function4() {
					public Object apply(final Object arg) {
						EnumerateIterator.Tuple tuple = (EnumerateIterator.Tuple)arg;
						if (tuple.index != wantedIndex) {
							return Iterators.SKIP;
						}
						return new FixtureDecorator(provider.variable(), tuple.value, tuple.index);
					}
				});
			}
		});
	    return decorators;
    }

	private Iterable4 allFixtureDecoratorsFor(final FixtureProvider[] providers) {
	    final Iterable4 decorators = Iterators.map(Iterators.iterable(providers), new Function4() {
			public Object apply(final Object arg) {
				final FixtureProvider provider = (FixtureProvider)arg;
				return Iterators.map(Iterators.enumerate(provider), new Function4() {
					public Object apply(final Object arg) {
						EnumerateIterator.Tuple tuple = (EnumerateIterator.Tuple)arg;
						return new FixtureDecorator(provider.variable(), tuple.value, tuple.index);
					}
				});
			}
		});
	    return decorators;
    }

	private Iterable4 tests() {
		final Class[] units = testUnits();
		if (units == null || units.length == 0) {
			throw new IllegalStateException(getClass() + " has no TestUnits.");
		}
		return new ReflectionTestSuiteBuilder(units);
	}
	
	private Test decorate(Test test, Iterator4 decorators) {
		while (decorators.moveNext()) {
			test = ((TestDecorator)decorators.current()).decorate(test);
		}
		return test;
	}

}