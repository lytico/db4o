/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public abstract class OneTimeFatalExceptionPropagationFixtureBase : IExceptionPropagationFixture
	{
		public void ThrowShutdownException()
		{
			Assert.Fail();
		}

		public virtual void ThrowCloseException()
		{
		}

		public virtual void AssertExecute(DatabaseContext context, TopLevelOperation op)
		{
			Assert.Expect(ExceptionType(), new _ICodeBlock_16(op, context));
			Assert.IsTrue(context.StorageIsClosed());
		}

		private sealed class _ICodeBlock_16 : ICodeBlock
		{
			public _ICodeBlock_16(TopLevelOperation op, DatabaseContext context)
			{
				this.op = op;
				this.context = context;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				op.Apply(context);
			}

			private readonly TopLevelOperation op;

			private readonly DatabaseContext context;
		}

		protected abstract Type ExceptionType();

		public abstract string Label();

		public abstract void ThrowInitialException();
	}
}
