/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System.Diagnostics;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Monitoring
{
    public class PerformanceCounterSpec
    {
        private readonly string _id;

        private readonly string _description;

        private readonly PerformanceCounterType _counterType;

        private PerformanceCounterSpec(string id, string description, PerformanceCounterType counterType)
        {
            _id = id;
            _description = description;
            _counterType = counterType;
        }

        public static readonly PerformanceCounterSpec BytesWrittenPerSec = new PerformanceCounterSpec("bytes written/sec",
                        "Bytes written per second",
                        PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec BytesReadPerSec = new PerformanceCounterSpec("bytes read/sec",
                                "Bytes read per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec ObjectsStoredPerSec = new PerformanceCounterSpec("objects stored/sec",
                                "Number of objects stored per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec ObjectsDeletedPerSec = new PerformanceCounterSpec("objects deleted/sec",
                                "Number of objects deleted per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec ObjectsActivatedPerSec = new PerformanceCounterSpec("objects activated/sec",
                                "Number of objects activated per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec ObjectsDeactivatedPerSec = new PerformanceCounterSpec("objects deactivated/sec",
                        "Number of objects deactivated per second",
                        PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec QueriesPerSec = new PerformanceCounterSpec("queries/sec",
                                "Number of queries executed per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec ClassIndexScansPerSec = new PerformanceCounterSpec("class index scans/sec",
                                "Number of queries that could not use field indexes and had to fall back to class index scans per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);


        public static readonly PerformanceCounterSpec NativeQueriesPerSec = new PerformanceCounterSpec("native queries/sec",
                                "Number of native queries executed per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec UnoptimizedNativeQueriesPerSec = new PerformanceCounterSpec("unoptimized native queries/sec",
                                "Number of unoptimized native queries executed per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec NetBytesSentPerSec = new PerformanceCounterSpec("network bytes sent/sec",
                                "Number of bytes sent per second through the socket layer",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec NetBytesReceivedPerSec = new PerformanceCounterSpec("network bytes received/sec",
                                 "Number of bytes received per second through the socket layer",
                                 PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec NetMessagesSentPerSec = new PerformanceCounterSpec("network messages sent/sec",
                                 "Number of messages per second through the socket layer",
                                 PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec NetClientConnections = new PerformanceCounterSpec("number of connected clients",
                                 "Number of connected clients",
                                 PerformanceCounterType.NumberOfItems32);

        public static readonly PerformanceCounterSpec FreespaceAverageSlotSize = new PerformanceCounterSpec("average freespace slot size",
                                 "Average freespace slot size in bytes",
                                 PerformanceCounterType.NumberOfItems32);

        public static readonly PerformanceCounterSpec FreespaceReusedSlotsPerSec = new PerformanceCounterSpec("reused freespace slots/sec",
                                 "Reused freespace slots per second",
                                 PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec FreespaceSlotCount = new PerformanceCounterSpec("number of freespace slots",
                                 "Number of freespace slots",
                                 PerformanceCounterType.NumberOfItems32);

        public static readonly PerformanceCounterSpec TotalFreespace = new PerformanceCounterSpec("total freespace",
                                 "Total freespace in bytes",
                                 PerformanceCounterType.NumberOfItems32);

        public static PerformanceCounterSpec ObjectReferenceCount = new PerformanceCounterSpec("number of object references",
                                 "Number of object references",
                                 PerformanceCounterType.NumberOfItems32);


        public static readonly PerformanceCounterSpec LinqQueriesPerSec = new PerformanceCounterSpec("linq queries/sec",
                                "Number of Linq queries executed per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public static readonly PerformanceCounterSpec UnoptimizedLinqQueriesPerSec = new PerformanceCounterSpec("unoptimized linq queries/sec",
                                "Number of unoptimized Linq queries executed per second",
                                PerformanceCounterType.RateOfCountsPerSecond32);

        public string Id
        {
            get { return _id; }
        }

        public static PerformanceCounterSpec[] All()
        {
            return new PerformanceCounterSpec[]
                       {
                           BytesWrittenPerSec,
                           BytesReadPerSec,
                           ObjectsStoredPerSec,
                           ObjectsDeletedPerSec,
                           ObjectsActivatedPerSec,
                           ObjectsDeactivatedPerSec,
                           QueriesPerSec,
                           ClassIndexScansPerSec,
                           NativeQueriesPerSec,
                           UnoptimizedNativeQueriesPerSec,
                           NetBytesSentPerSec,
                           NetBytesReceivedPerSec,
                           NetMessagesSentPerSec,
                           NetClientConnections,
                           FreespaceAverageSlotSize,
                           FreespaceReusedSlotsPerSec,
                           FreespaceSlotCount,
                           TotalFreespace,
                           ObjectReferenceCount,
                           LinqQueriesPerSec,
                           UnoptimizedLinqQueriesPerSec,
                       };
            
        }

        public CounterCreationData CounterCreationData()
        {
            return new CounterCreationData(_id, _description, _counterType);
        }

        public PerformanceCounter PerformanceCounter(IObjectContainer container)
        {
            return Db4oPerformanceCounters.CounterFor(this, container, true);
        }

        public PerformanceCounter PerformanceCounter()
        {
            return PerformanceCounter(My<IObjectContainer>.Instance);
        }

    }
}
#endif