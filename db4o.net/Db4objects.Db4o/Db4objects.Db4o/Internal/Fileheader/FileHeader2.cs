/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;
using Sharpen;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class FileHeader2 : NewFileHeaderBase
	{
		private static readonly int BlocksizeOffset = AccessTimeOffset + Const4.LongLength;

		public static readonly int HeaderLength = BlocksizeOffset + (Const4.IntLength * 5
			) + 1;

		private int _transactionPointerAddress = 0;

		// The header format is:
		// (byte) 'd'
		// (byte) 'b'
		// (byte) '4'
		// (byte) 'o'
		// (byte) headerVersion
		// (int) headerLock
		// (long) openTime
		// (long) accessTime
		// (int) blockSize
		// (int) classCollectionID
		// (byte) idSystemType
		// (int) variable part address
		// (int) variable part length
		// (int) transaction pointer address
		public override int Length()
		{
			return HeaderLength;
		}

		protected override void Read(LocalObjectContainer container, ByteArrayBuffer reader
			)
		{
			NewTimerFileLock(container);
			OldEncryptionOff(container);
			CheckThreadFileLock(container, reader);
			reader.Seek(BlocksizeOffset);
			container.BlockSizeReadFromFile(reader.ReadInt());
			SystemData systemData = container.SystemData();
			systemData.ClassCollectionID(reader.ReadInt());
			container.SystemData().IdSystemType(reader.ReadByte());
			_variablePart = CreateVariablePart(container);
			int variablePartAddress = reader.ReadInt();
			int variablePartLength = reader.ReadInt();
			_variablePart.Read(variablePartAddress, variablePartLength);
			_transactionPointerAddress = reader.ReadInt();
			if (_transactionPointerAddress != 0)
			{
				ByteArrayBuffer buffer = new ByteArrayBuffer(TransactionPointerLength);
				buffer.Read(container, _transactionPointerAddress, 0);
				systemData.TransactionPointer1(buffer.ReadInt());
				systemData.TransactionPointer2(buffer.ReadInt());
			}
		}

		public override void WriteFixedPart(LocalObjectContainer file, bool startFileLockingThread
			, bool shuttingDown, StatefulBuffer writer, int blockSize)
		{
			SystemData systemData = file.SystemData();
			writer.Append(Signature);
			writer.WriteByte(Version());
			writer.WriteInt((int)TimeToWrite(_timerFileLock.OpenTime(), shuttingDown));
			writer.WriteLong(TimeToWrite(_timerFileLock.OpenTime(), shuttingDown));
			writer.WriteLong(TimeToWrite(Runtime.CurrentTimeMillis(), shuttingDown));
			writer.WriteInt(blockSize);
			writer.WriteInt(systemData.ClassCollectionID());
			writer.WriteByte(systemData.IdSystemType());
			writer.WriteInt(((FileHeaderVariablePart2)_variablePart).Address());
			writer.WriteInt(((FileHeaderVariablePart2)_variablePart).Length());
			writer.WriteInt(_transactionPointerAddress);
			writer.Write();
			if (shuttingDown)
			{
				WriteVariablePart(file, true);
			}
			else
			{
				file.SyncFiles();
			}
			if (startFileLockingThread)
			{
				file.ThreadPool().Start("db4o lock thread", _timerFileLock);
			}
		}

		public override void WriteTransactionPointer(Transaction systemTransaction, int transactionPointer
			)
		{
			if (_transactionPointerAddress == 0)
			{
				LocalObjectContainer file = ((LocalTransaction)systemTransaction).LocalContainer(
					);
				_transactionPointerAddress = file.AllocateSafeSlot(TransactionPointerLength).Address
					();
				file.WriteHeader(false, false);
			}
			WriteTransactionPointer(systemTransaction, transactionPointer, _transactionPointerAddress
				, 0);
		}

		protected override byte Version()
		{
			return (byte)2;
		}

		protected override NewFileHeaderBase CreateNew()
		{
			return new FileHeader2();
		}

		public override FileHeaderVariablePart CreateVariablePart(LocalObjectContainer file
			)
		{
			return new FileHeaderVariablePart2(file);
		}
	}
}
