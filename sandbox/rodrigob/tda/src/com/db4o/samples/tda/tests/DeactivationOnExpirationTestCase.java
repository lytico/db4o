package com.db4o.samples.tda.tests;

import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.ta.*;
import com.db4o.samples.tda.*;
import com.db4o.samples.tda.mocks.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeactivationOnExpirationTestCase extends AbstractDb4oTestCase {
	
	private static final int EXPECTED_DEACTIVATION_TIMEOUT = 1 * 60 * 1000;

	public static class Item extends ActivatableImpl {
		
		private String _name;
		
		public Item(String name) {
			_name = name;
		}
		
		public String name() {
			activate(ActivationPurpose.READ);
			return _name;
		}
	}
	
	private final TimerMock _timer = new TimerMock();
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.add(new DeactivationOnExpiration(_timer));
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item("foo"));
	}
	
	public void testObjectIsDeactivateAfterTimeout() {
		
		final Item item = (Item)retrieveOnlyInstance(Item.class);
		assertIsNotActive(item);
		
		forceActivation(item);
		
		_timer.advanceTime(EXPECTED_DEACTIVATION_TIMEOUT); // objects should expire after one minute
		
		assertIsNotActive(item);
	}

	public void testObjectIsNotDeactivatedIfReusedBeforeTimeout() {
		
		final Item item = (Item)retrieveOnlyInstance(Item.class);
		forceActivation(item);
		
		_timer.advanceTime(EXPECTED_DEACTIVATION_TIMEOUT - 1);
		
		forceActivation(item);
		
		_timer.advanceTime(EXPECTED_DEACTIVATION_TIMEOUT - 1);
		assertIsActive(item);
		
		_timer.advanceTime(1);
		assertIsNotActive(item);
		
	}
	
	private void assertIsNotActive(final Item item) {
		Assert.isFalse(isActive(item));
	}
	
	private void forceActivation(final Item item) {
	    Assert.areEqual("foo", item.name());
		assertIsActive(item);
    }

	private void assertIsActive(final Item item) {
	    Assert.isTrue(isActive(item));
    }
	

	private boolean isActive(Item item) {
	    return db().isActive(item);
    }

}
