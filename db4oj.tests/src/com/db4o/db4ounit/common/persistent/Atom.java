/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.persistent;

public class Atom {
	
	public Atom child;
	public String name;
	
	public Atom(){
	}
	
	public Atom(Atom child){
		this.child = child;
	}
	
	public Atom(String name){
		this.name = name;
	}
	
	public Atom(Atom child, String name){
		this(child);
		this.name = name;
	}
	
	public int compareTo(Object obj){
		return 0;
	}
	
	public boolean equals(Object obj){
		if(obj instanceof Atom){
			Atom other = (Atom)obj;
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
