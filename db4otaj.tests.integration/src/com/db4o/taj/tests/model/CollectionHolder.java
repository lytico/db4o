/* Copyright (C) 2009  db4objects Inc.  http://www.db4o.com */

package com.db4o.taj.tests.model;

import java.util.*;

public class CollectionHolder {

	private String _name;
	private List _arrayList;
	private List _linkedList;
	private Map _hashMap;
	private Map _hashtable;
	private Stack _stack;
	private HashSet _hashSet;
	private TreeSet _treeSet;
	
	public CollectionHolder(){
		// db4o creation constructor
	}
	
	public CollectionHolder(String name) {
		_name = name;
		_arrayList = new ArrayList();
		_linkedList = new LinkedList();
		_hashMap = new HashMap();
		_hashtable = new Hashtable();
		_stack = new Stack();
		_hashSet = new HashSet();
		_treeSet = new TreeSet();
	}

	public List arrayList() {
		return _arrayList;
	}

	public List linkedList() {
		return _linkedList;
	}
	
	public Stack stack() {
		return _stack;
	}

	public Set hashSet() {
		return _hashSet;
	}

	public Map hashMap() {
		return _hashMap;
	}
	
	public Map hashtable(){
		return _hashtable;
	}
	
	public Set treeSet(){
		return _treeSet;
	}
	
	public String toString() {
		return _name + ": " + _arrayList + "";
	}
}
