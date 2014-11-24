/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

import com.db4o.activation.*;

/**
 * extends LinkedList with Transparent Activation and
 * Transparent Persistence support
 * @since 7.9
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableLinkedList<E> extends LinkedList<E> implements ActivatableList<E> {

	private transient Activator _activator;

	public ActivatableLinkedList() {
	}
	
	public ActivatableLinkedList(Collection<E> collection) {
		super(collection);
	}

	public void activate(ActivationPurpose purpose) {
		ActivatableSupport.activate(_activator, purpose);
	}

	public void bind(Activator activator) {
		_activator = ActivatableSupport.validateForBind(_activator, activator);
	}

	@Override
	public boolean add(E e) {
		activate(ActivationPurpose.WRITE);
		return super.add(e);
	};
	
	public void add(int index, E element) {
		activate(ActivationPurpose.WRITE);
		super.add(index, element);
	};
	
	@Override
	public boolean addAll(Collection<? extends E> c) {
		activate(ActivationPurpose.WRITE);
		return super.addAll(c);
	}
	
	@Override
	public boolean addAll(int index, Collection<? extends E> c) {
		activate(ActivationPurpose.WRITE);
		return super.addAll(index, c);
	}
	
	public void addFirst(E e) {
		activate(ActivationPurpose.WRITE);
		super.addFirst(e);
	};
	
	public void addLast(E e) {
		activate(ActivationPurpose.WRITE);
		super.addLast(e);
	};

	
	@Override
	public void clear() {
		activate(ActivationPurpose.WRITE);
		super.clear();
	}

	@Override
	public Object clone() {
		activate(ActivationPurpose.READ);
		ActivatableLinkedList<E> cloned = (ActivatableLinkedList<E>) super.clone();
		cloned._activator = null;
		return cloned;
	}
	
	@Override
	public boolean contains(Object o) {
		activate(ActivationPurpose.READ);
		return super.contains(o);
	}
	
	@Override
	public boolean containsAll(Collection<?> c) {
		activate(ActivationPurpose.READ);
		return super.containsAll(c);
	}
	
	@Override
	public boolean equals(Object o) {
		activate(ActivationPurpose.READ);
		return super.equals(o);
	}
	
	@Override
	public E get(int index) {
		activate(ActivationPurpose.READ);
		return super.get(index);
	}
	
	@Override
	public int hashCode() {
		activate(ActivationPurpose.READ);
		return super.hashCode();
	}
	
	@Override
	public int indexOf(Object o) {
		activate(ActivationPurpose.READ);
		return super.indexOf(o);
	}
	
	@Override
	public Iterator<E> iterator() {
		activate(ActivationPurpose.READ);
		return new ActivatingIterator(this, super.iterator());
	}
	
	@Override
	public boolean isEmpty() {
		activate(ActivationPurpose.READ);
		return super.isEmpty();
	}
	
	@Override
	public int lastIndexOf(Object o) {
		activate(ActivationPurpose.READ);
		return super.lastIndexOf(o);
	}
	
	@Override
	public ListIterator<E> listIterator() {
		activate(ActivationPurpose.READ);
		return new ActivatingListIterator(this, super.listIterator());
	}
	
	@Override
	public ListIterator<E> listIterator(int index) {
		activate(ActivationPurpose.READ);
		return new ActivatingListIterator(this, super.listIterator(index));
	}
	
	@Override
	public E remove(int index) {
		activate(ActivationPurpose.WRITE);		
		return super.remove(index);
	}
	
	@Override
	public boolean remove(Object o) {
		activate(ActivationPurpose.WRITE);
		return super.remove(o);
	}
	
	@Override
	public E set(int index, E element) {
		activate(ActivationPurpose.WRITE);
		return super.set(index, element);
	};
	
	@Override
	public int size() {
		activate(ActivationPurpose.READ);
		return super.size();
	}
	
	@Override
	public List<E> subList(int fromIndex, int toIndex) {
		activate(ActivationPurpose.READ);
		return super.subList(fromIndex, toIndex);
	}
	
	@Override
	public Object[] toArray() {
		activate(ActivationPurpose.READ);
		return super.toArray();
	}
	
	@Override
	public <T extends Object> T[] toArray(T[] a) {
		activate(ActivationPurpose.READ);
		return super.toArray(a);
	};
	
	@Override
	public boolean removeAll(Collection<?> c) {
		activate(ActivationPurpose.WRITE);
		return super.removeAll(c);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	@Override
    public java.util.Iterator<E> descendingIterator() {
		activate(ActivationPurpose.READ);
    	return super.descendingIterator();
    }

	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	@Override
    public E element() {
		activate(ActivationPurpose.READ);
    	return super.element();
    }
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	@Override
    public E getFirst() {
		activate(ActivationPurpose.READ);
    	return super.getFirst();
    }
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	@Override
    public E getLast() {
		activate(ActivationPurpose.READ);
    	return super.getLast();
    }

	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	@Override
	public boolean offer(E e) {
		activate(ActivationPurpose.READ);
		return super.offer(e);
	}

	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	@Override
	public boolean offerFirst(E e) {
		activate(ActivationPurpose.READ);
		return super.offerFirst(e);
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public boolean offerLast(E e) {
		activate(ActivationPurpose.READ);
		return super.offerLast(e);
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E peek() {
		activate(ActivationPurpose.READ);
		return super.peek();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E peekFirst() {
		activate(ActivationPurpose.READ);
		return super.peekFirst();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E peekLast() {
		activate(ActivationPurpose.READ);
		return super.peekLast();
	}


	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E poll() {
		activate(ActivationPurpose.READ);
		return super.poll();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E pollFirst() {
		activate(ActivationPurpose.READ);
		return super.pollFirst();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E pollLast() {
		activate(ActivationPurpose.READ);
		return super.pollLast();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E pop() {
		activate(ActivationPurpose.WRITE);
		return super.pop();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public void push(E element) {
		activate(ActivationPurpose.WRITE);
		super.push(element);
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E remove() {
		activate(ActivationPurpose.WRITE);
		return super.remove();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E removeFirst() {
		activate(ActivationPurpose.WRITE);
		return super.removeFirst();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public boolean removeFirstOccurrence(Object element) {
		activate(ActivationPurpose.WRITE);
		return super.removeFirstOccurrence(element);
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public E removeLast() {
		activate(ActivationPurpose.WRITE);
		return super.removeLast();
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public boolean removeLastOccurrence(Object element) {
		activate(ActivationPurpose.WRITE);
		return super.removeLastOccurrence(element);
	}

	@decaf.Ignore(unlessCompatible = decaf.Platform.JDK16)
	@Override
	public boolean retainAll(Collection<?> c) {
		activate(ActivationPurpose.WRITE);
		return super.retainAll(c);
	}

	@Override
	public String toString() {
		return super.toString();
	}
}
