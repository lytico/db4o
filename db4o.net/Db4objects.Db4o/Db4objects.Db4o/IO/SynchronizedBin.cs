/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	public class SynchronizedBin : BinDecorator
	{
		public SynchronizedBin(IBin bin) : base(bin)
		{
		}

		public override void Close()
		{
			lock (_bin)
			{
				base.Close();
			}
		}

		public override long Length()
		{
			lock (_bin)
			{
				return base.Length();
			}
		}

		public override int Read(long position, byte[] buffer, int bytesToRead)
		{
			lock (_bin)
			{
				return base.Read(position, buffer, bytesToRead);
			}
		}

		public override void Write(long position, byte[] bytes, int bytesToWrite)
		{
			lock (_bin)
			{
				base.Write(position, bytes, bytesToWrite);
			}
		}

		public override void Sync()
		{
			lock (_bin)
			{
				base.Sync();
			}
		}

		public override void Sync(IRunnable runnable)
		{
			lock (_bin)
			{
				base.Sync(runnable);
			}
		}
	}
}
