/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class SystemInfoFileImpl : ISystemInfo
	{
		private LocalObjectContainer _file;

		public SystemInfoFileImpl(LocalObjectContainer file)
		{
			_file = file;
		}

		public virtual int FreespaceEntryCount()
		{
			if (!HasFreespaceManager())
			{
				return 0;
			}
			return FreespaceManager().SlotCount();
		}

		private bool HasFreespaceManager()
		{
			return FreespaceManager() != null;
		}

		private IFreespaceManager FreespaceManager()
		{
			return _file.FreespaceManager();
		}

		public virtual long FreespaceSize()
		{
			if (!HasFreespaceManager())
			{
				return 0;
			}
			long blockSize = _file.BlockSize();
			long blockedSize = FreespaceManager().TotalFreespace();
			return blockSize * blockedSize;
		}

		public virtual long TotalSize()
		{
			return _file.FileLength();
		}
	}
}
