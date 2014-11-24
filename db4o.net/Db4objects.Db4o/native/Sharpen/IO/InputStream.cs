/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System.IO;

namespace Sharpen.IO
{
	public class InputStream : StreamAdaptor, IInputStream
	{
		public InputStream(Stream stream)
			: base(stream)
		{
		}

		public int Read()
		{
			return _stream.ReadByte();
		}

		public int Read(byte[] bytes)
		{
			return Read(bytes, 0, bytes.Length);
		}

		public int Read(byte[] bytes, int offset, int length)
		{
			return TranslateReadReturnValue(_stream.Read(bytes, offset, length));
		}

		internal static int TranslateReadReturnValue(int read)
		{
			return (0 == read) ? -1 : read;
		}
	}
}
