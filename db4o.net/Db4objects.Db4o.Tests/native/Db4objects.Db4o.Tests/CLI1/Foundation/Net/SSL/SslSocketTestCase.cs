/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL
{
	public class SslSocketTestCase : ITestCase
	{
		private const string Message = "SslSocketFactory test case!";

		public void Test()
		{
			ISocket4Factory sslSocketFactory = new SslSocketFactory(new StandardSocket4Factory(), ServerCertificate());
			IServerSocket4 serverSocket = sslSocketFactory.CreateServerSocket(0);

			PassThroughSocketFactory clientEncryptedContentsSocketFactory = NewPassThroughSocketFactory();
			PassThroughSocketFactory clientPassThroughSocketFactory = new PassThroughSocketFactory(new SslSocketFactory(clientEncryptedContentsSocketFactory, ValidateServerCertificate));

			Thread clientThread = StartClient(serverSocket.GetLocalPort(), clientPassThroughSocketFactory);

			string msg = ReadString(serverSocket.Accept());

			Assert.AreEqual(Message, msg);
			AssertAreNotEqual(clientEncryptedContentsSocketFactory.LastClient.Written, clientPassThroughSocketFactory.LastClient.Written);

			clientThread.Join();
		}

		public void TestClientCertificateValidation()
		{
			ISocket4Factory sslSocketFactory = new SslSocketFactory(new StandardSocket4Factory(), ServerCertificate());
			IServerSocket4 serverSocket = sslSocketFactory.CreateServerSocket(0);
			
			ThreadPool.QueueUserWorkItem(delegate
			{
				serverSocket.Accept();
			});

			SslSocketFactory clientSocketFactory = new SslSocketFactory(new StandardSocket4Factory(), delegate { return false; });

			Assert.Expect(typeof (AuthenticationException), delegate
			{
				clientSocketFactory.CreateSocket("localhost", serverSocket.GetLocalPort());
			});
		}

		public void TestServerAcceptSocketTimeout()
		{
			Thread serverThread = new Thread(delegate(object serverSocket)
			{
				Assert.Expect(typeof(Exception), delegate
				{
					AsServerSocket(serverSocket).Accept();
				});
			});

			AssertTimeoutBehavior(serverThread, 20, new StandardSocket4Factory());
		}

		public void TestServerReadSocketTimeout()
		{
			Thread server = new Thread(delegate(object serverSocket)
			{
				ISocket4 client = AsServerSocket(serverSocket).Accept();
				client.SetSoTimeout(1000);
				Assert.Expect(typeof(Exception), delegate
				{
					client.Read(new byte[1], 0, 1);
				});
			});

			int serverTimeout = MinutesToMiliseconds(5);
			ISocket4Factory clientSocketFactory = new SslSocketFactory(new StandardSocket4Factory(), delegate { return true; });

			AssertTimeoutBehavior(server, serverTimeout, clientSocketFactory);
		}

		private void AssertTimeoutBehavior(Thread serverTrigger, int serverTimeout, ISocket4Factory clientSocketFactory)
		{
			ISocket4Factory sslSocketFactory = new SslSocketFactory(new StandardSocket4Factory(), ServerCertificate());
			IServerSocket4 serverSocket = sslSocketFactory.CreateServerSocket(0);
			serverSocket.SetSoTimeout(serverTimeout);

			serverTrigger.IsBackground = true;
			serverTrigger.Start(serverSocket);

			ISocket4 clientSocket = clientSocketFactory.CreateSocket("localhost", serverSocket.GetLocalPort());

			if (!serverTrigger.Join(MinutesToMiliseconds(2)))
			{
				serverTrigger.Abort();
				Assert.Fail("Server thread should have timedout.");
			}
		}

		private static IServerSocket4 AsServerSocket(object socket)
		{
			return (IServerSocket4)socket;
		}

		private static int MinutesToMiliseconds(int minutes)
		{
			return 1000 * 60 * minutes;
		}

		private static void AssertAreNotEqual(byte[] encrypted, byte[] plainText)
		{
			Assert.AreNotEqual(encrypted.Length, plainText.Length);
			int diffCount = 0;
			for (int i = 0; i < encrypted.Length && i < plainText.Length; i++)
			{
				if (encrypted[i] != plainText[i])
				{
					diffCount++;
				}
			}

			Assert.IsGreater(0, diffCount);
			TestPlatform.Out.WriteLine("Diff count {0} in {1} bytes.", diffCount, plainText.Length);
		}

		private static Thread StartClient(int port, ISocket4Factory factory)
		{
			Thread clientTread = new Thread(delegate()
			{
				ISocket4 clientSocket = factory.CreateSocket("localhost", port);
				SendString(clientSocket, Message);
			});

			clientTread.Name = "SslSocketTest thread";
			clientTread.Start();

			return clientTread;
		}

		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
		{
			return certificate.Subject.Contains("CN=ssl-test.db4o.anywere");
		}

		private static void SendString(ISocket4 clientSocket, string message)
		{
			byte[] data = BytesToSendFor(message);
			clientSocket.Write(data, 0, data.Length);
		}

		private static string ReadString(ISocket4 socket)
		{
			ByteArrayBuffer buffer = ReadBufferFrom(socket, Const4.IntLength);
			int marshalledStringSize = buffer.ReadInt();

			ByteArrayBuffer marshalledString = ReadBufferFrom(socket, marshalledStringSize);
			LatinStringIO io = new LatinStringIO();
			return io.ReadLengthAndString(marshalledString);
		}

		private static ByteArrayBuffer ReadBufferFrom(ISocket4 socket, int length)
		{
			byte[] buffer = ReadFrom(socket, length);
			return new ByteArrayBuffer(buffer);
		}

		private static byte[] ReadFrom(ISocket4 socket, int length)
		{
			byte[] buffer = new byte[length];
			socket.Read(buffer, 0, buffer.Length);
			return buffer;
		}

		private static byte[] BytesToSendFor(string message)
		{
			LatinStringIO io = new LatinStringIO();
			int marshalledStringLength = io.Length(message);
			ByteArrayBuffer buffer = new ByteArrayBuffer(marshalledStringLength + Const4.IntLength);

			buffer.WriteInt(marshalledStringLength);

			io.WriteLengthAndString(buffer, message);
			buffer.Seek(0);
			return buffer.ReadBytes(buffer.Length());
		}

		private static PassThroughSocketFactory NewPassThroughSocketFactory()
		{
			return new PassThroughSocketFactory(new StandardSocket4Factory());
		}

		private X509Certificate2 ServerCertificate()
		{
			if (_serverCertificate == null)
			{
				byte[] bytes = Certificates.CreateSelfSignCertificate("CN=ssl-test.db4o.anywere, OU=db4o", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), "db4o-ssl-test");
				_serverCertificate = new X509Certificate2(bytes, "db4o-ssl-test");
			}
			
			return _serverCertificate;
		}

		private X509Certificate2 _serverCertificate;
	}
}
#endif