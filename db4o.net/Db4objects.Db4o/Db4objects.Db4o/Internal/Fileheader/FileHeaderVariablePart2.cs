/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;
using Db4objects.Db4o.Internal.Slots;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class FileHeaderVariablePart2 : FileHeaderVariablePart
	{
		private const int ChecksumLength = Const4.LongLength;

		private const int SingleLength = ChecksumLength + (Const4.IntLength * 8) + Const4
			.LongLength + 1 + Const4.AddedLength;

		private int _address;

		private int _length;

		public FileHeaderVariablePart2(LocalObjectContainer container, int address, int length
			) : base(container)
		{
			// The variable part format is:
			// (long) checksum
			// (int) address of InMemoryIdSystem slot
			// (int) length of InMemoryIdSystem slot
			// (int) address of InMemoryFreespace
			// (int) length of InMemoryFreespace
			// (int) BTreeFreespace id
			// (int) converter version
			// (int) uuid index ID
			// (int) identity ID
			// (long) versionGenerator
			// (byte) freespace system used
			_address = address;
			_length = length;
		}

		public FileHeaderVariablePart2(LocalObjectContainer container) : this(container, 
			0, 0)
		{
		}

		public override IRunnable Commit(bool shuttingDown)
		{
			int length = OwnLength();
			if (_address == 0 || _length < length)
			{
				Slot slot = AllocateSlot(MarshalledLength(length));
				_address = slot.Address();
				_length = length;
			}
			ByteArrayBuffer buffer = new ByteArrayBuffer(length);
			Marshall(buffer, shuttingDown);
			WriteToFile(0, buffer);
			return new _IRunnable_65(this, length, buffer);
		}

		private sealed class _IRunnable_65 : IRunnable
		{
			public _IRunnable_65(FileHeaderVariablePart2 _enclosing, int length, ByteArrayBuffer
				 buffer)
			{
				this._enclosing = _enclosing;
				this.length = length;
				this.buffer = buffer;
			}

			public void Run()
			{
				this._enclosing.WriteToFile(length * 2, buffer);
			}

			private readonly FileHeaderVariablePart2 _enclosing;

			private readonly int length;

			private readonly ByteArrayBuffer buffer;
		}

		private int MarshalledLength(int length)
		{
			return length * 4;
		}

		private void WriteToFile(int startAdress, ByteArrayBuffer buffer)
		{
			_container.WriteEncrypt(buffer, _address, startAdress);
			_container.WriteEncrypt(buffer, _address, startAdress + _length);
		}

		public virtual int OwnLength()
		{
			return SingleLength;
		}

		public virtual int Address()
		{
			return _address;
		}

		public virtual int Length()
		{
			return _length;
		}

		public override void Read(int address, int length)
		{
			_address = address;
			_length = length;
			ByteArrayBuffer buffer = _container.ReadBufferBySlot(new Slot(address, MarshalledLength
				(length)));
			bool versionsAreConsistent = VersionsAreConsistentAndSeek(buffer);
			// TODO: Diagnostic message if versions aren't consistent.
			ReadBuffer(buffer, versionsAreConsistent);
		}

		protected virtual void ReadBuffer(ByteArrayBuffer buffer, bool versionsAreConsistent
			)
		{
			buffer.IncrementOffset(ChecksumLength);
			SystemData systemData = SystemData();
			systemData.IdSystemSlot(ReadSlot(buffer, false));
			systemData.InMemoryFreespaceSlot(ReadSlot(buffer, !versionsAreConsistent));
			systemData.BTreeFreespaceId(buffer.ReadInt());
			systemData.ConverterVersion(buffer.ReadInt());
			systemData.UuidIndexId(buffer.ReadInt());
			systemData.IdentityId(buffer.ReadInt());
			systemData.LastTimeStampID(buffer.ReadLong());
			systemData.FreespaceSystem(buffer.ReadByte());
		}

		private Slot ReadSlot(ByteArrayBuffer buffer, bool readZero)
		{
			Slot slot = new Slot(buffer.ReadInt(), buffer.ReadInt());
			if (readZero)
			{
				return Slot.Zero;
			}
			return slot;
		}

		private void Marshall(ByteArrayBuffer buffer, bool shuttingDown)
		{
			int checkSumOffset = buffer.Offset();
			buffer.IncrementOffset(ChecksumLength);
			int checkSumBeginOffset = buffer.Offset();
			WriteBuffer(buffer, shuttingDown);
			int checkSumEndOffSet = buffer.Offset();
			byte[] bytes = buffer._buffer;
			int length = checkSumEndOffSet - checkSumBeginOffset;
			long checkSum = CRC32.CheckSum(bytes, checkSumBeginOffset, length);
			buffer.Seek(checkSumOffset);
			buffer.WriteLong(checkSum);
			buffer.Seek(checkSumEndOffSet);
		}

		protected virtual void WriteBuffer(ByteArrayBuffer buffer, bool shuttingDown)
		{
			SystemData systemData = SystemData();
			WriteSlot(buffer, systemData.IdSystemSlot(), false);
			WriteSlot(buffer, systemData.InMemoryFreespaceSlot(), !shuttingDown);
			buffer.WriteInt(systemData.BTreeFreespaceId());
			buffer.WriteInt(systemData.ConverterVersion());
			buffer.WriteInt(systemData.UuidIndexId());
			Db4oDatabase identity = systemData.Identity();
			buffer.WriteInt(identity == null ? 0 : identity.GetID(_container.SystemTransaction
				()));
			buffer.WriteLong(systemData.LastTimeStampID());
			buffer.WriteByte(systemData.FreespaceSystem());
		}

		private void WriteSlot(ByteArrayBuffer buffer, Slot slot, bool writeZero)
		{
			if (writeZero || slot == null)
			{
				buffer.WriteInt(0);
				buffer.WriteInt(0);
				return;
			}
			buffer.WriteInt(slot.Address());
			buffer.WriteInt(slot.Length());
		}

		private bool CheckSumOK(ByteArrayBuffer buffer, int offset)
		{
			int initialOffSet = buffer.Offset();
			int length = OwnLength();
			length -= ChecksumLength;
			buffer.Seek(offset);
			long readCheckSum = buffer.ReadLong();
			int checkSumBeginOffset = buffer.Offset();
			byte[] bytes = buffer._buffer;
			long calculatedCheckSum = CRC32.CheckSum(bytes, checkSumBeginOffset, length);
			buffer.Seek(initialOffSet);
			return calculatedCheckSum == readCheckSum;
		}

		private bool VersionsAreConsistentAndSeek(ByteArrayBuffer buffer)
		{
			byte[] bytes = buffer._buffer;
			int length = OwnLength();
			int[] offsets = Offsets();
			bool different = false;
			for (int i = 0; i < length; i++)
			{
				byte b = bytes[offsets[0] + i];
				for (int j = 1; j < 4; j++)
				{
					if (b != bytes[offsets[j] + i])
					{
						different = true;
						break;
					}
				}
			}
			if (!different)
			{
				// The following line cements our checksum algorithm in stone.
				// Things should be safe enough if we remove the throw.
				// If all four versions of the header are the same,
				// it's bound to be OK. (unless all bytes are zero or
				// greyed out by some kind of overwriting algorithm.)
				int firstOffset = 0;
				if (!CheckSumOK(buffer, firstOffset))
				{
					throw new Db4oFileHeaderCorruptionException();
				}
				return true;
			}
			bool firstPairDiffers = false;
			bool secondPairDiffers = false;
			for (int i = 0; i < length; i++)
			{
				if (bytes[offsets[0] + i] != bytes[offsets[1] + i])
				{
					firstPairDiffers = true;
				}
				if (bytes[offsets[2] + i] != bytes[offsets[3] + i])
				{
					secondPairDiffers = true;
				}
			}
			if (!secondPairDiffers)
			{
				if (CheckSumOK(buffer, offsets[2]))
				{
					buffer.Seek(offsets[2]);
					return false;
				}
			}
			if (firstPairDiffers)
			{
				// Should never ever happen, we are toast.
				// We could still try to use any random version of
				// the header but which one?
				// Maybe the first of the second pair could be an 
				// option for a recovery tool, or it could try all
				// versions.
				throw new Db4oFileHeaderCorruptionException();
			}
			if (!CheckSumOK(buffer, 0))
			{
				throw new Db4oFileHeaderCorruptionException();
			}
			return false;
		}

		private int[] Offsets()
		{
			return new int[] { 0, OwnLength(), OwnLength() * 2, OwnLength() * 3 };
		}

		public override int MarshalledLength()
		{
			return MarshalledLength(OwnLength());
		}
	}
}
