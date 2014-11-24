/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class TimerFileLockDisabled : TimerFileLock
	{
		public override void CheckHeaderLock()
		{
		}

		public override void CheckOpenTime()
		{
		}

		public override void Close()
		{
		}

		public override bool LockFile()
		{
			return false;
		}

		public override long OpenTime()
		{
			return 0;
		}

		public override void Run()
		{
		}

		public override void SetAddresses(int baseAddress, int openTimeOffset, int accessTimeOffset
			)
		{
		}

		public override void Start()
		{
		}

		public override void WriteHeaderLock()
		{
		}

		public override void WriteOpenTime()
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void CheckIfOtherSessionAlive(LocalObjectContainer container, int
			 address, int offset, long lastAccessTime)
		{
		}
	}
}
