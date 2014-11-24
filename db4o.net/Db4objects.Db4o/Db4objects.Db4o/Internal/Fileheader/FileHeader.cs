/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public abstract class FileHeader
	{
		public const int TransactionPointerLength = Const4.IntLength * 2;

		private static readonly FileHeader[] AvailableFileHeaders = new FileHeader[] { new 
			FileHeader1(), new FileHeader2(), new FileHeader3() };

		public static NewFileHeaderBase NewCurrentFileHeader()
		{
			return new FileHeader3();
		}

		private static int ReaderLength()
		{
			int length = AvailableFileHeaders[0].Length();
			for (int i = 1; i < AvailableFileHeaders.Length; i++)
			{
				length = Math.Max(length, AvailableFileHeaders[i].Length());
			}
			return length;
		}

		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		public static FileHeader Read(LocalObjectContainer file)
		{
			ByteArrayBuffer reader = PrepareFileHeaderReader(file);
			FileHeader header = DetectFileHeader(file, reader);
			if (header == null)
			{
				Exceptions4.ThrowRuntimeException(Db4objects.Db4o.Internal.Messages.IncompatibleFormat
					, file.ToString());
			}
			else
			{
				header.Read(file, reader);
			}
			return header;
		}

		public virtual FileHeader Convert(LocalObjectContainer file)
		{
			FileHeader3 fileHeader = new FileHeader3();
			fileHeader.InitNew(file);
			return fileHeader;
		}

		private static ByteArrayBuffer PrepareFileHeaderReader(LocalObjectContainer file)
		{
			ByteArrayBuffer reader = new ByteArrayBuffer(ReaderLength());
			reader.Read(file, 0, 0);
			return reader;
		}

		private static FileHeader DetectFileHeader(LocalObjectContainer file, ByteArrayBuffer
			 reader)
		{
			for (int i = 0; i < AvailableFileHeaders.Length; i++)
			{
				reader.Seek(0);
				FileHeader result = AvailableFileHeaders[i].NewOnSignatureMatch(file, reader);
				if (result != null)
				{
					return result;
				}
			}
			return null;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void Close();

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public abstract void InitNew(LocalObjectContainer file);

		public abstract void CompleteInterruptedTransaction(LocalObjectContainer container
			);

		public abstract int Length();

		protected abstract FileHeader NewOnSignatureMatch(LocalObjectContainer file, ByteArrayBuffer
			 reader);

		protected virtual long TimeToWrite(long time, bool shuttingDown)
		{
			return shuttingDown ? 0 : time;
		}

		protected abstract void Read(LocalObjectContainer file, ByteArrayBuffer reader);

		protected virtual bool SignatureMatches(ByteArrayBuffer reader, byte[] signature, 
			byte version)
		{
			for (int i = 0; i < signature.Length; i++)
			{
				if (reader.ReadByte() != signature[i])
				{
					return false;
				}
			}
			return reader.ReadByte() == version;
		}

		// TODO: freespaceID should not be passed here, it should be taken from SystemData
		public abstract void WriteFixedPart(LocalObjectContainer file, bool startFileLockingThread
			, bool shuttingDown, StatefulBuffer writer, int blockSize);

		public abstract void WriteTransactionPointer(Transaction systemTransaction, int transactionPointer
			);

		protected virtual void WriteTransactionPointer(Transaction systemTransaction, int
			 transactionPointer, int address, int offset)
		{
			StatefulBuffer bytes = new StatefulBuffer(systemTransaction, address, TransactionPointerLength
				);
			bytes.MoveForward(offset);
			bytes.WriteInt(transactionPointer);
			bytes.WriteInt(transactionPointer);
			// Dangerous write. 
			// On corruption transaction pointers will not be the same and nothing will happen.
			bytes.Write();
		}

		public virtual void WriteVariablePart(LocalObjectContainer file)
		{
			WriteVariablePart(file, false);
		}

		public abstract void WriteVariablePart(LocalObjectContainer file, bool shuttingDown
			);

		public static bool LockedByOtherSession(LocalObjectContainer container, long lastAccessTime
			)
		{
			return container.NeedsLockFileThread() && (lastAccessTime != 0);
		}

		public abstract void ReadIdentity(LocalObjectContainer container);

		public abstract IRunnable Commit(bool shuttingDown);
	}
}
