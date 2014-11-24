/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

public class FilteredIterator extends MappingIterator {

	private final Predicate4 _filter;

	public FilteredIterator(Iterator4 iterator, Predicate4 filter) {
		super(iterator);
		_filter = filter;
	}

	protected Object map(Object current) {
		return _filter.match(current)
			? current
			: Iterators.SKIP;
	}

}
