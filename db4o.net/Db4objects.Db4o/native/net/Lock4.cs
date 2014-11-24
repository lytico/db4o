/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Threading;

#if !CF
namespace Db4objects.Db4o.Foundation
{
    public class Lock4
    {    
        public void Awake()
        {
            Monitor.Pulse(this);
        }

        public Object Run(IClosure4 closure)
        {
            lock (this)
            {
                return closure.Run();
            }
        }
    
        public void Snooze(long timeout)
        {
            Monitor.Wait(this, (int)timeout);
        }
    }
}
#endif
