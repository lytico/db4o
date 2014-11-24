/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ExceptionPropagationInEventsTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), ExceptionPropagationInEventsTestVariables
				.EventProvider };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ExceptionPropagationInEventsTestUnit) };
		}
	}
}
