package com.db4o.db4ounit.common.assorted;
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