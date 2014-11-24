package com.db4o.drs.test.data;

import java.util.ArrayList;
import java.util.List;

@SuppressWarnings("unchecked")
public class SimpleListHolder {
	
	private String _name;
	
	private List list = new ArrayList();
	
	public SimpleListHolder(){
		
	}
	
	public SimpleListHolder(String name){
		_name = name;
	}
	

	public List getList() {
		return list;
	}

	public void setList(List list) {
		this.list = list;
	}
	
	public void add(SimpleItem item) {
		list.add(item);
	}

	public void setName(String name) {
		_name = name;
	}
	
	public String getName() {
		return _name;
	}

}
