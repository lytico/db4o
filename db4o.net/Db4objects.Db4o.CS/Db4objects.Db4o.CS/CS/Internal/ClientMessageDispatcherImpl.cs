/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	internal class ClientMessageDispatcherImpl : IRunnable, IClientMessageDispatcher
	{
		private ClientObjectContainer _container;

		private Socket4Adapter _socket;

		private readonly BlockingQueue _synchronousMessageQueue;

		private readonly BlockingQueue _asynchronousMessageQueue;

		private bool _isClosed;

		internal ClientMessageDispatcherImpl(ClientObjectContainer client, Socket4Adapter
			 a_socket, BlockingQueue synchronousMessageQueue, BlockingQueue asynchronousMessageQueue
			)
		{
			_container = client;
			_synchronousMessageQueue = synchronousMessageQueue;
			_asynchronousMessageQueue = asynchronousMessageQueue;
			_socket = a_socket;
		}

		public virtual bool IsMessageDispatcherAlive()
		{
			lock (this)
			{
				return !_isClosed;
			}
		}

		public virtual bool Close()
		{
			lock (this)
			{
				if (_isClosed)
				{
					return true;
				}
				_isClosed = true;
				if (_socket != null)
				{
					try
					{
						_socket.Close();
					}
					catch (Db4oIOException)
					{
					}
				}
				_synchronousMessageQueue.Stop();
				_asynchronousMessageQueue.Stop();
				return true;
			}
		}

		public virtual void Run()
		{
			MessageLoop();
			Close();
		}

		public virtual void MessageLoop()
		{
			while (IsMessageDispatcherAlive())
			{
				Msg message = null;
				try
				{
					message = Msg.ReadMessage(this, Transaction(), _socket);
				}
				catch (Db4oIOException exc)
				{
					if (DTrace.enabled)
					{
						DTrace.ClientMessageLoopException.Log(exc.ToString());
					}
					return;
				}
				if (message == null)
				{
					continue;
				}
				if (IsClientSideMessage(message))
				{
					_asynchronousMessageQueue.Add(message);
				}
				else
				{
					_synchronousMessageQueue.Add(message);
				}
			}
		}

		private bool IsClientSideMessage(Msg message)
		{
			return message is IClientSideMessage;
		}

		public virtual bool Write(Msg msg)
		{
			_container.Write(msg);
			return true;
		}

		private Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _container.Transaction;
		}
	}
}
