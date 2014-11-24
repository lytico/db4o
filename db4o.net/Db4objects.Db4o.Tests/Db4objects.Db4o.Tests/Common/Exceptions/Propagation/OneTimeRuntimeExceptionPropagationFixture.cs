/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class OneTimeRuntimeExceptionPropagationFixture : OneTimeFatalExceptionPropagationFixtureBase
	{
		protected override Type ExceptionType()
		{
			return typeof(Exception);
		}

		public override void ThrowInitialException()
		{
			throw new Exception();
		}

		public override string Label()
		{
			return "non-fatal/oneRTE";
		}
	}
}
