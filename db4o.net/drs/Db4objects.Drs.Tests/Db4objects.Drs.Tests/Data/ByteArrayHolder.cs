/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class ByteArrayHolder : IIByteArrayHolder
	{
		private byte[] _bytes;

		public ByteArrayHolder()
		{
		}

		public ByteArrayHolder(byte[] bytes)
		{
			this._bytes = bytes;
		}

		public virtual byte[] GetBytes()
		{
			return _bytes;
		}

		public virtual void SetBytes(byte[] bytes)
		{
			_bytes = bytes;
		}
	}
}
