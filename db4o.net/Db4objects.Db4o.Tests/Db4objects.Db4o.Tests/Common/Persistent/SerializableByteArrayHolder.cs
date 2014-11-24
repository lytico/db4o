/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Persistent;

namespace Db4objects.Db4o.Tests.Common.Persistent
{
	[System.Serializable]
	public class SerializableByteArrayHolder : IIByteArrayHolder
	{
		private const long serialVersionUID = 1L;

		public byte[] _bytes;

		public SerializableByteArrayHolder(byte[] bytes)
		{
			this._bytes = bytes;
		}

		public virtual byte[] GetBytes()
		{
			return _bytes;
		}
	}
}
