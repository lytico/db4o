/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class RecurringDb4oExceptionPropagationFixture : RecurringFatalExceptionPropagationFixtureBase
	{
		public override void ThrowInitialException()
		{
			throw new Db4oException(InitialMessage);
		}

		public override void ThrowCloseException()
		{
			throw new Db4oException(CloseMessage);
		}

		public override string Label()
		{
			return "fatal/recDb4o";
		}

		protected override Type ExceptionType()
		{
			return typeof(Db4oException);
		}
	}
}
