/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	public class BlockAwareBin : BinDecorator
	{
		private const int CopySize = 4096;

		private bool _readOnly;

		private readonly IBlockSize _blockSize = ((IBlockSize)Environments.My(typeof(IBlockSize
			)));

		public BlockAwareBin(IBin bin) : base(bin)
		{
		}

		/// <summary>converts address and address offset to an absolute address</summary>
		protected long RegularAddress(int blockAddress, int blockAddressOffset)
		{
			if (0 == BlockSize())
			{
				throw new InvalidOperationException();
			}
			return (long)blockAddress * BlockSize() + blockAddressOffset;
		}

		/// <summary>copies a block within a file in block mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockCopy(int oldAddress, int oldAddressOffset, int newAddress
			, int newAddressOffset, int length)
		{
			Copy(RegularAddress(oldAddress, oldAddressOffset), RegularAddress(newAddress, newAddressOffset
				), length);
		}

		/// <summary>copies a block within a file in absolute mode</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Copy(long oldAddress, long newAddress, int length)
		{
			if (DTrace.enabled)
			{
				DTrace.IoCopy.LogLength(newAddress, length);
			}
			if (length > CopySize)
			{
				byte[] buffer = new byte[CopySize];
				int pos = 0;
				while (pos + CopySize < length)
				{
					Copy(buffer, oldAddress + pos, newAddress + pos);
					pos += CopySize;
				}
				oldAddress += pos;
				newAddress += pos;
				length -= pos;
			}
			Copy(new byte[length], oldAddress, newAddress);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void Copy(byte[] buffer, long oldAddress, long newAddress)
		{
			Read(oldAddress, buffer);
			Write(oldAddress, buffer);
		}

		/// <summary>reads a buffer at the seeked address</summary>
		/// <returns>the number of bytes read and returned</returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int BlockRead(int address, int offset, byte[] buffer)
		{
			return BlockRead(address, offset, buffer, buffer.Length);
		}

		/// <summary>implement to read a buffer at the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int BlockRead(int address, int offset, byte[] bytes, int length)
		{
			return Read(RegularAddress(address, offset), bytes, length);
		}

		/// <summary>reads a buffer at the seeked address</summary>
		/// <returns>the number of bytes read and returned</returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int BlockRead(int address, byte[] buffer)
		{
			return BlockRead(address, 0, buffer, buffer.Length);
		}

		/// <summary>implement to read a buffer at the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int BlockRead(int address, byte[] bytes, int length)
		{
			return BlockRead(address, 0, bytes, length);
		}

		/// <summary>reads a buffer at the seeked address</summary>
		/// <returns>the number of bytes read and returned</returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int Read(long pos, byte[] buffer)
		{
			return Read(pos, buffer, buffer.Length);
		}

		/// <summary>reads a buffer at the seeked address</summary>
		/// <returns>the number of bytes read and returned</returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockWrite(int address, int offset, byte[] buffer)
		{
			BlockWrite(address, offset, buffer, buffer.Length);
		}

		/// <summary>implement to read a buffer at the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockWrite(int address, int offset, byte[] bytes, int length)
		{
			Write(RegularAddress(address, offset), bytes, length);
		}

		/// <summary>reads a buffer at the seeked address</summary>
		/// <returns>the number of bytes read and returned</returns>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockWrite(int address, byte[] buffer)
		{
			BlockWrite(address, 0, buffer, buffer.Length);
		}

		/// <summary>implement to read a buffer at the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void BlockWrite(int address, byte[] bytes, int length)
		{
			BlockWrite(address, 0, bytes, length);
		}

		public override void Sync()
		{
			ValidateReadOnly();
			try
			{
				base.Sync();
			}
			catch (Db4oIOException e)
			{
				_readOnly = true;
				throw;
			}
		}

		public override void Sync(IRunnable runnable)
		{
			ValidateReadOnly();
			try
			{
				base.Sync(runnable);
			}
			catch (Db4oIOException e)
			{
				_readOnly = true;
				throw;
			}
		}

		/// <summary>writes a buffer to the seeked address</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(long pos, byte[] bytes)
		{
			ValidateReadOnly();
			try
			{
				Write(pos, bytes, bytes.Length);
			}
			catch (Db4oIOException e)
			{
				_readOnly = true;
				throw;
			}
		}

		private void ValidateReadOnly()
		{
			if (_readOnly)
			{
				throw new EmergencyShutdownReadOnlyException();
			}
		}

		/// <summary>returns the block size currently used</summary>
		public virtual int BlockSize()
		{
			return _blockSize.Value();
		}

		/// <summary>outside call to set the block size of this adapter</summary>
		public virtual void BlockSize(int blockSize)
		{
			if (blockSize < 1)
			{
				throw new ArgumentException();
			}
			_blockSize.Set(blockSize);
		}
	}
}
