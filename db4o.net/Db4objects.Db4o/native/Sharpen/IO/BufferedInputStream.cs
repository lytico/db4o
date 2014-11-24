/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;

namespace Sharpen.IO
{
	public class BufferedInputStream : IInputStream
	{
		private IInputStream _stream;

		public BufferedInputStream(IInputStream stream)
		{
			_stream = stream;
		}

		public BufferedInputStream(IInputStream stream, int bufferSize)
		{
			_stream = stream;
		}

		public int Read()
		{
			return _stream.Read();
		}

		public int Read(byte[] bytes)
		{
			return _stream.Read(bytes);
		}

		public int Read(byte[] bytes, int offset, int length)
		{
			return _stream.Read(bytes, offset, length);
		}

		public void Close()
		{
			_stream.Close();
		}
	}
}
