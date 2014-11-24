/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.foundation;

import com.db4o.types.*;

/**
 * Fast linked list for all usecases.
 * 
 * @exclude
 */
public class Collection4 implements DeepClone, Unversioned {

	// TODO: encapsulate field access
	
    /** first element of the linked list */
    public List4 _first;

    /** number of elements collected */
    public int _size;

    /**
     * Adds an element to the beginning of this collection.
     * 
     * @param element
     */
    public final void add(Object element) {
        _first = new List4(_first, element);
        _size++;
    }    

    public final void addAll(Object[] elements) {
        if (elements != null) {
            for (int i = 0; i < elements.length; i++) {
                if (elements[i] != null) {
                    add(elements[i]);
                }
            }
        }
    }
    
    public final void addAll(Collection4 other){
        if (other != null){
            addAll(other.iterator());
        }
    }
    
	public final void addAll(Iterator4 iterator) {
		while (iterator.hasNext()) {
			add(iterator.next());
		}
	}

    public final void clear() {
        _first = null;
        _size = 0;
    }

    public final boolean contains(Object element) {
        return get(element) != null;
    }

    /**
     * tests if the object is in the Collection.
     * == comparison.
     */
    public final boolean containsByIdentity(Object element) {
        List4 current = _first;
        while (current != null) {
            if (current._element != null && current._element == element) {
                return true;
            }
            current = current._next;
        }
        return false;
    }

    /**
     * returns the first object found in the Collections
     * that equals() the passed object
     */
    public final Object get(Object element) {
        Iterator4 i = iterator();
        while (i.hasNext()) {
        	Object current = i.next();
            if (current.equals(element)) {
                return current;
            }
        }
        return null;
    }

    public Object deepClone(Object newParent) {
        Collection4 col = new Collection4();
        Object element = null;
        Iterator4 i = this.iterator();
        while (i.hasNext()) {
            element = i.next();
            if (element instanceof DeepClone) {
                col.add(((DeepClone) element).deepClone(newParent));
            } else {
                col.add(element);
            }
        }
        return col;
    }

    /**
     * makes sure the passed object is in the Collection.
     * equals() comparison.
     */
    public final Object ensure(Object a_obj) {
        Object obj = get(a_obj);
        if (obj != null) {
            return obj;
        }
        add(a_obj);
        return a_obj;
    }

    /**
     * Iterates through the collection in
     * reversed insertion order which happens
     * to be the fastest.
     * 
     * @return
     */
    public final Iterator4 iterator() {
        return _first == null
        	? Iterator4Impl.EMPTY
            : new Iterator4Impl(_first);
    }
    
    /**
     * Iterates through the collection in the correct
     * order (the insertion order).
     * 
     * @return
     */
    public Iterator4 strictIterator() {
		return new ArrayIterator4(toArray());
	}

    /**
     * removes an object from the Collection
     * equals() comparison
     * returns the removed object or null, if none found
     */
    public Object remove(Object a_object) {
        List4 previous = null;
        List4 current = _first;
        while (current != null) {
            if (current._element.equals(a_object)) {
                _size--;
                if (previous == null) {
                    _first = current._next;
                } else {
                    previous._next = current._next;
                }
                return current._element;
            }
            previous = current;
            current = current._next;
        }
        return null;
    }

    public final int size() {
        return _size;
    }

    /**
     * This is a non reflection implementation for more speed.
     * In contrast to the JDK behaviour, the passed array has
     * to be initialized to the right length. 
     */
    public final void toArray(Object[] a_array) {
        int j = _size;
        Iterator4 i = iterator();

        // backwards, since our linked list is the wrong way around
        while (i.hasNext()) {
            a_array[--j] = i.next();
        }
    }
    
    public final Object[] toArray() {
    	Object[] array = new Object[_size];
		toArray(array);
		return array;
    }
    
    public String toString() {
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        if(_size == 0){
            return "[]";
        }
        StringBuffer sb = new StringBuffer();
        sb.append("[");
        Iterator4 i = iterator();
        sb.append(i.next());
        while(i.hasNext()){
            sb.append(", ");
            sb.append(i.next());
        }
        sb.append("]");
        return sb.toString();
    }
}
