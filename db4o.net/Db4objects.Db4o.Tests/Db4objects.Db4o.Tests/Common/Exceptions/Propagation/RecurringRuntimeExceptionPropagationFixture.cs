/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class RecurringRuntimeExceptionPropagationFixture : RecurringFatalExceptionPropagationFixtureBase
	{
		public override void ThrowInitialException()
		{
			throw new Exception(InitialMessage);
		}

		public override void ThrowCloseException()
		{
			throw new Exception(CloseMessage);
		}

		public override string Label()
		{
			return "fatal/recRTE";
		}

		protected override Type ExceptionType()
		{
			return typeof(Exception);
		}
	}
}
