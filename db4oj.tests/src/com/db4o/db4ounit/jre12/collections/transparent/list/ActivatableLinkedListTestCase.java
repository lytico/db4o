/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.list;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.collections.*;
import com.db4o.db4ounit.jre12.collections.transparent.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableLinkedListTestCase extends ActivatableCollectionTestCaseBase<LinkedList<CollectionElement>> {
	
	private CollectionSpec<LinkedList<CollectionElement>> _spec = 
			new CollectionSpec<LinkedList<CollectionElement>>(
					LinkedList.class,
					CollectionFactories.activatableLinkedListFactory(),
					CollectionFactories.plainLinkedListFactory()
			);
	
	public LinkedList<CollectionElement> newActivatableCollection() {
		return _spec.newActivatableCollection();
	}
	
	private LinkedList<CollectionElement> newPlainList(){
		return _spec.newPlainCollection();
	}
	
	public void testCreation() {
		new ActivatableLinkedList<Object>();
		new ActivatableLinkedList<String>((Collection<String>)new ActivatableLinkedList<String>());
	}
	
	public void testClone() throws Exception{
		ActivatableLinkedList cloned = (ActivatableLinkedList) singleLinkedList().clone();
		// assert that activator is null - should throw IllegalStateException if it isn't
		cloned.bind(new Activator() {
			public void activate(ActivationPurpose purpose) {
			}
		});
		IteratorAssert.areEqual(newPlainList().iterator(), cloned.iterator());
	}

	public void testToString(){
		Assert.areEqual(newPlainList().toString(), singleCollection().toString());
	}
	
	public void testAddFirst() throws Exception{
		Element element = new Element("first");
		singleLinkedList().addFirst(element);
		reopen();
		Assert.isTrue(singleLinkedList().contains(element));
	}
	
	public void testAddLast() throws Exception{
		Element element = new Element("last");
		singleLinkedList().addLast(element);
		reopen();
		Assert.isTrue(singleLinkedList().contains(element));
	}
	
	private LinkedList<CollectionElement> singleLinkedList(){
		return singleCollection();
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testDescendingIterator() throws Exception{
		Iterator<CollectionElement> expected = newPlainList().descendingIterator();
		Iterator<CollectionElement> actual = singleLinkedList().descendingIterator();
		IteratorAssert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testElement() throws Exception{
		CollectionElement expected = newPlainList().element();
		CollectionElement actual = singleLinkedList().element();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testFirstAndLast() throws Exception{
		CollectionElement expected = newPlainList().getFirst();
		CollectionElement actual = singleLinkedList().getFirst();
		Assert.areEqual(expected, actual);
		expected = newPlainList().getLast();
		actual = singleLinkedList().getLast();
		Assert.areEqual(expected, actual);
	}

	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testOffer() throws Exception{
		Element element = new Element("last");
		Assert.isTrue(singleLinkedList().offer(element));
		reopen();
		Assert.areEqual(singleLinkedList().getLast(), element);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testOfferFirst() throws Exception{
		Element element = new Element("first");
		Assert.isTrue(singleLinkedList().offerFirst(element));
		reopen();
		Assert.areEqual(singleLinkedList().getFirst(), element);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testOfferLast() throws Exception{
		Element element = new Element("last");
		Assert.isTrue(singleLinkedList().offerLast(element));
		reopen();
		Assert.areEqual(singleLinkedList().getLast(), element);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPeek() throws Exception{
		CollectionElement expected = newPlainList().peek();
		CollectionElement actual = singleLinkedList().peek();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPeekFirst() throws Exception{
		CollectionElement expected = newPlainList().peekFirst();
		CollectionElement actual = singleLinkedList().peekFirst();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPeekLast() throws Exception{
		CollectionElement expected = newPlainList().peekLast();
		CollectionElement actual = singleLinkedList().peekLast();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPoll() throws Exception{
		CollectionElement expected = newPlainList().poll();
		CollectionElement actual = singleLinkedList().poll();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPollFirst() throws Exception{
		CollectionElement expected = newPlainList().pollFirst();
		CollectionElement actual = singleLinkedList().pollFirst();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPollLast() throws Exception{
		CollectionElement expected = newPlainList().pollLast();
		CollectionElement actual = singleLinkedList().pollLast();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPop() throws Exception{
		CollectionElement expected = newPlainList().pop();
		CollectionElement actual = singleLinkedList().pop();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testPush() throws Exception{
		Element element = new Element("last");
		singleLinkedList().push(element);
		reopen();
		Assert.areEqual(singleLinkedList().getFirst(), element);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRemove() throws Exception{
		CollectionElement expected = newPlainList().remove();
		CollectionElement actual = singleLinkedList().remove();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRemoveFirst() throws Exception{
		CollectionElement expected = newPlainList().removeFirst();
		CollectionElement actual = singleLinkedList().removeFirst();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRemoveFirstOccurrence() throws Exception{
		Element element = new Element("first");
		singleLinkedList().push(element);
		singleLinkedList().push(element);
		LinkedList<CollectionElement> newPlainList = newPlainList();
		newPlainList.push(element);
		newPlainList.push(element);
		reopen();
		newPlainList.removeFirstOccurrence(element);
		singleLinkedList().removeFirstOccurrence(element);
		reopen();
		Assert.areEqual(newPlainList.size(), singleLinkedList().size());
	}

	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRemoveLast() throws Exception{
		CollectionElement expected = newPlainList().removeLast();
		CollectionElement actual = singleLinkedList().removeLast();
		Assert.areEqual(expected, actual);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRemoveLastOccurrence() throws Exception{
		Element element = new Element("last");
		singleLinkedList().push(element);
		singleLinkedList().push(element);
		LinkedList<CollectionElement> newPlainList = newPlainList();
		newPlainList.push(element);
		newPlainList.push(element);
		reopen();
		newPlainList.removeLastOccurrence(element);
		singleLinkedList().removeLastOccurrence(element);
		reopen();
		Assert.areEqual(newPlainList.size(), singleLinkedList().size());
	}

	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRetainAll() throws Exception{
		Element element = new Element("first");
		singleLinkedList().push(element);
		LinkedList<CollectionElement> newPlainList = newPlainList();
		newPlainList.push(element);
		Assert.isTrue(newPlainList.size()>1);
		Collection<CollectionElement> excerpt = new ArrayList<CollectionElement>();
		excerpt.add(element);
		reopen();
		Assert.isTrue(newPlainList.retainAll(excerpt));
		Assert.isTrue(singleLinkedList().retainAll(excerpt));
		reopen();
		IteratorAssert.areEqual(newPlainList, singleLinkedList());
	}
	
}
