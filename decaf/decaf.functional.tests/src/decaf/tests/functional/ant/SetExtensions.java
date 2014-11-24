package decaf.tests.functional.ant;

import java.util.*;

public class SetExtensions {

	static <T> Set<T> difference(final Set<T> a, final Set<T> b) {
		HashSet<T> clone = new HashSet<T>(a);
		clone.removeAll(b);
		return clone;
	}

	static <T> Set<T> intersection(Set<T> x, Set<T> y) {
		final HashSet<T> result = new HashSet<T>(x);
		result.retainAll(y);
		return result;
	}

}
