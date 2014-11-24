/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Fixtures;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public interface IExceptionPropagationFixture : ILabeled
	{
		void ThrowInitialException();

		void ThrowShutdownException();

		void ThrowCloseException();

		void AssertExecute(DatabaseContext context, TopLevelOperation op);
	}
}
