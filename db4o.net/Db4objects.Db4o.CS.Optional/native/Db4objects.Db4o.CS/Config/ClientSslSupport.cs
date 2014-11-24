/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System.Net.Security;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.CS.Config
{
	public class ClientSslSupport : IClientConfigurationItem
	{
		public ClientSslSupport(RemoteCertificateValidationCallback validationCallback)
		{
			_validationCallback = validationCallback;	
		}

		public void Prepare(IClientConfiguration configuration)
		{
			configuration.Networking.SocketFactory = new SslSocketFactory(configuration.Networking.SocketFactory, _validationCallback);
		}

		public void Apply(IExtClient client)
		{
		}
		
		private readonly RemoteCertificateValidationCallback _validationCallback;
	}
}

#endif