/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class Element implements CollectionElement, Comparable{
	
	public String _name;
	
	public Element(String name){
		_name = name;
	}
	
	@Override
	public boolean equals(Object obj) {
		if(! (obj instanceof Element)){
			return false;
		}
		Element other = (Element)obj;
		return _name.equals(other._name);
	}
	
	@Override
	public String toString() {
		return "Element " + _name;
	}
	
	@Override
	public int hashCode() {
		return _name.hashCode();
	}

	public int compareTo(Object o) {
		CollectionElement other = (CollectionElement) o;
		if(_name == null){
			if(other.name() == null){
				return 0;
			}else{
				return -1;
			}
		}
		return _name.compareTo(other.name());
	}

	public String name() {
		return _name;
	}
	
}
