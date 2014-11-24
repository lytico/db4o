/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class TimeoutBlockingQueueTestCase : Queue4TestCaseBase
	{
		public virtual void TestTimeoutNext()
		{
			ITimeoutBlockingQueue4 queue = new TimeoutBlockingQueue(300);
			queue.Pause();
			queue.Add(new object());
			queue.Check();
			Assert.IsNull(queue.TryNext());
			Runtime4.SleepThrowsOnInterrupt(500);
			queue.Check();
			Assert.IsNotNull(queue.TryNext());
		}
	}
}
