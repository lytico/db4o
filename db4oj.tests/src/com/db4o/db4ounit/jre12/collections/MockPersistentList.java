/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.foundation.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class MockPersistentList implements List{
    
    private Vector _vector = new Vector();

    public boolean add(Object o) {
        return _vector.add(o);
    }

    public void add(int index, Object element) {
        _vector.add(index, element);
    }

    public boolean addAll(Iterable4 i) {
        Iterator4 iterator = i.iterator();
        while(iterator.moveNext()){
            add(iterator.current());
        }
        return true;
    }

    public boolean addAll(int index, Iterable4 i) {
        Iterator4 iterator = i.iterator();
        while(iterator.moveNext()){
            add(index++, iterator.current());
        }
        return true;
    }

    public void clear() {
        _vector.clear();
    }

    public boolean contains(Object o) {
        return _vector.contains(o);
    }

    public boolean containsAll(Iterable4 i) {
        Iterator4 iterator = i.iterator();
        while(iterator.moveNext()){
            if(! contains(iterator.current())){
                return false;
            }
        }
        return true;
    }

    public Object get(int index) {
        return _vector.get(index);
    }

    public int indexOf(Object o) {
        return _vector.indexOf(o);
    }

    public boolean isEmpty() {
        return _vector.isEmpty();
    }

    public Iterator iterator() {
        return _vector.iterator();
    }

    public int lastIndexOf(Object o) {
        return _vector.lastIndexOf(o);
    }

    public boolean remove(Object o) {
        return _vector.remove(o);
    }

    public Object remove(int index) {
        return _vector.remove(index);
    }

    public boolean removeAll(Iterable4 i) {
        boolean result = false;
        Iterator4 iterator = i.iterator();
        while(iterator.moveNext()){
            if(remove(iterator.current())){
                result = true;
            }
        }
        return result;
    }

    public boolean retainAll(Iterable4 retained) {
    	boolean result = false;
		Iterator iter = _vector.iterator();
		while (iter.hasNext()) {
			if (!contains(retained, iter.next())) {
				iter.remove();
				result = true;
			}
		}
		return result;
    }
    
    private boolean contains(Iterable4 iter, Object element) {
		Iterator4 i = iter.iterator();
		while (i.moveNext()) {
			Object current = i.current();
			if ((current == null && element == null) || current.equals(element)) {
				return true;
			}
		}
		return false;
	}

    public Object set(int index, Object element) {
        return _vector.set(index, element);
    }

    public int size() {
        return _vector.size();
    }

    public List subList(int fromIndex, int toIndex) {
        throw new NotImplementedException();
    }

    public Object[] toArray() {
        return _vector.toArray();
    }

    public Object[] toArray(Object[] a) {
        return _vector.toArray(a);
    }

	public boolean addAll(Collection c) {
		// TODO Auto-generated method stub
		return false;
	}

	public boolean addAll(int index, Collection c) {
		// TODO Auto-generated method stub
		return false;
	}

	public boolean containsAll(Collection c) {
		// TODO Auto-generated method stub
		return false;
	}

	public ListIterator listIterator() {
		// TODO Auto-generated method stub
		return null;
	}

	public ListIterator listIterator(int index) {
		// TODO Auto-generated method stub
		return null;
	}

	public boolean removeAll(Collection c) {
		// TODO Auto-generated method stub
		return false;
	}

	public boolean retainAll(Collection c) {
		// TODO Auto-generated method stub
		return false;
	}

}
