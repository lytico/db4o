/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class ClientHeartbeat : IRunnable
	{
		private SimpleTimer _timer;

		private readonly ClientObjectContainer _container;

		public ClientHeartbeat(ClientObjectContainer container)
		{
			_container = container;
			_timer = new SimpleTimer(this, Frequency(container.ConfigImpl));
		}

		private int Frequency(Config4Impl config)
		{
			return Math.Min(config.TimeoutClientSocket(), config.TimeoutServerSocket()) / 4;
		}

		public virtual void Run()
		{
			_container.WriteMessageToSocket(Msg.Ping);
		}

		public virtual void Start()
		{
			_container.ThreadPool().Start("db4o client heartbeat", _timer);
		}

		public virtual void Stop()
		{
			if (_timer == null)
			{
				return;
			}
			_timer.Stop();
			_timer = null;
		}
	}
}
