/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	public class TimeoutBlockingQueue : PausableBlockingQueue, ITimeoutBlockingQueue4
	{
		private long expirationDate;

		private readonly long maxTimeToRemainPaused;

		public TimeoutBlockingQueue(long maxTimeToRemainPaused)
		{
			this.maxTimeToRemainPaused = maxTimeToRemainPaused;
		}

		public override bool Pause()
		{
			Reset();
			return base.Pause();
		}

		public virtual void Check()
		{
			long now = Runtime.CurrentTimeMillis();
			if (now > expirationDate)
			{
				Resume();
			}
		}

		public virtual void Reset()
		{
			expirationDate = Runtime.CurrentTimeMillis() + maxTimeToRemainPaused;
		}
	}
}
