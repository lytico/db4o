/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Monitoring;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Tests.Optional.Monitoring.CS;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class ClientNetworkingPerformanceCounterTestCase : PerformanceCounterTestCaseBase, IOptOutMultiSession
	{
        private const int ByteCount = 100;

		protected override void Configure(IConfiguration config)
		{
			IFileConfiguration fileConfig = Db4oLegacyConfigurationBridge.AsFileConfiguration((Config4Impl) config);
			fileConfig.Storage = new MemoryStorage();
		}

		public void TestBytesSent()
		{
			Action<ISocket4> operation = delegate(ISocket4 socket) { socket.Write(null, 0, ByteCount); };
			Func<CountingSocket4, double> expectatedValueRetriever = delegate(CountingSocket4 s) { return s.BytesSent(); };
            Func<IObjectContainer, double> actualValueRetriever = delegate(IObjectContainer container) { return PerformanceCounterSpec.NetBytesSentPerSec.PerformanceCounter(container).RawValue; };

			AssertCounter(operation, expectatedValueRetriever, actualValueRetriever);
		}

		public void TestBytesReceived()
		{
			Action<ISocket4> operation = delegate(ISocket4 socket) { socket.Read(null, 0, ByteCount); };
			Func<CountingSocket4, double> expectatedValueRetriever = delegate(CountingSocket4 s) { return s.BytesReceived(); };
            Func<IObjectContainer, double> actualValueRetriever = delegate(IObjectContainer container) { return PerformanceCounterSpec.NetBytesReceivedPerSec.PerformanceCounter(container).RawValue; };

			AssertCounter(operation, expectatedValueRetriever, actualValueRetriever);
		}

		private void AssertCounter(Action<ISocket4> operation, Func<CountingSocket4, double> expectedValueRetriever, Func<IObjectContainer, double> actualValueRetriever)
		{
			ObjectContainerBase container = (ObjectContainerBase) Db();
			container.WithEnvironment(delegate
			{
				CountingSocket4 countingSocket = new CountingSocket4(new NullSocket());
			    ISocket4 socket = new MonitoredClientSocket4(countingSocket);
			    
				operation(socket);

				Assert.AreEqual(ByteCount, expectedValueRetriever(countingSocket));
			    Assert.AreEqual(ByteCount, actualValueRetriever(container));
			});
		}
	}
}

#endif