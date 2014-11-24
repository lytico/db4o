package com.db4o.drs.test;

import static com.db4o.drs.test.data.SimpleEnum.*;

import com.db4o.drs.test.data.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
public class GenericEnumTestCase extends DrsTestCase {

	@Override
	public void tearDown() throws Exception {
		ONE.setValue(1);
		super.tearDown();
	}

	public void testBasicEnumReplication() {

		storeNewTo(a(), new SimpleEnumContainer(ONE));

		replicateAll(a().provider(), b().provider());

		SimpleEnumContainer containerInB = (SimpleEnumContainer) getOneInstance(b(), SimpleEnumContainer.class);
		Assert.areSame(ONE, containerInB.getValue());

	}

	public void testMutableEnumContainer() {

		storeNewTo(a(), new SimpleEnumContainer(ONE));

		replicateAll(a().provider(), b().provider());

		SimpleEnumContainer containerInB = (SimpleEnumContainer) getOneInstance(b(), SimpleEnumContainer.class);

		containerInB.setValue(TWO);
		updateTo(b(), containerInB);

		replicateAll(b().provider(), a().provider());

		SimpleEnumContainer containerInA = (SimpleEnumContainer) getOneInstance(a(), SimpleEnumContainer.class);
		Assert.areSame(TWO, containerInA.getValue());
	}

	/**
	 * The behavior ilustrated with this task is Broken. It is kept here just to document the behaviour.
	 */
	public void testMutableEnumStateIsBroken() {

		storeNewTo(a(), new SimpleEnumContainer(ONE));

		replicateAll(a().provider(), b().provider());
		
		SimpleEnumContainer containerInA = (SimpleEnumContainer) getOneInstance(a(), SimpleEnumContainer.class);

		containerInA.setOpaque(42);
		ONE.setValue(3);

		updateTo(a(), containerInA, ONE);

		replicateAll(a().provider(), b().provider());

		SimpleEnumContainer containerInB = (SimpleEnumContainer) getOneInstance(b(), SimpleEnumContainer.class);
		
		int value = containerInB.getValue().getValue();
		
		Assert.isTrue(value == 3 || value == 1); // <------- this should expect 3, not 1
	}

}
