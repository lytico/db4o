/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
	public abstract class MonitoredSocket4Base : Socket4Decorator
	{
		protected MonitoredSocket4Base(ISocket4 socket) : base(socket)
		{
		}

		public override void Write(byte[] bytes, int offset, int count)
		{
			base.Write(bytes, offset, count);
			
			Counters().BytesSent().IncrementBy(count);
			Counters().MessagesSent().Increment();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesReceived = base.Read(buffer, offset, count);
			Counters().BytesReceived().IncrementBy(bytesReceived);

			return bytesReceived;
		}

		protected abstract NetworkingCounters Counters();
	}
}

#endif