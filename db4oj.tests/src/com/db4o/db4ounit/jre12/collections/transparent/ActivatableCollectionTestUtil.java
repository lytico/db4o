/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.jre12.collections.transparent;

import com.db4o.*;
import com.db4o.query.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public final class ActivatableCollectionTestUtil {

	public static long anyActivatableElementId(ObjectContainer db) {
		return allActivatableElementIds(db)[0];
	}


	public static long[] allActivatableElementIds(ObjectContainer db) {
		Query q = db.query();
		q.constrain(ActivatableElement.class);
		ObjectSet<Object> objectSet = q.execute();
		return objectSet.ext().getIDs();
	}

	
	private ActivatableCollectionTestUtil() {
	}
}
