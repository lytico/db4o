/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Runtime4TestCase : ITestCase
	{
		public virtual void TestLoopWithTimeoutReturnsWhenBlockIsFalse()
		{
			StopWatch watch = new AutoStopWatch();
			Runtime4.Retry(500, new _IClosure4_14());
			Assert.IsSmaller(500, watch.Peek());
		}

		private sealed class _IClosure4_14 : IClosure4
		{
			public _IClosure4_14()
			{
			}

			public object Run()
			{
				return true;
			}
		}

		public virtual void TestLoopWithTimeoutReturnsAfterTimeout()
		{
			StopWatch watch = new AutoStopWatch();
			Runtime4.Retry(500, new _IClosure4_24());
			watch.Stop();
			Assert.IsGreaterOrEqual(500, watch.Elapsed());
		}

		private sealed class _IClosure4_24 : IClosure4
		{
			public _IClosure4_24()
			{
			}

			public object Run()
			{
				return false;
			}
		}
	}
}
