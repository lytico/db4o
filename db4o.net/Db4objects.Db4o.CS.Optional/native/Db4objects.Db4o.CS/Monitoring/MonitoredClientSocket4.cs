/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
	public class MonitoredClientSocket4 : MonitoredSocket4Base
	{
		public MonitoredClientSocket4(ISocket4 socket) : base(socket)
		{
		}

		public override void Close()
		{
			base.Close();
			_counters.Close();
		}

		protected override NetworkingCounters Counters()
		{
			if (null == _counters)
			{
				_counters = new NetworkingCounters();
			}
			return _counters;
		}

		private NetworkingCounters _counters;
	}
}

#endif