/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Events.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ActivationEventsTestCase), typeof(CallbackTrackingTestCase
				), typeof(ClassRegistrationEventsTestCase), typeof(CreationEventsTestCase), typeof(
				DeleteEventOnClientTestCase), typeof(DeletionEventExceptionTestCase), typeof(DeletionEventsTestCase
				), typeof(EventArgsTransactionTestCase), typeof(EventCountTestCase), typeof(ExceptionInUpdatingCallbackCorruptionTestCase
				), typeof(ExceptionPropagationInEventsTestSuite), typeof(InstantiationEventsTestCase
				), typeof(ObjectContainerEventsTestCase), typeof(ObjectContainerOpenEventTestCase
				), typeof(QueryEventsTestCase), typeof(UpdateInCallbackThrowsTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(DeleteOnDeletingCallbackTestCase), typeof(OwnCommitCallbackFlaggedNetworkingTestSuite
				), typeof(QueryInCallBackCSCallback) };
		}
		#endif // !SILVERLIGHT
	}
}
