/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Monitoring
{
    public class NetworkingMonitoringSupport:IConfigurationItem
    {
        public void Prepare(IConfiguration configuration)
        {
            INetworkingConfiguration networkConfig = Db4oClientServerLegacyConfigurationBridge.AsNetworkingConfiguration(configuration);
            ISocket4Factory currentSocketFactory = networkConfig.SocketFactory;
            networkConfig.SocketFactory = new MonitoredSocket4Factory(currentSocketFactory);
        }

        public void Apply(IInternalObjectContainer container)
        {

        }
    }
}
#endif