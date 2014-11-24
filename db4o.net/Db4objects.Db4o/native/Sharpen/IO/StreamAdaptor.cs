/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System.IO;

namespace Sharpen.IO 
{
	public abstract class StreamAdaptor 
	{
		protected readonly Stream _stream;

		public StreamAdaptor(Stream stream) 
		{
			_stream = stream;
		}

		public Stream UnderlyingStream 
		{
			get { return _stream; }
		}

		public void Close() 
		{
			_stream.Close();
		}
	}
}
