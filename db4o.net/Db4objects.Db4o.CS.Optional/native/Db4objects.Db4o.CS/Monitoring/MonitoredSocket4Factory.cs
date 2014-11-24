/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
    public class MonitoredSocket4Factory : ISocket4Factory
    {
        private readonly ISocket4Factory _socketFactory;

        public MonitoredSocket4Factory(ISocket4Factory socketFactory)
        {
            _socketFactory = socketFactory;
        }

        public ISocket4 CreateSocket(string hostName, int port)
        {
            return new MonitoredClientSocket4(_socketFactory.CreateSocket(hostName, port));
        }

        public IServerSocket4 CreateServerSocket(int port)
        {
            return new MonitoredServerSocket4(_socketFactory.CreateServerSocket(port));
        }
    }
}
#endif