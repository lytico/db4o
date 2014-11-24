/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Internal.Transactionlog;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Transactionlog
{
	/// <exclude></exclude>
	public class FileBasedTransactionLogHandler : TransactionLogHandler
	{
		internal const int LockInt = int.MaxValue - 1;

		private IBin _lockFile;

		private IBin _logFile;

		private readonly string _fileName;

		public FileBasedTransactionLogHandler(LocalObjectContainer container, string fileName
			) : base(container)
		{
			_fileName = fileName;
		}

		public static string LogFileName(string fileName)
		{
			return fileName + ".log";
		}

		public static string LockFileName(string fileName)
		{
			return fileName + ".lock";
		}

		private IBin OpenBin(string fileName)
		{
			return new FileStorage().Open(new BinConfiguration(fileName, _container.Config().
				LockFile(), 0, false));
		}

		public override void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
			if (!System.IO.File.Exists(LockFileName(_fileName)))
			{
				return;
			}
			if (!LockFileSignalsInterruptedTransaction())
			{
				return;
			}
			ByteArrayBuffer buffer = new ByteArrayBuffer(Const4.IntLength);
			OpenLogFile();
			Read(_logFile, buffer);
			int length = buffer.ReadInt();
			if (length > 0)
			{
				buffer = new ByteArrayBuffer(length);
				Read(_logFile, buffer);
				buffer.IncrementOffset(Const4.IntLength);
				ReadWriteSlotChanges(buffer);
			}
			DeleteLockFile();
			CloseLogFile();
			DeleteLogFile();
		}

		private bool LockFileSignalsInterruptedTransaction()
		{
			OpenLockFile();
			ByteArrayBuffer buffer = NewLockFileBuffer();
			Read(_lockFile, buffer);
			for (int i = 0; i < 2; i++)
			{
				int checkInt = buffer.ReadInt();
				if (checkInt != LockInt)
				{
					CloseLockFile();
					return false;
				}
			}
			CloseLockFile();
			return true;
		}

		public override void Close()
		{
			if (!LogsOpened())
			{
				return;
			}
			CloseLockFile();
			CloseLogFile();
			DeleteLockFile();
			DeleteLogFile();
		}

		private void CloseLockFile()
		{
			SyncAndClose(_lockFile);
			_lockFile = null;
		}

		private void SyncAndClose(IBin bin)
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

		private void CloseLogFile()
		{
			SyncAndClose(_logFile);
			_logFile = null;
		}

		private void DeleteLockFile()
		{
			File4.Delete(LockFileName(_fileName));
		}

		private void DeleteLogFile()
		{
			File4.Delete(LogFileName(_fileName));
		}

		public override Slot AllocateSlot(bool append, int slotChangeCount)
		{
			// do nothing
			return null;
		}

		public override void ApplySlotChanges(IVisitable slotChangeTree, int slotChangeCount
			, Slot reservedSlot)
		{
			if (slotChangeCount < 1)
			{
				return;
			}
			IRunnable commitHook = _container.CommitHook();
			FlushDatabaseFile();
			EnsureLogAndLock();
			int length = TransactionLogSlotLength(slotChangeCount);
			ByteArrayBuffer logBuffer = new ByteArrayBuffer(length);
			logBuffer.WriteInt(length);
			logBuffer.WriteInt(slotChangeCount);
			AppendSlotChanges(logBuffer, slotChangeTree);
			Write(_logFile, logBuffer);
			_logFile.Sync();
			WriteToLockFile(LockInt);
			WriteSlots(slotChangeTree);
			commitHook.Run();
			FlushDatabaseFile();
			WriteToLockFile(0);
		}

		private void WriteToLockFile(int lockSignal)
		{
			ByteArrayBuffer lockBuffer = NewLockFileBuffer();
			lockBuffer.WriteInt(lockSignal);
			lockBuffer.WriteInt(lockSignal);
			Write(_lockFile, lockBuffer);
			_lockFile.Sync();
		}

		private ByteArrayBuffer NewLockFileBuffer()
		{
			return new ByteArrayBuffer(LockFileBufferLength());
		}

		private int LockFileBufferLength()
		{
			return Const4.LongLength * 2;
		}

		private void EnsureLogAndLock()
		{
			if (_container.Config().IsReadOnly())
			{
				return;
			}
			if (LogsOpened())
			{
				return;
			}
			OpenLockFile();
			OpenLogFile();
		}

		private void OpenLogFile()
		{
			_logFile = OpenBin(LogFileName(_fileName));
		}

		private void OpenLockFile()
		{
			_lockFile = OpenBin(LockFileName(_fileName));
		}

		private bool LogsOpened()
		{
			return _lockFile != null;
		}

		private void Read(IBin storage, ByteArrayBuffer buffer)
		{
			storage.Read(0, buffer._buffer, buffer.Length());
		}

		private void Write(IBin storage, ByteArrayBuffer buffer)
		{
			storage.Write(0, buffer._buffer, buffer.Length());
		}
	}
}
