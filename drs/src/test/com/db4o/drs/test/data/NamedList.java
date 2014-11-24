package com.db4o.drs.test.data;

import java.util.*;

import com.db4o.drs.test.*;

@OptOutRdbms
public class NamedList extends DelegatingList {
	
	private String _name;
	
	public NamedList() {
		this(null);
	}
	
	public NamedList(String name) {
		super(new ArrayList());
		_name = name;
	}
	
	public String name() {
		return _name;
	}
}