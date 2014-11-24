/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Mocking;
using Db4objects.Db4o;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class RollbackStrategyMock : IRollbackStrategy
	{
		private MethodCallRecorder _recorder = new MethodCallRecorder();

		public virtual void Rollback(IObjectContainer container, object obj)
		{
			_recorder.Record(new MethodCall("rollback", new object[] { container, obj }));
		}

		public virtual void Verify(MethodCall[] expectedCalls)
		{
			_recorder.Verify(expectedCalls);
		}

		public virtual void VerifyUnordered(MethodCall[] methodCalls)
		{
			_recorder.VerifyUnordered(methodCalls);
		}
	}
}
