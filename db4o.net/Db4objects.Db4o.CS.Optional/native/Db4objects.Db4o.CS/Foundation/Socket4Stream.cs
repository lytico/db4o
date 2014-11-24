/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using System.IO;

namespace Db4objects.Db4o.CS.Foundation
{
	internal class Socket4Stream : Stream
	{
		public Socket4Stream(ISocket4 socket)
		{
			_socket = socket;
		}

		public override void Flush()
		{
			_socket.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _socket.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_socket.Write(buffer, offset, count);
		}

		public override bool CanRead
		{
			get { return true ; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		public override long Position
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		private readonly ISocket4 _socket;
	}
}