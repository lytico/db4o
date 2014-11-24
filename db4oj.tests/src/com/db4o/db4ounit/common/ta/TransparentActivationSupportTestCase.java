/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ta;

import com.db4o.activation.*;
import com.db4o.events.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.activation.*;

import db4ounit.*;

/**
 * 
 * @sharpen.partial
 *
 */
public class TransparentActivationSupportTestCase extends TransparentActivationTestCaseBase {

	public static void main(String[] args) {
		new TransparentActivationSupportTestCase().runAll();
	}
	
	public void testActivationDepth() {
		Assert.isInstanceOf(TransparentActivationDepthProviderImpl.class, stream().configImpl().activationDepthProvider());
	}
	
	/**
	 * 
	 * @sharpen.partial
	 *
	 */
	public static final class Item extends ActivatableImpl {
		public void update() {
			activate(ActivationPurpose.WRITE);
		}
	}
	
	public void testTransparentActivationDoesNotImplyTransparentUpdate() {
		final Item item = new Item();
		db().store(item);
		db().commit();
		
		item.update();
		final Collection4 updated = commitCapturingUpdatedObjects(db());
		Assert.areEqual(0, updated.size());
	}
	
	private Collection4 commitCapturingUpdatedObjects(
			final ExtObjectContainer container) {
		final Collection4 updated = new Collection4();
		eventRegistryFor(container).updated().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				ObjectEventArgs objectArgs = (ObjectEventArgs)args;
				updated.add(objectArgs.object());
			}
		});
		container.commit();
		return updated;
	}
}
