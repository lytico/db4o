/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre11.events;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

    public static void main(String[] args) {
        new AllTests().runAll();
    }

    protected Class[] testCases() {
        return new Class[] {
        	CommittingCallbacksForClientServerTestCase.class,
        	CommittingCallbacksTestCase.class,
        	CommittedCallbacksByAnotherClientTestCase.class,
        	CommittedCallbacksTestCase.class,
            EventRegistryTestCase.class,
            GlobalLifecycleEventsTestCase.class,
            SelectiveCascadingDeleteTestCase.class,
            SelectiveActivationTestCase.class
        };
    }
}
