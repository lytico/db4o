/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.sampledata;

public class MoleculeData extends AtomData {
	
	public MoleculeData(){
	}
	
	public MoleculeData(AtomData child){
		super(child);
	}
	
	public MoleculeData(String name){
		super(name);
	}
	
	public MoleculeData(AtomData child, String name){
		super(child, name);
	}
	
	public boolean equals(Object obj){
		if(obj instanceof MoleculeData){
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
