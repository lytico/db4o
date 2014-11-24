/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Monitoring;
using Db4objects.Db4o.Monitoring;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class ServerNetworkingPerformanceCounterTestCase : PerformanceCounterTestCaseBase, IOptOutAllButNetworkingCS
	{
		private const int ClientCount = 10;

		private delegate void SocketOperation(ISocket4 socket, int byteCount);

		public void TestBytesSent()
		{
			SocketOperation operation = delegate(ISocket4 client, int byteCount) { client.Write(null, 0, byteCount); };
			Func<PerformanceCounter> counterRetriever = delegate { return PerformanceCounterSpec.NetBytesSentPerSec.PerformanceCounter(FileSession()); };
			Func<MockServerSocket4, long> delegatingRetriever = delegate(MockServerSocket4 mockServerSocket) { return (long) mockServerSocket.BytesSent(); };

			AssertCounter(operation, delegatingRetriever, counterRetriever, ExpectedBytesCount());
		}

		public void TestBytesReceived()
		{
			SocketOperation operation = delegate(ISocket4 client, int byteCount) { client.Read(null, 0, byteCount); };
            Func<PerformanceCounter> counterRetriever = delegate { return PerformanceCounterSpec.NetBytesReceivedPerSec.PerformanceCounter(FileSession()); };
			Func<MockServerSocket4, long> delegatingRetriever = delegate(MockServerSocket4 mockServerSocket) { return (long) mockServerSocket.BytesReceived(); };

			AssertCounter(operation, delegatingRetriever, counterRetriever, ExpectedBytesCount());
		}

		public void TestMessagesSent()
		{
			SocketOperation operation = delegate(ISocket4 client, int byteCount) { client.Write(null, 0, 1); };
			Func<PerformanceCounter> counterRetriever = delegate { return PerformanceCounterSpec.NetMessagesSentPerSec.PerformanceCounter(FileSession()); };
			Func<MockServerSocket4, long> delegatingRetriever = delegate(MockServerSocket4 mockServerSocket) { return (long)mockServerSocket.MessagesSent(); };

			AssertCounter(operation, delegatingRetriever, counterRetriever, ClientCount);
		}

		private void AssertCounter(SocketOperation operation, Func<MockServerSocket4, long> delegatingRetriever, Func<PerformanceCounter> counterRetriever, int expectedCount)
		{
			MockServerSocket4 mockServerSocket = new MockServerSocket4();
			IServerSocket4 serverSocket = new MonitoredServerSocket4(mockServerSocket);

			FileSession().WithEnvironment(
				delegate
					{
						IList<ISocket4> clients = new List<ISocket4>();
						for (int i = 0; i < ClientCount; i++)
						{
							clients.Add(serverSocket.Accept());
						}

						for (int i = 0; i < clients.Count; i++)
						{
							operation(clients[i], i);
						}
					});

			Assert.AreEqual(expectedCount, delegatingRetriever(mockServerSocket));
			Assert.AreEqual(expectedCount, counterRetriever().RawValue);
		}

		private static int ExpectedBytesCount()
		{
			return ClientCount * (ClientCount - 1) / 2;
		}
	}
}

#endif