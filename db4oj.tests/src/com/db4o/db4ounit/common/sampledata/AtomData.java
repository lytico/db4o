/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.sampledata;

public class AtomData {
	
	public AtomData child;
	public String name;
	
	public AtomData(){
	}
	
	public AtomData(AtomData child){
		this.child = child;
	}
	
	public AtomData(String name){
		this.name = name;
	}
	
	public AtomData(AtomData child, String name){
		this(child);
		this.name = name;
	}
	
	public int hashCode() {
		return this.name != null ? this.name.hashCode() : 0;
	}
	
	public boolean equals(Object obj){
		if(obj instanceof AtomData){
			AtomData other = (AtomData)obj;
			if(name == null){
				if(other.name != null){
					return false;
				}
			}else{
				if(! name.equals(other.name)){
					return false;
				}
			}
			if(child != null){
				return child.equals(other.child);
			}
			return other.child == null;
		}
		return false;
	}
	
	public String toString(){
		String str = "Atom(" + name + ")";
		if(child != null){
			return str + "." + child.toString();
		}
		return str;
	}
	
}
