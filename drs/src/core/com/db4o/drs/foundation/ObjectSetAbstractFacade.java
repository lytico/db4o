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
package com.db4o.drs.foundation;

import com.db4o.ObjectSet;
import com.db4o.ext.ExtObjectSet;

import java.util.*;

/**
 * @sharpen.ignore
 */
public abstract class ObjectSetAbstractFacade implements ObjectSet {
	private RuntimeException notSupported() {
		return new UnsupportedOperationException("not supported");
	}

	public ExtObjectSet ext() {
		throw notSupported();
	}

	public boolean isEmpty() {
		throw notSupported();
	}

	public Iterator iterator() {
		throw notSupported();
	}

	public Object[] toArray() {
		throw notSupported();
	}

	public Object[] toArray(Object[] arg0) {
		throw notSupported();
	}

	public boolean add(Object arg0) {
		throw notSupported();
	}

	public boolean remove(Object o) {
		throw notSupported();
	}

	public boolean containsAll(Collection arg0) {
		throw notSupported();
	}

	public boolean addAll(Collection arg0) {
		throw notSupported();
	}

	public boolean addAll(int arg0, Collection arg1) {
		throw notSupported();
	}

	public boolean removeAll(Collection arg0) {
		throw notSupported();
	}

	public boolean retainAll(Collection arg0) {
		throw notSupported();
	}

	public void clear() {
		throw notSupported();
	}

	public Object get(int index) {
		throw notSupported();
	}

	public Object set(int arg0, Object arg1) {
		throw notSupported();
	}

	public void add(int arg0, Object arg1) {
		throw notSupported();
	}

	public Object remove(int index) {
		throw notSupported();
	}

	public int indexOf(Object o) {
		throw notSupported();
	}

	public int lastIndexOf(Object o) {
		throw notSupported();
	}

	public ListIterator listIterator() {
		throw notSupported();
	}

	public ListIterator listIterator(int index) {
		throw notSupported();
	}

	public List subList(int fromIndex, int toIndex) {
		throw notSupported();
	}

	public void remove() {
		throw notSupported();
	}
}
