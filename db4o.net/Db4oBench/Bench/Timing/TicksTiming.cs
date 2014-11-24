/* Copyright (C) 2004 - 2008 Versant Inc. http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Bench.Timing
{

    public class TicksTiming
    {


        public long TicksTime()
        {
            return System.DateTime.Now.Ticks;
        }

        public void WaitTicks(long ticks)
        {
            long target = System.DateTime.Now.Ticks + ticks;
            while (System.DateTime.Now.Ticks <= target)
            {
            }
        }

    }
}
