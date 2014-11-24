/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System;
using System.IO;

namespace Sharpen.IO
{
	public class ByteArrayInputStream : InputStream
	{
		public ByteArrayInputStream(byte[] initial) : base(new MemoryStream(initial))
		{
		}
	}
}
