/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.typehandlers.internal;

import java.util.*;

import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public abstract class TreeSetTypeHandler
	implements InstantiatingTypeHandler, QueryableTypeHandler {
	
	public boolean descendsIntoMembers() {
		return true;
    }

	public void writeInstantiation(WriteContext context, Object obj) {
		final Comparator comparator = ((TreeSet)obj).comparator();
		context.writeObject(comparator);
	}
	
	public Object instantiate(ReadContext context) {
		final Comparator comparator = (Comparator)context.readObject();
		return create(comparator);
	}

	protected abstract TreeSet create(final Comparator comparator);

	public void activate(ReferenceActivationContext context) {
		// already handled by CollectionTypeHandler
	}
	
	public void write(WriteContext context, Object obj) {
		// already handled by CollectionTypeHandler
	}

	public void defragment(DefragmentContext context) {
		context.copyID();
	}

	public void delete(DeleteContext context) throws Db4oIOException {
		// TODO: when the TreeSet is deleted
		// the comparator should be deleted too
		// TODO: What to do about shared comparators?
		// context.deleteObject();
	}
}
