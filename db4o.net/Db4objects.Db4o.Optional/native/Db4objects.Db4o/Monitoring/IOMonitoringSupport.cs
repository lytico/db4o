/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Monitoring
{
    public class IOMonitoringSupport : IConfigurationItem
    {
        public void Prepare(IConfiguration configuration)
        {
            configuration.Storage = new MonitoredStorage(configuration.Storage);
        }

        public void Apply(IInternalObjectContainer container)
        {
            
        }
    }
}
#endif
