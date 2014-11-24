/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public class OutOfMemoryErrorPropagationFixture : IExceptionPropagationFixture
	{
		public virtual void ThrowInitialException()
		{
			throw new OutOfMemoryException();
		}

		public virtual void ThrowShutdownException()
		{
			Assert.Fail();
		}

		public virtual void ThrowCloseException()
		{
			Assert.Fail();
		}

		public virtual void AssertExecute(DatabaseContext context, TopLevelOperation op)
		{
			Assert.Expect(typeof(OutOfMemoryException), new _ICodeBlock_21(op, context));
			Assert.IsFalse(context.StorageIsClosed());
		}

		private sealed class _ICodeBlock_21 : ICodeBlock
		{
			public _ICodeBlock_21(TopLevelOperation op, DatabaseContext context)
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

		public virtual string Label()
		{
			return "OOME";
		}
	}
}
