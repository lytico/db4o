/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class IoAdaptedObjectContainer : LocalObjectContainer, IEmbeddedObjectContainer
	{
		private readonly string _fileName;

		private BlockAwareBin _file;

		private volatile BlockAwareBin _backupFile;

		private object _fileLock;

		private readonly IFreespaceFiller _freespaceFiller;

		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		internal IoAdaptedObjectContainer(IConfiguration config, string fileName) : base(
			config)
		{
			_fileLock = new object();
			_fileName = fileName;
			_freespaceFiller = CreateFreespaceFiller();
			Open();
		}

		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		protected sealed override void OpenImpl()
		{
			Config4Impl configImpl = ConfigImpl;
			IStorage storage = configImpl.Storage;
			bool isNew = !storage.Exists(FileName());
			if (isNew)
			{
				LogMsg(14, FileName());
				CheckReadOnly();
				_handlers.OldEncryptionOff();
			}
			bool readOnly = configImpl.IsReadOnly();
			bool lockFile = Debug4.lockFile && configImpl.LockFile() && (!readOnly);
			if (NeedsLockFileThread())
			{
				IBin fileBin = storage.Open(new BinConfiguration(FileName(), false, 0, false, configImpl
					.BlockSize()));
				IBin synchronizedBin = new SynchronizedBin(fileBin);
				_file = new BlockAwareBin(synchronizedBin);
			}
			else
			{
				IBin bin = storage.Open(new BinConfiguration(FileName(), lockFile, 0, readOnly, configImpl
					.BlockSize()));
				if (configImpl.AsynchronousSync())
				{
					bin = new ThreadedSyncBin(bin);
				}
				_file = new BlockAwareBin(bin);
			}
			if (isNew)
			{
				ConfigureNewFile();
				if (configImpl.ReservedStorageSpace() > 0)
				{
					Reserve(configImpl.ReservedStorageSpace());
				}
				CommitTransaction();
				WriteHeader(true, false);
			}
			else
			{
				ReadThis();
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Backup(IStorage targetStorage, string path)
		{
			WithEnvironment(new _IRunnable_76(this, targetStorage, path));
		}

		private sealed class _IRunnable_76 : IRunnable
		{
			public _IRunnable_76(IoAdaptedObjectContainer _enclosing, IStorage targetStorage, 
				string path)
			{
				this._enclosing = _enclosing;
				this.targetStorage = targetStorage;
				this.path = path;
			}

			public void Run()
			{
				lock (this._enclosing._lock)
				{
					this._enclosing.CheckClosed();
					if (this._enclosing._backupFile != null)
					{
						throw new BackupInProgressException();
					}
					this._enclosing._backupFile = new BlockAwareBin(targetStorage.Open(new BinConfiguration
						(path, true, this._enclosing._file.Length(), false, this._enclosing._blockConverter
						.BlocksToBytes(1))));
				}
				long pos = 0;
				byte[] buffer = new byte[8192];
				while (true)
				{
					lock (this._enclosing._lock)
					{
						int read = this._enclosing._file.Read(pos, buffer);
						if (read <= 0)
						{
							break;
						}
						this._enclosing._backupFile.Write(pos, buffer, read);
						pos += read;
					}
					// Let the database engine continue to do 
					// some work if it likes to.
					Runtime4.Sleep(1);
				}
				lock (this._enclosing._lock)
				{
					try
					{
						Db4objects.Db4o.Internal.IoAdaptedObjectContainer.SyncAndClose(this._enclosing._backupFile
							);
					}
					finally
					{
						this._enclosing._backupFile = null;
					}
				}
			}

			private readonly IoAdaptedObjectContainer _enclosing;

			private readonly IStorage targetStorage;

			private readonly string path;
		}

		public override void BlockSize(int size)
		{
			CreateBlockConverter(size);
			_file.BlockSize(size);
		}

		public override byte BlockSize()
		{
			return (byte)_file.BlockSize();
		}

		protected override void ShutdownDataStorage()
		{
			lock (_fileLock)
			{
				try
				{
					CloseFileHeader();
				}
				finally
				{
					CloseDatabaseFile();
				}
			}
		}

		private void CloseDatabaseFile()
		{
			try
			{
				SyncAndClose(_file);
			}
			finally
			{
				_file = null;
			}
		}

		private static void SyncAndClose(IBin bin)
		{
			if (bin != null)
			{
				try
				{
					bin.Sync();
				}
				finally
				{
					bin.Close();
				}
			}
		}

		private void CloseFileHeader()
		{
			try
			{
				if (_fileHeader != null)
				{
					_fileHeader.Close();
				}
			}
			finally
			{
				_fileHeader = null;
			}
		}

		public override void CloseTransaction(Transaction transaction, bool isSystemTransaction
			, bool rollbackOnClose)
		{
			transaction.Close(rollbackOnClose);
		}

		public override void Commit1(Transaction trans)
		{
			EnsureLastSlotWritten();
			base.Commit1(trans);
		}

		private void CheckXBytes(int newAddress, int newAddressOffset, int length)
		{
			if (Debug4.xbytes && Deploy.overwrite)
			{
				try
				{
					byte[] checkXBytes = new byte[length];
					_file.BlockRead(newAddress, newAddressOffset, checkXBytes);
					for (int i = 0; i < checkXBytes.Length; i++)
					{
						if (checkXBytes[i] != Const4.Xbyte)
						{
							string msg = "XByte corruption adress:" + newAddress + " length:" + length + " starting:"
								 + i;
							throw new Db4oException(msg);
						}
					}
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}
		}

		public override long FileLength()
		{
			return _file.Length();
		}

		public override string FileName()
		{
			return _fileName;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void ReadBytes(byte[] bytes, int address, int length)
		{
			ReadBytes(bytes, address, 0, length);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void ReadBytes(byte[] bytes, int address, int addressOffset, int 
			length)
		{
			if (DTrace.enabled)
			{
				DTrace.ReadBytes.LogLength(address + addressOffset, length);
			}
			int bytesRead = _file.BlockRead(address, addressOffset, bytes, length);
			CheckReadCount(bytesRead, length);
		}

		private void CheckReadCount(int bytesRead, int expected)
		{
			if (bytesRead != expected)
			{
				throw new IncompatibleFileFormatException();
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public override void Reserve(int byteCount)
		{
			CheckReadOnly();
			lock (_lock)
			{
				Slot slot = AllocateSlot(byteCount);
				ZeroReservedSlot(slot);
				Free(slot);
			}
		}

		private void ZeroReservedSlot(Slot slot)
		{
			ZeroFile(_file, slot);
			ZeroFile(_backupFile, slot);
		}

		private void ZeroFile(BlockAwareBin io, Slot slot)
		{
			if (io == null)
			{
				return;
			}
			byte[] zeroBytes = new byte[1024];
			int left = slot.Length();
			int offset = 0;
			while (left > zeroBytes.Length)
			{
				io.BlockWrite(slot.Address(), offset, zeroBytes, zeroBytes.Length);
				offset += zeroBytes.Length;
				left -= zeroBytes.Length;
			}
			if (left > 0)
			{
				io.BlockWrite(slot.Address(), offset, zeroBytes, left);
			}
		}

		public override void SyncFiles()
		{
			_file.Sync();
		}

		public override void SyncFiles(IRunnable runnable)
		{
			_file.Sync(runnable);
		}

		public override void WriteBytes(ByteArrayBuffer buffer, int blockedAddress, int addressOffset
			)
		{
			if (Deploy.debug && !Deploy.flush)
			{
				return;
			}
			if (Debug4.xbytes && Deploy.overwrite)
			{
				if (buffer.CheckXBytes())
				{
					CheckXBytes(blockedAddress, addressOffset, buffer.Length());
				}
				else
				{
					buffer.CheckXBytes(true);
				}
			}
			if (DTrace.enabled)
			{
				DTrace.WriteBytes.LogLength(blockedAddress + addressOffset, buffer.Length());
			}
			_file.BlockWrite(blockedAddress, addressOffset, buffer._buffer, buffer.Length());
			if (_backupFile != null)
			{
				_backupFile.BlockWrite(blockedAddress, addressOffset, buffer._buffer, buffer.Length
					());
			}
		}

		public override void OverwriteDeletedBytes(int address, int length)
		{
			if (_freespaceFiller == null)
			{
				return;
			}
			if (address > 0 && length > 0)
			{
				if (DTrace.enabled)
				{
					DTrace.WriteXbytes.LogLength(address, length);
				}
				BlockAwareBinWindow window = new BlockAwareBinWindow(_file, address, length);
				try
				{
					CreateFreespaceFiller().Fill(window);
				}
				catch (IOException e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
				finally
				{
					window.Disable();
				}
			}
		}

		public virtual BlockAwareBin TimerFile()
		{
			return _file;
		}

		private IFreespaceFiller CreateFreespaceFiller()
		{
			return Config().FreespaceFiller();
		}

		private class XByteFreespaceFiller : IFreespaceFiller
		{
			/// <exception cref="System.IO.IOException"></exception>
			public virtual void Fill(BlockAwareBinWindow io)
			{
				io.Write(0, XBytes(io.Length()));
			}

			private byte[] XBytes(int len)
			{
				byte[] bytes = new byte[len];
				for (int i = 0; i < len; i++)
				{
					bytes[i] = Const4.Xbyte;
				}
				return bytes;
			}
		}

		protected override void FatalStorageShutdown()
		{
			if (_file != null)
			{
				_file.Close();
			}
		}
	}
}
