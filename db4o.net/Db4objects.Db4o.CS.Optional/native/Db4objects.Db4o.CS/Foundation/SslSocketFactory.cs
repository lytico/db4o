/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Db4objects.Db4o.CS.Foundation
{
	public class SslSocketFactory : ISocket4Factory
	{
		public SslSocketFactory(ISocket4Factory delegating, X509Certificate2 certificate)
		{
			_certificate = certificate;
			_delegating = delegating;
		}

		public SslSocketFactory(ISocket4Factory delegating, RemoteCertificateValidationCallback validationCallback)
		{
			_delegating = delegating;
			_validationCallback = validationCallback;
		}

		public ISocket4 CreateSocket(string hostName, int port)
		{
			ISocket4 clientSocket = _delegating.CreateSocket(hostName, port);
			return new SslSocket(clientSocket, hostName, _validationCallback);
		}

		public IServerSocket4 CreateServerSocket(int port)
		{
			IServerSocket4 serverSocket = _delegating.CreateServerSocket(port);
			return new ServerSslSocket(serverSocket, _certificate);
		}

		private readonly ISocket4Factory _delegating;
		private readonly X509Certificate2 _certificate;
		private readonly RemoteCertificateValidationCallback _validationCallback;
	}
}

#endif