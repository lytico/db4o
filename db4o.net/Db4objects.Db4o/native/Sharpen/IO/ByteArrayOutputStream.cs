/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System;
using System.IO;

namespace Sharpen.IO
{
	public class ByteArrayOutputStream : OutputStream
	{
		public ByteArrayOutputStream() : base(new MemoryStream())
		{
		}

		public ByteArrayOutputStream(int size) : base(new MemoryStream(size))
		{
		}

		public int Size()
		{
			return (int)Stream.Length;
		}

		public void WriteTo(OutputStream stream)
		{
			Stream.WriteTo(stream.UnderlyingStream);
		}

		public byte[] ToByteArray()
		{
			return Stream.ToArray();
		}

		private MemoryStream Stream
		{
			get { return (MemoryStream)UnderlyingStream; }
		}
	}
}
