package db4ounit.data;

import com.db4o.foundation.*;

/**
 * @sharpen.partial
 */
public class Generators {

	public static Iterable4 arbitraryValuesOf(Class type) {
		final Iterable4 platformSpecific = platformSpecificArbitraryValuesOf(type);
		if (null != platformSpecific) {
			return platformSpecific;
		}
		if (type == Integer.class) {
			return take(10, Streams.randomIntegers());
		}
		if (type == String.class) {
			return take(10, Streams.randomStrings());
		}
		throw new NotImplementedException("No generator for type " + type);
    }

	/**
	 * @sharpen.ignore
	 */
	private static Iterable4 platformSpecificArbitraryValuesOf(Class type) {
		return null;
    }

	static Iterable4 trace(Iterable4 source) {
		return Iterators.map(source, new Function4() {
			public Object apply(Object value) {
				System.out.println(value);
				return value;
            }
		});
    }

	public static Iterable4 take(final int count, final Iterable4 source) {
		return new Iterable4() {
			public Iterator4 iterator() {
				return new Iterator4() {
					private int _taken = 0;
					private Iterator4 _delegate = source.iterator();

					public Object current() {
						if (_taken > count) {
							throw new IllegalStateException();
						}
						return _delegate.current();
                    }

					public boolean moveNext() {
						if (_taken < count) {
							if (!_delegate.moveNext()) {
								_taken = count;
								return false;
							}
							++_taken;
							return true;
						}
						return false;
                    }

					public void reset() {
						_taken = 0;
						_delegate = source.iterator();
                    }
				};
			}
		};
    }

}
