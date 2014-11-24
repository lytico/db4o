package decaf.tests.functional.ant;

public class IterableExtensions {

	static <T> String join(Iterable<T> iterable, String separator) {
		final StringBuilder builder = new StringBuilder();
		for (Object o : iterable) {
			if (builder.length() > 0) builder.append(separator);
			builder.append(o);
		}
		return builder.toString();
	}

}
