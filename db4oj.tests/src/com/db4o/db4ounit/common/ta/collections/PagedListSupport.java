/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.ta.collections;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.events.*;
import com.db4o.internal.*;


/**
 * Configures the support for paged collections.
 */
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class PagedListSupport implements ConfigurationItem {

	public void apply(InternalObjectContainer db) {
		eventRegistry(db).updating().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				CancellableObjectEventArgs cancellable = (CancellableObjectEventArgs) args;
				if (cancellable.object() instanceof Page) {
					Page page = (Page) cancellable.object();
					if (!page.isDirty()) {
						cancellable.cancel();
					}
				}
			}
		});
	}
	
	private static EventRegistry eventRegistry(ObjectContainer db) {
		return EventRegistryFactory.forObjectContainer(db);
	}

	public void prepare(Configuration configuration) {
		// Nothing to do...
	}
}
