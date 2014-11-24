/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent;

import java.util.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class CollectionHolder <C extends Collection<CollectionElement>>{
	public C _collection;
}

