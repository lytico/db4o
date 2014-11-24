/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal.Caching;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	internal class CachingBin : BinDecorator
	{
		private readonly int _pageSize;

		private readonly ICache4 _cache;

		private readonly IObjectPool _pagePool;

		private long _fileLength;

		private sealed class _IProcedure4_22 : IProcedure4
		{
			public _IProcedure4_22(CachingBin _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object discardedPage)
			{
				this._enclosing.FlushPage(((CachingBin.Page)discardedPage));
				this._enclosing._pagePool.ReturnObject(((CachingBin.Page)discardedPage));
			}

			private readonly CachingBin _enclosing;
		}

		private IProcedure4 _onDiscardPage;

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public CachingBin(IBin bin, ICache4 cache, int pageCount, int pageSize) : base(bin
			)
		{
			_onDiscardPage = new _IProcedure4_22(this);
			_producerFromDisk = new _IFunction4_138(this);
			_producerFromPool = new _IFunction4_147(this);
			_pageSize = pageSize;
			_pagePool = new SimpleObjectPool(NewPagePool(pageCount));
			_cache = cache;
			_fileLength = _bin.Length();
		}

		private CachingBin.Page[] NewPagePool(int pageCount)
		{
			CachingBin.Page[] pages = new CachingBin.Page[pageCount];
			for (int i = 0; i < pages.Length; ++i)
			{
				pages[i] = new CachingBin.Page(_pageSize);
			}
			return pages;
		}

		/// <summary>Reads the file into the buffer using pages from cache.</summary>
		/// <remarks>
		/// Reads the file into the buffer using pages from cache. If the next page
		/// is not cached it will be read from the file.
		/// </remarks>
		/// <param name="pos">
		/// 
		/// start position to read
		/// </param>
		/// <param name="buffer">destination buffer</param>
		/// <param name="length">how many bytes to read</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override int Read(long pos, byte[] buffer, int length)
		{
			return ReadInternal(pos, buffer, length, false);
		}

		private int ReadInternal(long pos, byte[] buffer, int length, bool syncRead)
		{
			long startAddress = pos;
			int bytesToRead = length;
			int totalRead = 0;
			while (bytesToRead > 0)
			{
				CachingBin.Page page = syncRead ? SyncReadPage(startAddress) : GetPage(startAddress
					, _producerFromDisk);
				int readBytes = page.Read(buffer, totalRead, startAddress, bytesToRead);
				if (readBytes <= 0)
				{
					break;
				}
				bytesToRead -= readBytes;
				startAddress += readBytes;
				totalRead += readBytes;
			}
			return totalRead == 0 ? -1 : totalRead;
		}

		/// <summary>Writes the buffer to cache using pages</summary>
		/// <param name="pos">start position to write</param>
		/// <param name="buffer">source buffer</param>
		/// <param name="length">how many bytes to write</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Write(long pos, byte[] buffer, int length)
		{
			long startAddress = pos;
			int bytesToWrite = length;
			int bufferOffset = 0;
			while (bytesToWrite > 0)
			{
				// page doesn't need to loadFromDisk if the whole page is dirty
				bool loadFromDisk = (bytesToWrite < _pageSize) || (startAddress % _pageSize != 0);
				CachingBin.Page page = GetPage(startAddress, loadFromDisk);
				int writtenBytes = page.Write(buffer, bufferOffset, startAddress, bytesToWrite);
				bytesToWrite -= writtenBytes;
				startAddress += writtenBytes;
				bufferOffset += writtenBytes;
			}
			long endAddress = startAddress;
			_fileLength = Math.Max(endAddress, _fileLength);
		}

		/// <summary>Flushes cache to a physical storage</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Sync()
		{
			FlushAllPages();
			base.Sync();
		}

		public override void Sync(IRunnable runnable)
		{
			FlushAllPages();
			base.Sync(new _IRunnable_119(this, runnable));
		}

		private sealed class _IRunnable_119 : IRunnable
		{
			public _IRunnable_119(CachingBin _enclosing, IRunnable runnable)
			{
				this._enclosing = _enclosing;
				this.runnable = runnable;
			}

			public void Run()
			{
				runnable.Run();
				this._enclosing.FlushAllPages();
			}

			private readonly CachingBin _enclosing;

			private readonly IRunnable runnable;
		}

		public override int SyncRead(long position, byte[] bytes, int bytesToRead)
		{
			return ReadInternal(position, bytes, bytesToRead, true);
		}

		/// <summary>Returns the file length</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override long Length()
		{
			return _fileLength;
		}

		private sealed class _IFunction4_138 : IFunction4
		{
			public _IFunction4_138(CachingBin _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object pageAddress)
			{
				// in case that page is not found in the cache
				CachingBin.Page newPage = ((CachingBin.Page)this._enclosing._pagePool.BorrowObject
					());
				this._enclosing.LoadPage(newPage, ((long)pageAddress));
				return newPage;
			}

			private readonly CachingBin _enclosing;
		}

		internal readonly IFunction4 _producerFromDisk;

		private sealed class _IFunction4_147 : IFunction4
		{
			public _IFunction4_147(CachingBin _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object pageAddress)
			{
				// in case that page is not found in the cache
				CachingBin.Page newPage = ((CachingBin.Page)this._enclosing._pagePool.BorrowObject
					());
				this._enclosing.ResetPageAddress(newPage, ((long)pageAddress));
				return newPage;
			}

			private readonly CachingBin _enclosing;
		}

		internal readonly IFunction4 _producerFromPool;

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private CachingBin.Page GetPage(long startAddress, bool loadFromDisk)
		{
			IFunction4 producer = loadFromDisk ? _producerFromDisk : _producerFromPool;
			return GetPage(startAddress, producer);
		}

		private CachingBin.Page GetPage(long startAddress, IFunction4 producer)
		{
			CachingBin.Page page = ((CachingBin.Page)_cache.Produce(PageAddressFor(startAddress
				), producer, _onDiscardPage));
			page.EnsureEndAddress(_fileLength);
			return page;
		}

		private CachingBin.Page SyncReadPage(long startAddress)
		{
			CachingBin.Page page = new CachingBin.Page(_pageSize);
			LoadPage(page, startAddress);
			page.EnsureEndAddress(_fileLength);
			return page;
		}

		private long PageAddressFor(long startAddress)
		{
			return (startAddress / _pageSize) * _pageSize;
		}

		private void ResetPageAddress(CachingBin.Page page, long startAddress)
		{
			page._startAddress = startAddress;
			page._endAddress = startAddress + _pageSize;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		protected virtual void FlushAllPages()
		{
			for (IEnumerator pIter = _cache.GetEnumerator(); pIter.MoveNext(); )
			{
				CachingBin.Page p = ((CachingBin.Page)pIter.Current);
				FlushPage(p);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void FlushPage(CachingBin.Page page)
		{
			if (!page._dirty)
			{
				return;
			}
			WritePageToDisk(page);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void LoadPage(CachingBin.Page page, long pos)
		{
			long startAddress = pos - pos % _pageSize;
			page._startAddress = startAddress;
			int count = _bin.Read(page._startAddress, page._buffer, page._bufferSize);
			if (count > 0)
			{
				page._endAddress = startAddress + count;
			}
			else
			{
				page._endAddress = startAddress;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void WritePageToDisk(CachingBin.Page page)
		{
			base.Write(page._startAddress, page._buffer, page.Size());
			page._dirty = false;
		}

		private class Page
		{
			public readonly byte[] _buffer;

			public long _startAddress = -1;

			public long _endAddress;

			public readonly int _bufferSize;

			public bool _dirty;

			private byte[] zeroBytes;

			public Page(int size)
			{
				_bufferSize = size;
				_buffer = new byte[_bufferSize];
			}

			internal virtual void EnsureEndAddress(long fileLength)
			{
				long bufferEndAddress = _startAddress + _bufferSize;
				if (_endAddress < bufferEndAddress && fileLength > _endAddress)
				{
					long newEndAddress = Math.Min(fileLength, bufferEndAddress);
					if (zeroBytes == null)
					{
						zeroBytes = new byte[_bufferSize];
					}
					System.Array.Copy(zeroBytes, 0, _buffer, (int)(_endAddress - _startAddress), (int
						)(newEndAddress - _endAddress));
					_endAddress = newEndAddress;
				}
			}

			internal virtual int Size()
			{
				return (int)(_endAddress - _startAddress);
			}

			internal virtual int Read(byte[] @out, int outOffset, long startAddress, int length
				)
			{
				int bufferOffset = (int)(startAddress - _startAddress);
				int pageAvailbeDataSize = (int)(_endAddress - startAddress);
				int readBytes = Math.Min(pageAvailbeDataSize, length);
				if (readBytes <= 0)
				{
					// meaning reach EOF
					return -1;
				}
				System.Array.Copy(_buffer, bufferOffset, @out, outOffset, readBytes);
				return readBytes;
			}

			internal virtual int Write(byte[] data, int dataOffset, long startAddress, int length
				)
			{
				int bufferOffset = (int)(startAddress - _startAddress);
				int pageAvailabeBufferSize = _bufferSize - bufferOffset;
				int writtenBytes = Math.Min(pageAvailabeBufferSize, length);
				System.Array.Copy(data, dataOffset, _buffer, bufferOffset, writtenBytes);
				long endAddress = startAddress + writtenBytes;
				if (endAddress > _endAddress)
				{
					_endAddress = endAddress;
				}
				_dirty = true;
				return writtenBytes;
			}
		}
	}
}
