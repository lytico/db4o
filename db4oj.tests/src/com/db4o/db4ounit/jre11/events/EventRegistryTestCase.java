/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.events;

import com.db4o.events.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class EventRegistryTestCase extends AbstractDb4oTestCase {
	
	public void testForObjectContainerReturnsSameInstance() {
		Assert.areSame(
				EventRegistryFactory.forObjectContainer(db()),
				EventRegistryFactory.forObjectContainer(db()));
	}

	public void testQueryEvents() {

		EventRegistry registry = EventRegistryFactory.forObjectContainer(db());

		EventRecorder recorder = new EventRecorder(fileSession().lock());
		
		registry.queryStarted().addListener(recorder);
		registry.queryFinished().addListener(recorder);

		
		Assert.areEqual(0, recorder.size());
		
		Query q = db().query();
		q.execute();
		
		Assert.areEqual(2, recorder.size());
		EventRecord e1 = recorder.get(0);
		Assert.areSame(registry.queryStarted(), e1.e);
		Assert.areSame(q, ((QueryEventArgs)e1.args).query());

		EventRecord e2 = recorder.get(1);
		Assert.areSame(registry.queryFinished(), e2.e);
		Assert.areSame(q, ((QueryEventArgs)e2.args).query());

		recorder.clear();

		registry.queryStarted().removeListener(recorder);
		registry.queryFinished().removeListener(recorder);

		db().query().execute();

		Assert.areEqual(0, recorder.size());
	}
}
