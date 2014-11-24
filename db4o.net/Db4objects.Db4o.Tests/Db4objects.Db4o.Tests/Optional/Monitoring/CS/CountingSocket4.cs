/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.Tests.Optional.Monitoring.CS
{
	public class CountingSocket4 : Socket4Decorator
	{
		private readonly object Lock = new object();

		public CountingSocket4(ISocket4 socket) : base(socket)
		{
		}

		/// <exception cref="System.IO.IOException"></exception>
		public override void Write(byte[] bytes, int offset, int count)
		{
			lock (Lock)
			{
				base.Write(bytes, offset, count);
				_bytesSent += count;
				_messagesSent++;
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesReceived = base.Read(buffer, offset, count);
			lock (Lock)
			{
				_bytesReceived += bytesReceived;
			}
			return bytesReceived;
		}

		public virtual double BytesSent()
		{
			lock (Lock)
			{
				return _bytesSent;
			}
		}

		public virtual double BytesReceived()
		{
			lock (Lock)
			{
				return _bytesReceived;
			}
		}

		public virtual double MessagesSent()
		{
			lock (Lock)
			{
				return _messagesSent;
			}
		}

		public virtual void ResetCount()
		{
			lock (Lock)
			{
				_bytesSent = 0.0;
				_bytesReceived = 0.0;
				_messagesSent = 0.0;
			}
		}

		private double _bytesSent;

		private double _bytesReceived;

		private double _messagesSent;
	}
}
#endif // !SILVERLIGHT
