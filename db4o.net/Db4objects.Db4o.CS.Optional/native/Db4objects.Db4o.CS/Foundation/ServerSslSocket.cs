/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System.Security.Cryptography.X509Certificates;

namespace Db4objects.Db4o.CS.Foundation
{
	internal class ServerSslSocket : ServerSocket4Decorator
	{
		public ServerSslSocket(IServerSocket4 socket, X509Certificate2 certificate) : base(socket)
		{
			_certificate = certificate;
		}

		public override ISocket4 Accept()
		{
			ISocket4 socket = base.Accept();
			return new SslSocket(socket, _certificate);
		}
		
		private readonly X509Certificate2 _certificate;
	}
}

#endif