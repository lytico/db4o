/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public class ClientAsynchronousMessageProcessor : IRunnable
	{
		private readonly BlockingQueue _messageQueue;

		private bool _stopped;

		public ClientAsynchronousMessageProcessor(BlockingQueue messageQueue)
		{
			_messageQueue = messageQueue;
		}

		public virtual void Run()
		{
			while (!_stopped)
			{
				try
				{
					IClientSideMessage message = (IClientSideMessage)_messageQueue.Next();
					if (message != null)
					{
						message.ProcessAtClient();
					}
				}
				catch (BlockingQueueStoppedException)
				{
					break;
				}
			}
		}

		public virtual void StopProcessing()
		{
			_stopped = true;
		}
	}
}
