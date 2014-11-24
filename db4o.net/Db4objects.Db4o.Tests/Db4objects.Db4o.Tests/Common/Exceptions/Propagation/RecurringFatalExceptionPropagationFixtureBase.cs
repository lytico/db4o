/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public abstract class RecurringFatalExceptionPropagationFixtureBase : IExceptionPropagationFixture
	{
		protected static readonly string CloseMessage = "B";

		protected static readonly string InitialMessage = "A";

		public virtual void ThrowShutdownException()
		{
			Assert.Fail();
		}

		public virtual void AssertExecute(DatabaseContext context, TopLevelOperation op)
		{
			try
			{
				op.Apply(context);
				Assert.Fail();
			}
			catch (CompositeDb4oException exc)
			{
				Assert.AreEqual(2, exc._exceptions.Length);
				AssertExceptionMessage(exc, InitialMessage, 0);
				AssertExceptionMessage(exc, CloseMessage, 1);
			}
		}

		private void AssertExceptionMessage(CompositeDb4oException exc, string expected, 
			int idx)
		{
			Assert.AreEqual(expected, exc._exceptions[idx].Message);
		}

		protected abstract Type ExceptionType();

		public abstract string Label();

		public abstract void ThrowCloseException();

		public abstract void ThrowInitialException();
	}
}
