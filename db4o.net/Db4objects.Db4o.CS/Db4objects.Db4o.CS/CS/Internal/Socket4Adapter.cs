/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.CS.Internal
{
	public class Socket4Adapter
	{
		private readonly ISocket4 _delegate;

		public Socket4Adapter(ISocket4 delegate_)
		{
			_delegate = delegate_;
		}

		public Socket4Adapter(ISocket4Factory socketFactory, string hostName, int port)
		{
			try
			{
				_delegate = socketFactory.CreateSocket(hostName, port);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Close()
		{
			try
			{
				_delegate.Close();
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Flush()
		{
			try
			{
				_delegate.Flush();
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		public virtual bool IsConnected()
		{
			return _delegate.IsConnected();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual Db4objects.Db4o.CS.Internal.Socket4Adapter OpenParalellSocket()
		{
			try
			{
				return new Db4objects.Db4o.CS.Internal.Socket4Adapter(_delegate.OpenParallelSocket
					());
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int Read(byte[] buffer, int bufferOffset, int byteCount)
		{
			try
			{
				return _delegate.Read(buffer, bufferOffset, byteCount);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		public virtual void SetSoTimeout(int timeout)
		{
			_delegate.SetSoTimeout(timeout);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(byte[] bytes, int offset, int count)
		{
			try
			{
				_delegate.Write(bytes, offset, count);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(byte[] bytes)
		{
			try
			{
				_delegate.Write(bytes, 0, bytes.Length);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		public override string ToString()
		{
			return _delegate.ToString();
		}
	}
}
