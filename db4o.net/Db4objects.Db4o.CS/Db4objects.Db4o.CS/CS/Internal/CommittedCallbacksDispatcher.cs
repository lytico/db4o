/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	public class CommittedCallbacksDispatcher : IRunnable
	{
		private bool _stopped;

		private readonly BlockingQueue _committedInfosQueue;

		private readonly ObjectServerImpl _server;

		public CommittedCallbacksDispatcher(ObjectServerImpl server, BlockingQueue committedInfosQueue
			)
		{
			_server = server;
			_committedInfosQueue = committedInfosQueue;
		}

		public virtual void Run()
		{
			SetThreadName();
			MessageLoop();
		}

		private void MessageLoop()
		{
			while (!_stopped)
			{
				MCommittedInfo committedInfos;
				try
				{
					committedInfos = (MCommittedInfo)_committedInfosQueue.Next();
				}
				catch (BlockingQueueStoppedException)
				{
					break;
				}
				_server.BroadcastMsg(committedInfos, new _IBroadcastFilter_33());
			}
		}

		private sealed class _IBroadcastFilter_33 : IBroadcastFilter
		{
			public _IBroadcastFilter_33()
			{
			}

			public bool Accept(IServerMessageDispatcher dispatcher)
			{
				return dispatcher.CaresAboutCommitted();
			}
		}

		private void SetThreadName()
		{
			Thread.CurrentThread().SetName("committed callback thread");
		}

		public virtual void Stop()
		{
			_committedInfosQueue.Stop();
			_stopped = true;
		}
	}
}
