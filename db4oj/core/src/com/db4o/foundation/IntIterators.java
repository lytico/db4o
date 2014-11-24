package com.db4o.foundation;

public class IntIterators {

	public static FixedSizeIntIterator4 forInts(final int[] array, final int count) {
	    return new IntIterator4Impl(array, count);
    }

	public static IntIterator4 forLongs(final long[] ids) {
		return new IntIterator4() {
			int _next = 0;
			int _current;
			
			public int currentInt() {
				return _current;
            }

			public Object current() {
				return _current;
            }

			public boolean moveNext() {
				if (_next < ids.length) {
					_current = (int)ids[_next];
					++_next;
					return true;
				}
				return false;
            }

			public void reset() {
	            throw new com.db4o.foundation.NotImplementedException();
            }
		};
    }
	
}
