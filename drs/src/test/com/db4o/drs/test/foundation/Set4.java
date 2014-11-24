/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test.foundation;

import com.db4o.foundation.*;

public class Set4 implements Iterable4 {
	
	public static final Set4 EMPTY_SET = new Set4(0);
	
	private final Hashtable4 _table;
	
	public Set4() {
		_table = new Hashtable4();
	}
	
	public Set4(int size) {
		_table = new Hashtable4(size);
	}
	
	public Set4(Iterable4 keys) {
		this();
		addAll(keys);
	}

	public void add(Object element) {
		_table.put(element, element);
	}
	
	public void addAll(Iterable4 other) {
		Iterator4 i = other.iterator();
		while(i.moveNext()){
			add(i.current());			
		}
	}
	
	public boolean isEmpty() {
		return _table.size() == 0;
	}
	
	public int size() {
		return _table.size();
	}
	
	public boolean contains(Object element) {
		return _table.get(element) != null;
	}
	
	public boolean containsAll(Set4 other) {
		return _table.containsAllKeys(other);
	}
	
	public Iterator4 iterator() {
		return _table.keys();
	}
	
	public String toString() {
		return Iterators.join(iterator(), "[", "]", ", ");
	}
}