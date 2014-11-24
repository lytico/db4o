/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Bench.Timing;

namespace Db4objects.Db4o.Bench.Timing
{
    public class TicksStopWatch
    {
        private long _started;

        private long _elapsed;

        private TicksTiming _timing;

        public TicksStopWatch()
        {
            _timing = new TicksTiming();
        }

        public virtual void Start()
        {
            _started = _timing.TicksTime();
        }

        public virtual void Stop()
        {
            _elapsed = _timing.TicksTime() - _started;
        }

        public virtual long Elapsed()
        {

            return _elapsed;
        }
    }
}
