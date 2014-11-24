/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Net.Sockets;
using NativeSocket=System.Net.Sockets.Socket;

namespace Sharpen.Net
{
	public class SocketWrapper
	{
		protected NativeSocket _delegate;

#if CF || SILVERLIGHT
	    private int _soTimeout = 0;

        public int SoTimeout
        {
            get { return _soTimeout; }
        }
#endif

		public NativeSocket UnderlyingSocket
		{
			get { return _delegate;  }
		}

	    protected virtual void Initialize(NativeSocket socket)
		{
			_delegate = socket;
		}

		public void SetSoTimeout(int timeout)
		{
#if !CF && !SILVERLIGHT
			_delegate.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);
			_delegate.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeout);
#else
			_soTimeout = timeout;
#endif
		}

		public void Close()
		{
			if (_delegate.Connected)
			{
				try
				{
					_delegate.Shutdown(SocketShutdown.Both);
				}
				catch (Exception)
				{	
				}
			}
			_delegate.Close();
		}

        public bool IsConnected() 
        {
            return _delegate.Connected;
        }
	}
}
