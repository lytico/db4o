/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.CS;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation.CS;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation.CS
{
	public class MsgExceptionHandlingTestCase : ClientServerTestCaseBase, IOptOutAllButNetworkingCS
	{
		private static readonly string ExceptionMessage = "exc";

		private class CloseAwareBin : BinDecorator
		{
			private readonly MsgExceptionHandlingTestCase.CloseAwareStorage _storage;

			public CloseAwareBin(MsgExceptionHandlingTestCase.CloseAwareStorage storage, IBin
				 bin) : base(bin)
			{
				_storage = storage;
			}

			public override void Close()
			{
				base.Close();
				_storage.NotifyClosed(this);
			}

			public override void Sync()
			{
				base.Sync();
				_storage.NotifySyncInvocation();
			}

			public override void Sync(IRunnable runnable)
			{
				base.Sync(runnable);
				_storage.NotifySyncInvocation();
			}
		}

		private class CloseAwareStorage : StorageDecorator
		{
			private readonly IDictionary _openBins = new Hashtable();

			private bool _syncAllowed = true;

			private bool _illegalSyncInvocation = false;

			public CloseAwareStorage(IStorage storage) : base(storage)
			{
			}

			protected override IBin Decorate(BinConfiguration config, IBin bin)
			{
				MsgExceptionHandlingTestCase.CloseAwareBin decorated = new MsgExceptionHandlingTestCase.CloseAwareBin
					(this, bin);
				lock (_openBins)
				{
					_openBins[decorated] = decorated;
				}
				return decorated;
			}

			public virtual void NotifyClosed(MsgExceptionHandlingTestCase.CloseAwareBin bin)
			{
				lock (_openBins)
				{
					Sharpen.Collections.Remove(_openBins, bin);
				}
			}

			public virtual int NumOpenBins()
			{
				lock (_openBins)
				{
					return _openBins.Count;
				}
			}

			public virtual void SyncAllowed(bool isAllowed)
			{
				lock (this)
				{
					_syncAllowed = isAllowed;
				}
			}

			public virtual bool IllegalSyncInvocation()
			{
				return _illegalSyncInvocation;
			}

			public virtual void NotifySyncInvocation()
			{
				lock (this)
				{
					if (!_syncAllowed)
					{
						_illegalSyncInvocation = true;
					}
				}
			}
		}

		private MsgExceptionHandlingTestCase.CloseAwareStorage _storage;

		private bool _serverClosed;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			_serverClosed = false;
			IObjectServerEvents events = Server();
			events.Closed += new System.EventHandler<ServerClosedEventArgs>(new _IEventListener4_103
				(this).OnEvent);
		}

		private sealed class _IEventListener4_103
		{
			public _IEventListener4_103(MsgExceptionHandlingTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, ServerClosedEventArgs args)
			{
				this._enclosing._serverClosed = true;
			}

			private readonly MsgExceptionHandlingTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			_storage = new MsgExceptionHandlingTestCase.CloseAwareStorage(config.Storage);
			config.Storage = _storage;
		}

		public virtual void TestRecoverableExceptionWithResponse()
		{
			Client().Write(Msg.RequestExceptionWithResponse.GetWriterForSingleObject(Trans(), 
				new Db4oRecoverableException(ExceptionMessage)));
			try
			{
				Client().ExpectedResponse(Msg.Ok);
				Assert.Fail();
			}
			catch (Db4oRecoverableException exc)
			{
				AssertExceptionMessage(exc);
			}
			Assert.IsTrue(Client().IsAlive());
			AssertServerContainerStateClosed(false);
		}

		public virtual void TestNonRecoverableExceptionWithResponse()
		{
			AssertNonRecoverableExceptionForMessage(Msg.RequestExceptionWithResponse, new Db4oException
				(ExceptionMessage));
		}

		public virtual void TestRecoverableExceptionWithoutResponse()
		{
			Client().Write(Msg.RequestExceptionWithoutResponse.GetWriterForSingleObject(Trans
				(), new Db4oRecoverableException(ExceptionMessage)));
			AssertServerContainerStateClosed(false);
		}

		public virtual void TestNonRecoverableExceptionWithoutResponse()
		{
			AssertNonRecoverableExceptionForMessage(Msg.RequestExceptionWithoutResponse, new 
				Db4oException(ExceptionMessage));
		}

		public virtual void TestVmErrorWithResponse()
		{
			AssertNonRecoverableExceptionForMessage(Msg.RequestExceptionWithResponse, new OutOfMemoryException
				());
		}

		public virtual void TestVmErrorWithoutResponse()
		{
			AssertNonRecoverableExceptionForMessage(Msg.RequestExceptionWithoutResponse, new 
				OutOfMemoryException());
		}

		private void AssertNonRecoverableExceptionForMessage(MsgD message, Exception throwable
			)
		{
			// Make sure the ClassMetadata of the exception is in the
			// ObjectContainer otherwise we get side effects from producing it.
			IReflectClass reflectClass = Client().Reflector().ForClass(throwable.GetType());
			Client().ProduceClassMetadata(reflectClass);
			_storage.SyncAllowed(false);
			Client().Write(message.GetWriterForSingleObject(Trans(), throwable));
			AssertDatabaseClosedException();
			AssertServerContainerStateClosed(true);
		}

		private void AssertDatabaseClosedException()
		{
			Assert.Expect(typeof(DatabaseClosedException), new _ICodeBlock_164(this));
			Assert.IsFalse(Client().IsAlive());
		}

		private sealed class _ICodeBlock_164 : ICodeBlock
		{
			public _ICodeBlock_164(MsgExceptionHandlingTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Client().ExpectedResponse(Msg.Ok);
			}

			private readonly MsgExceptionHandlingTestCase _enclosing;
		}

		private void AssertExceptionMessage(Db4oRecoverableException exc)
		{
			Assert.AreEqual(ExceptionMessage, exc.Message);
		}

		private void AssertServerContainerStateClosed(bool expectedClosed)
		{
			if (expectedClosed)
			{
				long timeout = 1000;
				long startTime = Runtime.CurrentTimeMillis();
				while (!_serverClosed && (Runtime.CurrentTimeMillis() - startTime < timeout))
				{
					try
					{
						Thread.Sleep(10);
					}
					catch (Exception e)
					{
						Sharpen.Runtime.PrintStackTrace(e);
					}
				}
			}
			Assert.IsFalse(_storage.IllegalSyncInvocation());
			Assert.AreEqual(expectedClosed, _serverClosed);
			Assert.AreEqual(!expectedClosed, _storage.NumOpenBins() > 0);
			if (!expectedClosed)
			{
				TryToOpenNewClient();
			}
		}

		// TODO: fails on .NET
		//			Assert.expect(Db4oIOException.class, new CodeBlock() {
		//				public void run() throws Throwable {
		//					tryToOpenNewClient();
		//				}
		//			});
		private void TryToOpenNewClient()
		{
			IExtObjectContainer otherClient = OpenNewSession();
			otherClient.Close();
		}
	}
}
#endif // !SILVERLIGHT
