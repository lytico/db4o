/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	public sealed class ServerMessageDispatcherImpl : IServerMessageDispatcher, IRunnable
	{
		private string _clientName;

		private bool _loggedin;

		private bool _closeMessageSent;

		private readonly ObjectServerImpl _server;

		private Socket4Adapter _socket;

		private readonly ClientTransactionHandle _transactionHandle;

		private Hashtable4 _queryResults;

		internal readonly int _threadID;

		private CallbackObjectInfoCollections _committedInfo;

		private bool _caresAboutCommitted;

		private bool _isClosed;

		private readonly object _lock = new object();

		private readonly object _mainLock;

		private System.EventHandler<MessageEventArgs> _messageReceived;

		private Sharpen.Lang.Thread _thread;

		/// <exception cref="System.Exception"></exception>
		internal ServerMessageDispatcherImpl(ObjectServerImpl server, ClientTransactionHandle
			 transactionHandle, ISocket4 socket4, int threadID, bool loggedIn, object mainLock
			)
		{
			_mainLock = mainLock;
			_transactionHandle = transactionHandle;
			_loggedin = loggedIn;
			_server = server;
			_threadID = threadID;
			_socket = new Socket4Adapter(socket4);
			_socket.SetSoTimeout(((Config4Impl)server.Configure()).TimeoutServerSocket());
		}

		// TODO: Experiment with packetsize and noDelay
		// i_socket.setSendBufferSize(100);
		// i_socket.setTcpNoDelay(true);
		public bool Close()
		{
			return Close(ShutdownMode.Normal);
		}

		public bool Close(ShutdownMode mode)
		{
			lock (_lock)
			{
				if (!IsMessageDispatcherAlive())
				{
					return true;
				}
				_isClosed = true;
			}
			lock (_mainLock)
			{
				_transactionHandle.ReleaseTransaction(mode);
				if (!mode.IsFatal())
				{
					SendCloseMessage();
				}
				_transactionHandle.Close(mode);
				CloseSocket();
				RemoveFromServer();
				return true;
			}
		}

		public void CloseConnection()
		{
			lock (_lock)
			{
				if (!IsMessageDispatcherAlive())
				{
					return;
				}
				_isClosed = true;
			}
			lock (_mainLock)
			{
				CloseSocket();
				RemoveFromServer();
			}
		}

		public bool IsMessageDispatcherAlive()
		{
			lock (_lock)
			{
				return !_isClosed;
			}
		}

		private void SendCloseMessage()
		{
			try
			{
				if (!_closeMessageSent)
				{
					_closeMessageSent = true;
					Write(Msg.Close);
				}
			}
			catch (Exception e)
			{
			}
		}

		private void RemoveFromServer()
		{
			try
			{
				_server.RemoveThread(this);
			}
			catch (Exception e)
			{
			}
		}

		private void CloseSocket()
		{
			try
			{
				if (_socket != null)
				{
					_socket.Close();
				}
			}
			catch (Db4oIOException e)
			{
			}
		}

		public Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _transactionHandle.Transaction();
		}

		public void Run()
		{
			_thread = Sharpen.Lang.Thread.CurrentThread();
			try
			{
				SetDispatcherName(string.Empty + _threadID);
				_server.WithEnvironment(new _IRunnable_152(this));
			}
			finally
			{
				Close();
			}
		}

		private sealed class _IRunnable_152 : IRunnable
		{
			public _IRunnable_152(ServerMessageDispatcherImpl _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.MessageLoop();
			}

			private readonly ServerMessageDispatcherImpl _enclosing;
		}

		private void MessageLoop()
		{
			while (IsMessageDispatcherAlive())
			{
				try
				{
					if (!MessageProcessor())
					{
						return;
					}
				}
				catch (Db4oIOException e)
				{
					if (DTrace.enabled)
					{
						DTrace.AddToClassIndex.Log(e.ToString());
					}
					return;
				}
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private bool MessageProcessor()
		{
			Msg message = Msg.ReadMessage(this, Transaction(), _socket);
			if (message == null)
			{
				return true;
			}
			TriggerMessageReceived(message);
			if (!_loggedin && !Msg.Login.Equals(message))
			{
				return true;
			}
			// TODO: COR-885 - message may process against closed server
			// Checking aliveness just makes the issue less likely to occur. Naive synchronization against main lock is prohibitive.        
			return ProcessMessage(message);
		}

		public bool ProcessMessage(Msg message)
		{
			if (IsMessageDispatcherAlive())
			{
				if (message is IMessageWithResponse)
				{
					IMessageWithResponse msgWithResp = (IMessageWithResponse)message;
					try
					{
						Msg reply = msgWithResp.ReplyFromServer();
						Write(reply);
					}
					catch (Db4oRecoverableException exc)
					{
						WriteException(message, exc);
						return true;
					}
					catch (Exception t)
					{
						Sharpen.Runtime.PrintStackTrace(t);
						FatalShutDownServer(t);
						return false;
					}
					try
					{
						msgWithResp.PostProcessAtServer();
						return true;
					}
					catch (Exception exc)
					{
						Sharpen.Runtime.PrintStackTrace(exc);
					}
					return true;
				}
				try
				{
					((IServerSideMessage)message).ProcessAtServer();
					return true;
				}
				catch (Db4oRecoverableException exc)
				{
					Sharpen.Runtime.PrintStackTrace(exc);
					return true;
				}
				catch (Exception t)
				{
					Sharpen.Runtime.PrintStackTrace(t);
					FatalShutDownServer(t);
				}
			}
			return false;
		}

		private void FatalShutDownServer(Exception origExc)
		{
			new FatalServerShutdown(_server, origExc);
		}

		private void WriteException(Msg message, Exception exc)
		{
			if (!(message is IMessageWithResponse))
			{
				Sharpen.Runtime.PrintStackTrace(exc);
				return;
			}
			if (!(exc is Exception))
			{
				exc = new Db4oException(exc);
			}
			// Writing exceptions can produce ClassMetadata in
			// the main ObjectContainer.
			lock (_mainLock)
			{
				message.WriteException((Exception)exc);
			}
		}

		private void TriggerMessageReceived(IMessage message)
		{
			if (null != _messageReceived) _messageReceived(null, new MessageEventArgs(message
				));
		}

		public ObjectServerImpl Server()
		{
			return _server;
		}

		public void QueryResultFinalized(int queryResultID)
		{
			_queryResults.Remove(queryResultID);
		}

		public void MapQueryResultToID(LazyClientObjectSetStub stub, int queryResultID)
		{
			if (_queryResults == null)
			{
				_queryResults = new Hashtable4();
			}
			_queryResults.Put(queryResultID, stub);
		}

		public LazyClientObjectSetStub QueryResultForID(int queryResultID)
		{
			return (LazyClientObjectSetStub)_queryResults.Get(queryResultID);
		}

		public void SwitchToFile(MSwitchToFile message)
		{
			lock (_mainLock)
			{
				string fileName = message.ReadString();
				try
				{
					_transactionHandle.ReleaseTransaction(ShutdownMode.Normal);
					_transactionHandle.AcquireTransactionForFile(fileName);
					Write(Msg.Ok);
				}
				catch (Exception e)
				{
					_transactionHandle.ReleaseTransaction(ShutdownMode.Normal);
					Write(Msg.Error);
				}
			}
		}

		public void SwitchToMainFile()
		{
			lock (_mainLock)
			{
				_transactionHandle.ReleaseTransaction(ShutdownMode.Normal);
				Write(Msg.Ok);
			}
		}

		public void UseTransaction(MUseTransaction message)
		{
			int threadID = message.ReadInt();
			Db4objects.Db4o.Internal.Transaction transToUse = _server.FindTransaction(threadID
				);
			_transactionHandle.Transaction(transToUse);
		}

		public bool Write(Msg msg)
		{
			lock (_lock)
			{
				if (!IsMessageDispatcherAlive())
				{
					return false;
				}
				return msg.Write(_socket);
			}
		}

		public Socket4Adapter Socket()
		{
			return _socket;
		}

		public string Name
		{
			get
			{
				return _clientName;
			}
		}

		public void SetDispatcherName(string name)
		{
			_clientName = name;
			Thread().SetName("db4o server message dispatcher " + name);
		}

		public int DispatcherID()
		{
			return _threadID;
		}

		public void Login()
		{
			_loggedin = true;
		}

		public bool CaresAboutCommitted()
		{
			return _caresAboutCommitted;
		}

		public void CaresAboutCommitted(bool care)
		{
			_caresAboutCommitted = true;
			Server().CheckCaresAboutCommitted();
		}

		public CallbackObjectInfoCollections CommittedInfo()
		{
			return _committedInfo;
		}

		public void DispatchCommitted(CallbackObjectInfoCollections committedInfo)
		{
			_committedInfo = committedInfo;
		}

		public bool WillDispatchCommitted()
		{
			return Server().CaresAboutCommitted();
		}

		public Db4objects.Db4o.CS.Internal.ClassInfoHelper ClassInfoHelper()
		{
			return Server().ClassInfoHelper();
		}

		/// <summary>EventArgs =&gt; MessageEventArgs</summary>
		public event System.EventHandler<MessageEventArgs> MessageReceived
		{
			add
			{
				_messageReceived = (System.EventHandler<MessageEventArgs>)System.Delegate.Combine
					(_messageReceived, value);
			}
			remove
			{
				_messageReceived = (System.EventHandler<MessageEventArgs>)System.Delegate.Remove(
					_messageReceived, value);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public void Join()
		{
			Thread().Join();
		}

		private Sharpen.Lang.Thread Thread()
		{
			if (null == _thread)
			{
				throw new InvalidOperationException();
			}
			return _thread;
		}
	}
}
