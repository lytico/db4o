/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.qlin;

/**
 * Internal implementation class, access should not be necessary,
 * except for implementors.
 * Use the static methods in {@link QLinSupport} {@link QLinSupport#ascending()}
 * and {@link QLinSupport#descending()}
 * @exclude
 */
public class QLinOrderByDirection {
	
	private final String _direction;
	
	private final boolean _ascending;
	
	private QLinOrderByDirection(String direction, boolean ascending) {
		_direction = direction;
		_ascending = ascending;
	}

	final static QLinOrderByDirection ASCENDING = new QLinOrderByDirection("ascending", true);
	
	final static QLinOrderByDirection DESCENDING = new QLinOrderByDirection("descending", false);
	
	public boolean isAscending(){
		return _ascending;
	}
	
	public boolean isDescending(){
		return ! _ascending;
	}
	
	@Override
	public String toString() {
		return _direction;
	}

}
