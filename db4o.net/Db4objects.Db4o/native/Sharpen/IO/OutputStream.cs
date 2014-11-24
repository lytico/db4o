/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System.IO;

namespace Sharpen.IO
{
	public class OutputStream : StreamAdaptor, IOutputStream
	{
		public OutputStream(Stream stream)
			: base(stream)
		{
		}

		public void Write(int i)
		{
			_stream.WriteByte((byte)i);
		}

		public void Write(byte[] bytes)
		{
			_stream.Write(bytes, 0, bytes.Length);
		}

		public void Write(byte[] bytes, int offset, int length)
		{
			_stream.Write(bytes, offset, length);
		}

		public void Flush()
		{
			_stream.Flush();
		}
	}
}
