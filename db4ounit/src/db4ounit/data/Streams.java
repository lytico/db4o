package db4ounit.data;

import java.util.*;

import com.db4o.foundation.*;

/**
 * Factory for infinite sequences of values.
 */
public class Streams {

	private static final Random random = new Random();
	
	public static Iterable4 randomIntegers() {
		return Iterators.series(null, new Function4() {
			public Object apply(Object arg) {
	            return new Integer(random.nextInt());
            }
		});
    }
	
	public static Iterable4 randomNaturals(final int ceiling) {
		return Iterators.series(null, new Function4() {
			public Object apply(Object arg) {
	            return new Integer(random.nextInt(ceiling));
            }
		});
	}
	
	public static Iterable4 randomStrings() {
		final int maxLength = 42;
		return Iterators.map(randomNaturals(maxLength), new Function4() {
			public Object apply(Object arg) {
				final int length = ((Integer)arg).intValue();
				return randomString(length);
            }
		});
	}

	private static String randomString(int length) {
		return Iterators.join(Generators.take(length, printableCharacters()), "");
    }

	public static Iterable4 printableCharacters() {
		return Iterators.filter(randomCharacters(), new Predicate4() {
			public boolean match(Object candidate) {
	            final Character character = (Character)candidate;
	            return isPrintable(character.charValue());
            }

			private boolean isPrintable(final char value) {
	            if (value >= 'a' && value <= 'z') {
	            	return true;
	            }
	            if (value >= 'A' && value <= 'Z') {
	            	return true;
	            }
	            if (value >= '0' && value <= '9') {
	            	return true;
	            }
	            switch (value) {
	            case '_':
	            case ' ':
	            case '\r':
	            case '\n':
	            	return true;
	            }
	            return false;
            }
		});
    }

	public static Iterable4 randomCharacters() {
	    final char maxCharInclusive = 'z';
		return Iterators.map(randomNaturals(1 + (int)maxCharInclusive), new Function4() {
			public Object apply(final Object value) {
				return new Character((char)((Integer)value).intValue());
			}
		});
    }	
}
