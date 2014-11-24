/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package  com.db4o.foundation;


/**
 * @exclude
 */
public class Iterator4Impl<T> implements Iterator4 {
	
    private final List4<T> _first;
    private List4<T> _next;
	
	private Object _current;

	public Iterator4Impl(List4 first){
		_first = first;
		_next = first;
		
		_current = Iterators.NO_ELEMENT;
	}

	public boolean moveNext() {
		if (_next == null) {
			_current = Iterators.NO_ELEMENT;
			return false;
		}
		_current = _next._element;
		_next = _next._next;
		return true;
	}

	public T current(){
		if (Iterators.NO_ELEMENT == _current) {
			throw new IllegalStateException();
		}
		return (T) _current;
	}
	
	public void reset() {
		_next = _first;
		_current = Iterators.NO_ELEMENT;
	}
}
