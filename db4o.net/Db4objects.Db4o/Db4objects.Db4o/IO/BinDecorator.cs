/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <summary>Wrapper baseclass for all classes that wrap Bin.</summary>
	/// <remarks>
	/// Wrapper baseclass for all classes that wrap Bin.
	/// Each class that adds functionality to a Bin can
	/// extend this class to allow db4o to access the
	/// delegate instance with
	/// <see cref="StorageDecorator.Decorate(BinConfiguration, IBin)">StorageDecorator.Decorate(BinConfiguration, IBin)
	/// 	</see>
	/// .
	/// </remarks>
	public class BinDecorator : IBin
	{
		protected readonly IBin _bin;

		/// <summary>Default constructor.</summary>
		/// <remarks>Default constructor.</remarks>
		/// <param name="bin">
		/// the
		/// <see cref="IBin">IBin</see>
		/// that is to be wrapped.
		/// </param>
		public BinDecorator(IBin bin)
		{
			_bin = bin;
		}

		/// <summary>
		/// closes the BinDecorator and the underlying
		/// <see cref="IBin">IBin</see>
		/// .
		/// </summary>
		public virtual void Close()
		{
			_bin.Close();
		}

		/// <seealso cref="IBin.Length()"></seealso>
		public virtual long Length()
		{
			return _bin.Length();
		}

		/// <seealso cref="IBin.Read(long, byte[], int)">IBin.Read(long, byte[], int)</seealso>
		public virtual int Read(long position, byte[] bytes, int bytesToRead)
		{
			return _bin.Read(position, bytes, bytesToRead);
		}

		/// <seealso cref="IBin.Sync()">IBin.Sync()</seealso>
		public virtual void Sync()
		{
			_bin.Sync();
		}

		/// <seealso cref="IBin.SyncRead(long, byte[], int)">IBin.SyncRead(long, byte[], int)
		/// 	</seealso>
		public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
		{
			return _bin.SyncRead(position, bytes, bytesToRead);
		}

		/// <seealso cref="IBin.Write(long, byte[], int)">IBin.Write(long, byte[], int)</seealso>
		public virtual void Write(long position, byte[] bytes, int bytesToWrite)
		{
			_bin.Write(position, bytes, bytesToWrite);
		}

		public virtual void Sync(IRunnable runnable)
		{
			_bin.Sync(runnable);
		}
	}
}
