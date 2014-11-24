/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>Bounded handle into an IoAdapter: Can only access a restricted area.</summary>
	/// <remarks>Bounded handle into an IoAdapter: Can only access a restricted area.</remarks>
	/// <exclude></exclude>
	public class BlockAwareBinWindow
	{
		private BlockAwareBin _bin;

		private int _blockOff;

		private int _len;

		private bool _disabled;

		/// <param name="io">The delegate I/O adapter</param>
		/// <param name="blockOff">The block offset address into the I/O adapter that maps to the start index (0) of this window
		/// 	</param>
		/// <param name="len">The size of this window in bytes</param>
		public BlockAwareBinWindow(BlockAwareBin io, int blockOff, int len)
		{
			_bin = io;
			_blockOff = blockOff;
			_len = len;
			_disabled = false;
		}

		/// <returns>Size of this I/O adapter window in bytes.</returns>
		public virtual int Length()
		{
			return _len;
		}

		/// <param name="off">Offset in bytes relative to the window start</param>
		/// <param name="data">Data to write into the window starting from the given offset</param>
		/// <exception cref="System.ArgumentException"></exception>
		/// <exception cref="System.InvalidOperationException"></exception>
		public virtual void Write(int off, byte[] data)
		{
			CheckBounds(off, data);
			_bin.BlockWrite(_blockOff + off, data);
		}

		/// <param name="off">Offset in bytes relative to the window start</param>
		/// <param name="data">Data buffer to read from the window starting from the given offset
		/// 	</param>
		/// <exception cref="System.ArgumentException"></exception>
		/// <exception cref="System.InvalidOperationException"></exception>
		public virtual int Read(int off, byte[] data)
		{
			CheckBounds(off, data);
			return _bin.BlockRead(_blockOff + off, data);
		}

		/// <summary>Disable IO Adapter Window</summary>
		public virtual void Disable()
		{
			_disabled = true;
		}

		/// <summary>Flush IO Adapter Window</summary>
		public virtual void Flush()
		{
			if (!_disabled)
			{
				_bin.Sync();
			}
		}

		private void CheckBounds(int off, byte[] data)
		{
			if (_disabled)
			{
				throw new InvalidOperationException();
			}
			if (data == null || off < 0 || off + data.Length > _len)
			{
				throw new ArgumentException();
			}
		}
	}
}
