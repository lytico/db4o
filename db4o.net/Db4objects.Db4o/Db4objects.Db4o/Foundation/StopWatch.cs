/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	public class StopWatch
	{
		private long _started;

		private long _elapsed;

		public StopWatch()
		{
		}

		public virtual void Start()
		{
			_started = Runtime.CurrentTimeMillis();
		}

		public virtual void Stop()
		{
			_elapsed = Peek();
		}

		public virtual long Peek()
		{
			return Runtime.CurrentTimeMillis() - _started;
		}

		public virtual long Elapsed()
		{
			return _elapsed;
		}

		public static long Time(IBlock4 block)
		{
			Db4objects.Db4o.Foundation.StopWatch stopWatch = new Db4objects.Db4o.Foundation.StopWatch
				();
			stopWatch.Start();
			block.Run();
			stopWatch.Stop();
			return stopWatch.Elapsed();
		}
	}
}
