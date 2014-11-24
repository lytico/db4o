/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package  com.db4o.foundation;


/**
 * @exclude
 */
public class Iterator4Impl implements Iterator4
{
    public static final Iterator4 EMPTY = new EmptyIterator();
    
	private List4 _next;

	public Iterator4Impl(List4 first){
		_next = first;
	}

	public boolean hasNext(){
		return _next != null;
	}

	public Object next(){
		Object obj = _next._element;
		_next = _next._next;
		return obj;
	}
}
