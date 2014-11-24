/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public abstract class TimerFileLock : IRunnable
	{
		public static TimerFileLock ForFile(LocalObjectContainer file)
		{
			return new TimerFileLockDisabled();
		}

		public abstract void CheckHeaderLock();

		public abstract void CheckOpenTime();

		public abstract bool LockFile();

		public abstract long OpenTime();

		public abstract void SetAddresses(int baseAddress, int openTimeOffset, int accessTimeOffset
			);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Start();

		public abstract void WriteHeaderLock();

		public abstract void WriteOpenTime();

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Close();

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void CheckIfOtherSessionAlive(LocalObjectContainer container, int
			 address, int offset, long lastAccessTime);

		public abstract void Run();
	}
}
