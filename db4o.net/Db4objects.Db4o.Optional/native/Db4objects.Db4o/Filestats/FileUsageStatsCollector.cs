/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.Internal.Collections;

namespace Db4objects.Db4o.Filestats
{

    public partial class FileUsageStatsCollector
    {

        private void RegisterBigSetCollector()
        {
            MiscCollectors[typeof(BigSet<>).FullName] = new BigSetMiscCollector();
        }

    }
}
#endif