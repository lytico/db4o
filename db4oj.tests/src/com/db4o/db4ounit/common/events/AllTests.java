/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runAll();
	}

	protected Class[] testCases() {
		return composeTests(
				new Class[] {			
						ActivationEventsTestCase.class,
						CallbackTrackingTestCase.class, 
						ClassRegistrationEventsTestCase.class,
						CreationEventsTestCase.class,
						DeleteEventOnClientTestCase.class,
						DeletionEventExceptionTestCase.class,
						DeletionEventsTestCase.class,
						EventArgsTransactionTestCase.class,			
						EventCountTestCase.class,
						ExceptionInUpdatingCallbackCorruptionTestCase.class,
						ExceptionPropagationInEventsTestSuite.class,
						InstantiationEventsTestCase.class,
						ObjectContainerEventsTestCase.class,
						ObjectContainerOpenEventTestCase.class,
						QueryEventsTestCase.class,
						UpdateInCallbackThrowsTestCase.class,
				});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
				DeleteOnDeletingCallbackTestCase.class,
				OwnCommitCallbackFlaggedNetworkingTestSuite.class,
				QueryInCallBackCSCallback.class,
			};
	}

}
