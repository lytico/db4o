/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class HashtableIdentityEntry extends HashtableIntEntry{
	
	public HashtableIdentityEntry(Object obj){
		super(System.identityHashCode(obj), obj);
	}
	
	@Override
	public boolean sameKeyAs(HashtableIntEntry other) {
		if(! super.sameKeyAs(other)){
			return false;
		}
		return other._object == _object;
	}

}
