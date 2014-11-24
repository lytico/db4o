/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System.Security.Cryptography.X509Certificates;
using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Config
{
	public class ServerSslSupport : IServerConfigurationItem
	{
		public ServerSslSupport(X509Certificate2 certificate)
		{
			_certificate = certificate;
		}

		public void Prepare(IServerConfiguration configuration)
		{
			configuration.Networking.SocketFactory = new SslSocketFactory(configuration.Networking.SocketFactory, _certificate);
		}

		public void Apply(IObjectServer server)
		{
		}
		
		private readonly X509Certificate2 _certificate;
	}
}

#endif