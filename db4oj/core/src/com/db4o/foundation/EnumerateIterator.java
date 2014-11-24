/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;


public class EnumerateIterator extends MappingIterator {
	
	public static final class Tuple {

		public final int index;
		public final Object value;

		public Tuple(int index_, Object value_) {
			index = index_;
			value = value_;
		}
		
	}

	private int _index;

	public EnumerateIterator(Iterator4 iterator) {
		super(iterator);
		_index = 0;
	}
	
	public boolean moveNext() {
		if (super.moveNext()) {
			++_index;
			return true;
		}
		return false;
	}

	protected Object map(Object current) {
		return new Tuple(_index, current);
	}

}
