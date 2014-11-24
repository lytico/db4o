/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

#if !CF && !SILVERLIGHT

using System;
using System.Collections.Generic;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Tests.Optional.Monitoring.CS;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class MockServerSocket4 : IServerSocket4
	{
		public void SetSoTimeout(int timeout)
		{
		}

		public int GetLocalPort()
		{
			return 0xDB40;
		}

		public ISocket4 Accept()
		{
			CountingSocket4 accepted = new CountingSocket4(new NullSocket());
			_acceptedSockets.Add(accepted);
			return accepted;
		}

		public void Close()
		{
			throw new NotImplementedException();
		}

		public double BytesSent()
		{
			double total = 0.0;
			foreach (CountingSocket4 countingSocket in _acceptedSockets)
			{
				total += countingSocket.BytesSent();
			}

			return total;
		}

		public double BytesReceived()
		{
			double total = 0.0;
			foreach (CountingSocket4 countingSocket in _acceptedSockets)
			{
				total += countingSocket.BytesReceived();
			}

			return total;
		}

		public double MessagesSent()
		{
			double total = 0.0;
			foreach (CountingSocket4 countingSocket in _acceptedSockets)
			{
				total += countingSocket.MessagesSent();
			}

			return total;
		}

		private readonly IList<CountingSocket4> _acceptedSockets = new List<CountingSocket4>();
	}
}

#endif