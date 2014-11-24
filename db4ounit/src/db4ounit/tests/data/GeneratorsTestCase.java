package db4ounit.tests.data;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.data.*;

public class GeneratorsTestCase implements TestCase {

	public void testArbitraryIntegerValues() {
		checkArbitraryValuesOf(Integer.class);
	}
	
	public void testArbitraryStringValues() {
		checkArbitraryValuesOf(String.class);
		Iterator4Assert.all(Generators.arbitraryValuesOf(String.class), new Predicate4() {
			public boolean match(Object candidate) {
				return isValidString((String)candidate);
			}

			private boolean isValidString(final String s) {
	            for (int i=0; i<s.length(); ++i) {
					final char ch = s.charAt(i);
					if (!Character.isLetterOrDigit(ch)
						&& !Character.isWhitespace(ch)
						&& ch != '_') {
						return false;
					}
				}
				return true;
            }			
		});
	}

	private void checkArbitraryValuesOf(final Class expectedType) {
	    final Iterable4 values = Generators.arbitraryValuesOf(expectedType);
		Assert.isTrue(values.iterator().moveNext());
		Iterator4Assert.areInstanceOf(expectedType, values);
    }

	public void testTake() {
		final String[] values = new String[] { "1", "2", "3" };
		final Iterable4 source = Iterators.iterable(values);
		assertTake(new Object[0], 0, source);
		assertTake(new Object[] { "1" }, 1, source);
		assertTake(new Object[] { "1", "2" }, 2, source);
		assertTake(values, 3, source);
		assertTake(values, 4, source);
	}


	private void assertTake(final Object[] expected, final int count, final Iterable4 source) {
		Iterator4Assert.areEqual(expected, Generators.take(count, source).iterator());
	}
	
}
