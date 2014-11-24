/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class SubArrayList4<E> extends AbstractList4<E> {

	private AbstractList4<E> _delegate;

	private int _fromIndex;

	private int _size;

	public SubArrayList4(AbstractList4<E> delegate, int fromIndex, int toIndex) {
		_delegate = delegate;
		_fromIndex = fromIndex;
		syncModCount();
		setSize(toIndex - fromIndex);
	}

	@Override
	public void add(int index, E element) {
		checkIndex(index, 0, size());
		checkConcurrentModification();
		_delegate.add(translatedIndex(index), element);
		increaseSize(1);
		syncModCount();
	}

	@Override
	public boolean addAll(int index, Collection<? extends E> collection) {
		checkIndex(index, 0, size());
		checkConcurrentModification();
		boolean changed = _delegate.addAll(translatedIndex(index), collection);
		increaseSize(collection.size());
		syncModCount();
		return changed;
	}

	@Override
	public E get(int index) {
		checkIndex(index, 0, size() - 1);
		checkConcurrentModification();
		return _delegate.get(translatedIndex(index));
	}

	@Override
	public E remove(int index) {
		checkIndex(index, 0, size() - 1);
		checkConcurrentModification();
		E removed = _delegate.remove(translatedIndex(index));
		decreaseSize(1);
		syncModCount();
		return removed;
	}

	@Override
	protected void removeRange(int fromIndex, int toIndex) {
		if ((fromIndex < 0 || fromIndex >= size() || toIndex > size() || toIndex < fromIndex)) {
			throw new IndexOutOfBoundsException();
		}
		if (fromIndex == toIndex) {
			return;
		}
		_delegate.removeRange(fromIndex+_fromIndex, toIndex+_fromIndex);
		decreaseSize(toIndex - fromIndex);
		syncModCount();
	}

	@Override
	public E set(int index, E element) {
		checkIndex(index, 0, size() - 1);
		checkConcurrentModification();
		E replaced = _delegate.set(translatedIndex(index), element);
		syncModCount();
		return replaced;
	}

	@Override
	public int size() {
		return _size;
	}

	private void checkConcurrentModification() {
		if (modCount != _delegate.modCount) {
			throw new ConcurrentModificationException();
		}
	}

	private void syncModCount() {
		modCount = _delegate.modCount;
	}

	private int translatedIndex(int index) {
		return index + _fromIndex;
	}

	private void setSize(int count) {
		_size = count;
	}

	private void increaseSize(int count) {
		_size += count;
	}

	private void decreaseSize(int count) {
		_size -= count;
	}
}
