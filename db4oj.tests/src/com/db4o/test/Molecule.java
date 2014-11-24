/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

public class Molecule extends Atom {
	
	public Molecule(){
	}
	
	public Molecule(Atom child){
		super(child);
	}
	
	public Molecule(String name){
		super(name);
	}
	
	public Molecule(Atom child, String name){
		super(child, name);
	}
	
	public boolean equals(Object obj){
		if(obj instanceof Molecule){
			return super.equals(obj);
		}
		return false;
	}
	
	public String toString(){
		String str = "Molecule(" + name + ")";
		if(child != null){
			return str + "." + child.toString();
		}
		return str;
	}
}
