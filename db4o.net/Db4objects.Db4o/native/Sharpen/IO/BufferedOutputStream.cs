/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;

namespace Sharpen.IO
{
	public class BufferedOutputStream : IOutputStream
	{
		private IOutputStream _stream;

		public BufferedOutputStream(IOutputStream stream)
		{
			_stream = stream;
		}

		public BufferedOutputStream(IOutputStream stream, int bufferSize)
		{
			_stream = stream;
		}

		public void Write(int i)
		{
			_stream.Write(i);
		}

		public void Write(byte[] bytes)
		{
			_stream.Write(bytes);
		}

		public void Write(byte[] bytes, int offset, int length)
		{
			_stream.Write(bytes, offset, length);
		}

		public void Flush()
		{
			_stream.Flush();
		}

		public void Close()
		{
			_stream.Close();
		}
	}
}
