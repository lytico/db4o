/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public abstract class NewFileHeaderBase : FileHeader
	{
		protected static readonly byte[] Signature = new byte[] { (byte)'d', (byte)'b', (
			byte)'4', (byte)'o' };

		protected static readonly int HeaderLockOffset = Signature.Length + 1;

		protected static readonly int OpenTimeOffset = HeaderLockOffset + Const4.IntLength;

		protected static readonly int AccessTimeOffset = OpenTimeOffset + Const4.LongLength;

		protected TimerFileLock _timerFileLock;

		protected FileHeaderVariablePart _variablePart;

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Close()
		{
			if (_timerFileLock == null)
			{
				return;
			}
			_timerFileLock.Close();
		}

		protected virtual void NewTimerFileLock(LocalObjectContainer file)
		{
			_timerFileLock = TimerFileLock.ForFile(file);
			_timerFileLock.SetAddresses(0, OpenTimeOffset, AccessTimeOffset);
		}

		protected abstract NewFileHeaderBase CreateNew();

		protected abstract byte Version();

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public sealed override void InitNew(LocalObjectContainer file)
		{
			NewTimerFileLock(file);
			OldEncryptionOff(file);
			_variablePart = CreateVariablePart(file);
			WriteVariablePart(file);
		}

		public abstract FileHeaderVariablePart CreateVariablePart(LocalObjectContainer file
			);

		protected virtual void OldEncryptionOff(LocalObjectContainer file)
		{
			file._handlers.OldEncryptionOff();
		}

		public sealed override void WriteVariablePart(LocalObjectContainer file, bool shuttingDown
			)
		{
			if (!IsInitalized())
			{
				return;
			}
			IRunnable commitHook = Commit(shuttingDown);
			file.SyncFiles();
			commitHook.Run();
			file.SyncFiles();
		}

		private bool IsInitalized()
		{
			return _variablePart != null;
		}

		protected override FileHeader NewOnSignatureMatch(LocalObjectContainer file, ByteArrayBuffer
			 reader)
		{
			if (SignatureMatches(reader, Signature, Version()))
			{
				return CreateNew();
			}
			return null;
		}

		public override void CompleteInterruptedTransaction(LocalObjectContainer container
			)
		{
			SystemData systemData = container.SystemData();
			container.IdSystem().CompleteInterruptedTransaction(systemData.TransactionPointer1
				(), systemData.TransactionPointer2());
		}

		protected virtual void CheckThreadFileLock(LocalObjectContainer container, ByteArrayBuffer
			 reader)
		{
			reader.Seek(AccessTimeOffset);
			long lastAccessTime = reader.ReadLong();
			if (FileHeader.LockedByOtherSession(container, lastAccessTime))
			{
				_timerFileLock.CheckIfOtherSessionAlive(container, 0, AccessTimeOffset, lastAccessTime
					);
			}
		}

		public override void ReadIdentity(LocalObjectContainer container)
		{
			_variablePart.ReadIdentity((LocalTransaction)container.SystemTransaction());
		}

		public override IRunnable Commit(bool shuttingDown)
		{
			return _variablePart.Commit(shuttingDown);
		}
	}
}
