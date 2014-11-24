/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

public class IdentityHashtable4 extends HashtableBase implements Map4 {

	public IdentityHashtable4(){
	}
	
	public IdentityHashtable4(int size){
		super(size);
	}
	
	public boolean contains(Object obj){
		return getEntry(obj) != null;
	}
	
	public Object remove(Object obj) {
		if(null == obj){
			throw new ArgumentNullException();
		}
		
		return removeIntEntry(System.identityHashCode(obj));
	}
	
	public boolean containsKey(Object key) {
		return getEntry(key) != null;
	}

	public Object get(Object key) {
		HashtableIntEntry entry = getEntry(key);
		return (entry == null ? null : entry._object);
	}

	private HashtableIntEntry getEntry(Object key) {
		return findWithSameKey(new IdentityEntry(key));
	}

	public void put(Object key, Object value) {
		if(null == key){
			throw new ArgumentNullException();
		}
		putEntry(new IdentityEntry(key, value));
	}
	
	public static class IdentityEntry extends HashtableObjectEntry{
		
		public IdentityEntry(Object obj){
			this(obj, null);
		}
		
		public IdentityEntry(Object key, Object value){
			super(System.identityHashCode(key), key, value);
		}
		
		@Override
		public boolean hasKey(Object key) {
			return _objectKey == key;
		}
	}

}
