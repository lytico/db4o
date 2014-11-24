using System;
using System.Diagnostics;

namespace Db4oDoc.Code.Performance
{
    public static class StopWatchUtil
    {
        public static void Time(Action taskToTime)
        {
            var st = Stopwatch.StartNew();
            taskToTime();
            Console.Out.WriteLine("Time elapsed: {0}",st.ElapsedMilliseconds);
        }   
    }
}