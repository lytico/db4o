/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using System.Net.Sockets;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.IO;

namespace Db4objects.Db4o.CS.Foundation
{
	public abstract class NetworkSocketBase : ISocket4
	{
		private string _hostName;

		private Sharpen.Net.Socket _socket;

		private IInputStream _in;

		private IOutputStream _out;

		/// <exception cref="System.IO.IOException"></exception>
		public NetworkSocketBase(Sharpen.Net.Socket socket) : this(socket, null)
		{
		}

		/// <exception cref="System.IO.IOException"></exception>
		public NetworkSocketBase(Sharpen.Net.Socket socket, string hostName)
		{
			_socket = socket;
			_hostName = hostName;
			_in = _socket.GetInputStream();
			_out = _socket.GetOutputStream();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			_socket.Close();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Flush()
		{
			_out.Flush();
		}

		public virtual bool IsConnected()
		{
			return Platform4.IsConnected(_socket);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual int Read(byte[] a_bytes, int a_offset, int a_length)
		{
			int ret = _in.Read(a_bytes, a_offset, a_length);
			CheckEOF(ret);
			return ret;
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void CheckEOF(int ret)
		{
			if (ret == -1)
			{
				throw new IOException();
			}
		}

		public virtual void SetSoTimeout(int timeout)
		{
			try
			{
				_socket.SetSoTimeout(timeout);
			}
			catch (SocketException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Write(byte[] bytes, int off, int len)
		{
			_out.Write(bytes, off, len);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 OpenParallelSocket()
		{
			if (_hostName == null)
			{
				throw new InvalidOperationException();
			}
			return CreateParallelSocket(_hostName, _socket.GetPort());
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected abstract ISocket4 CreateParallelSocket(string hostName, int port);

		public override string ToString()
		{
			return _socket.ToString();
		}
	}
}
