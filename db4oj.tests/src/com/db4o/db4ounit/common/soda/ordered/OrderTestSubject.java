package com.db4o.db4ounit.common.soda.ordered;

public class OrderTestSubject {
	public String _name;
	public int _seniority;
	public int _age;
		 
	public OrderTestSubject(String name,int age, int seniority){
		this._name=name;
		this._age=age;
		this._seniority=seniority;
	}
		 
	public String toString(){
		return _name + " " + _age + " " + _seniority;
	}
		
	public boolean equals(Object o){
		if (o == null){
			return false;
		}		
			
		if (o.getClass() != getClass()){
			return false;
		}
		
		OrderTestSubject ots = (OrderTestSubject) o;
		boolean ret  =(_age == ots._age) && (_name.equals(ots._name)) && (_seniority == ots._seniority);
		return ret;
	}
}
