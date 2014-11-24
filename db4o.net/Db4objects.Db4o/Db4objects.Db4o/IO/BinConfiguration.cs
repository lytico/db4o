/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	public class BinConfiguration
	{
		private readonly string _uri;

		private readonly bool _lockFile;

		private readonly long _initialLength;

		private readonly bool _readOnly;

		private readonly int _blockSize;

		public BinConfiguration(string uri, bool lockFile, long initialLength, bool readOnly
			) : this(uri, lockFile, initialLength, readOnly, 1)
		{
		}

		public BinConfiguration(string uri, bool lockFile, long initialLength, bool readOnly
			, int blockSize)
		{
			_uri = uri;
			_lockFile = lockFile;
			_initialLength = initialLength;
			_readOnly = readOnly;
			_blockSize = blockSize;
		}

		public virtual string Uri()
		{
			return _uri;
		}

		public virtual bool LockFile()
		{
			return _lockFile;
		}

		public virtual long InitialLength()
		{
			return _initialLength;
		}

		public virtual bool ReadOnly()
		{
			return _readOnly;
		}

		public virtual int BlockSize()
		{
			return _blockSize;
		}

		public override string ToString()
		{
			return "BinConfiguration(Uri: " + _uri + ", Locked: " + _lockFile + ", ReadOnly: "
				 + _readOnly + ", BlockSize: " + _blockSize + ")";
		}
	}
}
