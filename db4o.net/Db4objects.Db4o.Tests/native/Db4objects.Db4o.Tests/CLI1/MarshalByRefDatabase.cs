/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !SILVERLIGHT
using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4objects.Db4o.Tests.CLI1
{
    /// <summary>
    /// A facade to an ObjectContainer executing in a different AppDomain.
    /// </summary>
    public class MarshalByRefDatabase : MarshalByRefObject, IDisposable
    {
        protected IObjectServer _server;
        protected IObjectContainer _container;

        public void Open(string fname, bool clientServer)
        {
            if (clientServer)
            {
                _server = Db4oClientServer.OpenServer(fname, 0);
                _container = _server.OpenClient();
            }
            else
            {
                _container = Db4oFactory.OpenFile(fname);
            }
        }

        public void Dispose()
        {
            if (null != _container)
            {
                _container.Close();
                _container = null;
            }
            if (null != _server)
            {
                _server.Close();
                _server = null;
            }
            // MAGIC: give some time for the db4o background threads to exit
            System.Threading.Thread.Sleep(1000);
        }
    }
}
#endif