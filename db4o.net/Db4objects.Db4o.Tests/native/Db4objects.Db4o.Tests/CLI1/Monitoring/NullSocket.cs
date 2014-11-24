/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT
using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class NullSocket : ISocket4
	{
		public void Close()
		{
		}

		public void Flush()
		{
		}

		public void SetSoTimeout(int timeout)
		{
		}

		public bool IsConnected()
		{
			return true;
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			return count;
		}

		public void Write(byte[] bytes, int offset, int count)
		{
		}

		public ISocket4 OpenParallelSocket()
		{
			return null;
		}
	}
}
#endif