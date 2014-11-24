using Db4objects.Db4o.Monitoring;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    public class InstallPerformanceCounters
    {
        public static void Main(string[] args)
        {
            // #example: Install the performance counters
            Db4oPerformanceCounters.Install();
            // #end example
        }
    }
}