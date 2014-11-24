/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation.Network;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Foundation.Network
{
	/// <exclude></exclude>
	public class NetworkSocketTestCase : ITestLifeCycle
	{
		private IServerSocket4 _serverSocket;

		private int _port;

		internal Socket4Adapter _client;

		internal Socket4Adapter _server;

		private ISocket4Factory _plainSocketFactory = new StandardSocket4Factory();

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(NetworkSocketTestCase)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_serverSocket = _plainSocketFactory.CreateServerSocket(0);
			_port = _serverSocket.GetLocalPort();
			_client = new Socket4Adapter(_plainSocketFactory.CreateSocket("localhost", _port)
				);
			_server = new Socket4Adapter(_serverSocket.Accept());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			_serverSocket.Close();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestReadByteArrayCloseClient()
		{
			AssertReadClose(_client, new _ICodeBlock_44(this));
		}

		private sealed class _ICodeBlock_44 : ICodeBlock
		{
			public _ICodeBlock_44(NetworkSocketTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._server.Read(new byte[10], 0, 10);
			}

			private readonly NetworkSocketTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestReadByteArrayCloseServer()
		{
			AssertReadClose(_server, new _ICodeBlock_52(this));
		}

		private sealed class _ICodeBlock_52 : ICodeBlock
		{
			public _ICodeBlock_52(NetworkSocketTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._client.Read(new byte[10], 0, 10);
			}

			private readonly NetworkSocketTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestWriteByteArrayCloseClient()
		{
			AssertWriteClose(_client, new _ICodeBlock_61(this));
		}

		private sealed class _ICodeBlock_61 : ICodeBlock
		{
			public _ICodeBlock_61(NetworkSocketTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._server.Write(new byte[10]);
			}

			private readonly NetworkSocketTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestWriteByteArrayCloseServer()
		{
			AssertWriteClose(_server, new _ICodeBlock_69(this));
		}

		private sealed class _ICodeBlock_69 : ICodeBlock
		{
			public _ICodeBlock_69(NetworkSocketTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._client.Write(new byte[10]);
			}

			private readonly NetworkSocketTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestWriteByteArrayPartCloseClient()
		{
			AssertWriteClose(_client, new _ICodeBlock_77(this));
		}

		private sealed class _ICodeBlock_77 : ICodeBlock
		{
			public _ICodeBlock_77(NetworkSocketTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._server.Write(new byte[10], 0, 10);
			}

			private readonly NetworkSocketTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestWriteByteArrayPartCloseServer()
		{
			AssertWriteClose(_server, new _ICodeBlock_85(this));
		}

		private sealed class _ICodeBlock_85 : ICodeBlock
		{
			public _ICodeBlock_85(NetworkSocketTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing._client.Write(new byte[10], 0, 10);
			}

			private readonly NetworkSocketTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertReadClose(Socket4Adapter socketToBeClosed, ICodeBlock codeBlock
			)
		{
			NetworkSocketTestCase.CatchAllThread thread = new NetworkSocketTestCase.CatchAllThread
				(codeBlock);
			thread.EnsureStarted();
			socketToBeClosed.Close();
			thread.Join();
			Assert.IsInstanceOf(typeof(Db4oIOException), thread.Caught());
		}

		private void AssertWriteClose(Socket4Adapter socketToBeClosed, ICodeBlock codeBlock
			)
		{
			socketToBeClosed.Close();
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_102(codeBlock));
		}

		private sealed class _ICodeBlock_102 : ICodeBlock
		{
			public _ICodeBlock_102(ICodeBlock codeBlock)
			{
				this.codeBlock = codeBlock;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				// This is a magic number: 
				// On my machine all tests start to pass when I write at least 7 times.
				// Trying with 20 on the build machine.
				for (int i = 0; i < 20; i++)
				{
					codeBlock.Run();
				}
			}

			private readonly ICodeBlock codeBlock;
		}

		internal class CatchAllThread
		{
			private readonly Thread _thread;

			internal bool _isRunning;

			internal readonly ICodeBlock _codeBlock;

			internal Exception _throwable;

			public CatchAllThread(ICodeBlock codeBlock)
			{
				_thread = new Thread(new _IRunnable_126(this), "NetworkSocketTestCase.CatchAllThread"
					);
				_thread.SetDaemon(true);
				_codeBlock = codeBlock;
			}

			private sealed class _IRunnable_126 : IRunnable
			{
				public _IRunnable_126(CatchAllThread _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Run()
				{
					try
					{
						lock (this)
						{
							this._enclosing._isRunning = true;
						}
						this._enclosing._codeBlock.Run();
					}
					catch (Exception t)
					{
						this._enclosing._throwable = t;
					}
				}

				private readonly CatchAllThread _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void Join()
			{
				_thread.Join();
			}

			private bool IsRunning()
			{
				lock (this)
				{
					return _isRunning;
				}
			}

			public virtual void EnsureStarted()
			{
				_thread.Start();
				while (!IsRunning())
				{
					Runtime4.Sleep(10);
				}
				Runtime4.Sleep(10);
			}

			public virtual Exception Caught()
			{
				return _throwable;
			}
		}
	}
}
#endif // !SILVERLIGHT
