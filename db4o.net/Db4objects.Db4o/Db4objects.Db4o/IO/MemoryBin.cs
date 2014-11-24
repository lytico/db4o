/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.IO;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	public class MemoryBin : IBin
	{
		private byte[] _bytes;

		private int _length;

		private IGrowthStrategy _growthStrategy;

		public MemoryBin(int initialSize, IGrowthStrategy growthStrategy) : this(new byte
			[initialSize], growthStrategy)
		{
		}

		public MemoryBin(byte[] bytes, IGrowthStrategy growthStrategy)
		{
			_bytes = bytes;
			_length = bytes.Length;
			_growthStrategy = growthStrategy;
		}

		public virtual long Length()
		{
			return _length;
		}

		public virtual long BufferSize()
		{
			return _bytes.Length;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int Read(long pos, byte[] bytes, int length)
		{
			long avail = _length - pos;
			if (avail <= 0)
			{
				return -1;
			}
			int read = Math.Min((int)avail, length);
			System.Array.Copy(_bytes, (int)pos, bytes, 0, read);
			return read;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Sync()
		{
		}

		public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
		{
			return Read(position, bytes, bytesToRead);
		}

		public virtual void Close()
		{
		}

		/// <summary>Returns a copy of the raw data contained in this bin for external processing.
		/// 	</summary>
		/// <remarks>
		/// Returns a copy of the raw data contained in this bin for external processing.
		/// Access to the data is not guarded by synchronisation. If this method is called
		/// while the MemoryBin is in use, it is possible that the returned byte array is
		/// not consistent.
		/// </remarks>
		public virtual byte[] Data()
		{
			byte[] data = new byte[_length];
			System.Array.Copy(_bytes, 0, data, 0, _length);
			return data;
		}

		/// <summary>for internal processing only.</summary>
		/// <remarks>for internal processing only.</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(long pos, byte[] buffer, int length)
		{
			if (pos + length > _bytes.Length)
			{
				long newSize = _growthStrategy.NewSize(_bytes.Length, pos + length);
				//			if (pos + length > newSize) {
				//				newSize = pos + length;
				//			}
				byte[] temp = new byte[(int)newSize];
				System.Array.Copy(_bytes, 0, temp, 0, _length);
				_bytes = temp;
			}
			System.Array.Copy(buffer, 0, _bytes, (int)pos, length);
			pos += length;
			if (pos > _length)
			{
				_length = (int)pos;
			}
		}

		public virtual void Sync(IRunnable runnable)
		{
			runnable.Run();
		}
	}
}
