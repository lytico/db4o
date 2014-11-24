/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

using System;
using System.Threading;

namespace Db4objects.Db4o.Foundation
{
#if CF
    public class Lock4
    {
        private volatile Thread lockedByThread;

        private volatile Thread waitReleased;
        private volatile Thread closureReleased;

    	readonly AutoResetEvent waitEvent = new AutoResetEvent(false);
    	readonly AutoResetEvent closureEvent = new AutoResetEvent(false);

		public void Awake()
		{
			AwakeWait();
		}

		public Object Run(IClosure4 closure4)
        {
			EnterClosure();
            try
            {
                return closure4.Run();
            }
            finally
            {
                AwakeClosure();
            }
        }

		public void Snooze(long timeout)
		{
			AwakeClosure();
			WaitWait(timeout);
			EnterClosure();
		}

		private void EnterClosure()
		{
			while (lockedByThread != Thread.CurrentThread)
			{
				while (!SetLock())
				{
					WaitClosure();
				}
			}
		}

		private void AwakeClosure()
        {
            lock (this)
            {
                RemoveLock();
                closureReleased = Thread.CurrentThread;
                closureEvent.Set();
                Thread.Sleep(0);
                if (closureReleased == Thread.CurrentThread)
                {
                    closureEvent.Reset();
                }
            }
        }

		private void AwakeWait()
		{
			lock (this)
			{
				waitReleased = Thread.CurrentThread;
				waitEvent.Set();
				Thread.Sleep(0);
				if (waitReleased == Thread.CurrentThread)
				{
					waitEvent.Reset();
				}
			}
		}

		private void WaitWait(long timeout)
        {
        	waitEvent.WaitOne((int) timeout, false);
            waitReleased = Thread.CurrentThread;
        }

        private void WaitClosure()
        {
            closureEvent.WaitOne();
            closureReleased = Thread.CurrentThread;
        }

        private bool SetLock()
        {
            lock (this)
            {
                if (lockedByThread == null)
                {
                    lockedByThread = Thread.CurrentThread;
                    return true;
                }
                return false;
            }
        }

        private void RemoveLock()
        {
            lock (this)
            {
                if (lockedByThread == Thread.CurrentThread)
                {
                    lockedByThread = null;
                }
            }
        }
    }
#endif
}