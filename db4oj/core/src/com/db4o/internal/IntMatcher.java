/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;


/**
 * @exclude
 */
public abstract class IntMatcher {
	
	public abstract boolean match(int i);
	
	public static final IntMatcher ZERO = new IntMatcher(){
		public boolean match(int i){
			return i == 0;
		}
	};
	
	public static final IntMatcher POSITIVE = new IntMatcher(){
		public boolean match(int i){
			return i > 0;
		}
	};
	
	public static final IntMatcher NEGATIVE = new IntMatcher(){
		public boolean match(int i){
			return i < 0;
		}
	};

}
