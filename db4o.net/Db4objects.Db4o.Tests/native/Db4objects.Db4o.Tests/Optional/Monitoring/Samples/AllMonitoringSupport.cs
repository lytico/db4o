/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

#if !CF && !SILVERLIGHT

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.Tests.Optional.Monitoring.Samples
{
	public class AllMonitoringSupport
	{
		public virtual void Apply(ICommonConfigurationProvider config)
		{
            config.Common.Add(new IOMonitoringSupport());
            config.Common.Add(new QueryMonitoringSupport());
            config.Common.Add(new NativeQueryMonitoringSupport());
            config.Common.Add(new ReferenceSystemMonitoringSupport());
            config.Common.Add(new FreespaceMonitoringSupport());
            //config.Common.Add(new NetworkingMonitoringSupport());
            config.Common.Add(new ObjectLifecycleMonitoringSupport());
		}
	}
}
#endif 
