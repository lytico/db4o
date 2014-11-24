/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;
using Sharpen;

namespace Db4objects.Db4o.Internal
{
	/// <summary>
	/// public for .NET conversion reasons
	/// TODO: Split this class for individual usecases.
	/// </summary>
	/// <remarks>
	/// public for .NET conversion reasons
	/// TODO: Split this class for individual usecases. Only use the member
	/// variables needed for the respective usecase.
	/// </remarks>
	/// <exclude></exclude>
	public sealed class StatefulBuffer : ByteArrayBuffer
	{
		internal Db4objects.Db4o.Internal.Transaction _trans;

		private int _address;

		private int _addressOffset;

		private int _cascadeDelete;

		private int _id;

		private int _length;

		public StatefulBuffer(Db4objects.Db4o.Internal.Transaction trans, int initialBufferSize
			)
		{
			_trans = trans;
			_length = initialBufferSize;
			_buffer = new byte[_length];
		}

		public StatefulBuffer(Db4objects.Db4o.Internal.Transaction trans, int address, int
			 length) : this(trans, length)
		{
			_address = address;
		}

		public StatefulBuffer(Db4objects.Db4o.Internal.Transaction trans, Db4objects.Db4o.Internal.Slots.Slot
			 slot) : this(trans, slot.Address(), slot.Length())
		{
		}

		public StatefulBuffer(Db4objects.Db4o.Internal.Transaction trans, Pointer4 pointer
			) : this(trans, pointer._slot)
		{
			_id = pointer._id;
		}

		public void DebugCheckBytes()
		{
		}

		// Db4o.log("!!! YapBytes.debugCheckBytes not all bytes used");
		// This is normal for writing The FreeSlotArray, becauce one
		// slot is possibly reserved by it's own pointer.
		public int GetAddress()
		{
			return _address;
		}

		public int GetID()
		{
			return _id;
		}

		public override int Length()
		{
			return _length;
		}

		public ObjectContainerBase Container()
		{
			return _trans.Container();
		}

		public LocalObjectContainer File()
		{
			return ((LocalTransaction)_trans).LocalContainer();
		}

		public Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _trans;
		}

		public byte[] GetWrittenBytes()
		{
			byte[] bytes = new byte[_offset];
			System.Array.Copy(_buffer, 0, bytes, 0, _offset);
			return bytes;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public void Read()
		{
			Container().ReadBytes(_buffer, _address, _addressOffset, _length);
		}

		public Db4objects.Db4o.Internal.StatefulBuffer ReadStatefulBuffer()
		{
			int length = ReadInt();
			if (length == 0)
			{
				return null;
			}
			Db4objects.Db4o.Internal.StatefulBuffer yb = new Db4objects.Db4o.Internal.StatefulBuffer
				(_trans, length);
			System.Array.Copy(_buffer, _offset, yb._buffer, 0, length);
			_offset += length;
			return yb;
		}

		public void RemoveFirstBytes(int aLength)
		{
			_length -= aLength;
			byte[] temp = new byte[_length];
			System.Array.Copy(_buffer, aLength, temp, 0, _length);
			_buffer = temp;
			_offset -= aLength;
			if (_offset < 0)
			{
				_offset = 0;
			}
		}

		public void Address(int address)
		{
			_address = address;
		}

		public void SetID(int id)
		{
			_id = id;
		}

		public void SetTransaction(Db4objects.Db4o.Internal.Transaction aTrans)
		{
			_trans = aTrans;
		}

		public void UseSlot(int adress)
		{
			_address = adress;
			_offset = 0;
		}

		// FIXME: FB remove
		public void UseSlot(int address, int length)
		{
			UseSlot(new Db4objects.Db4o.Internal.Slots.Slot(address, length));
		}

		public void UseSlot(Db4objects.Db4o.Internal.Slots.Slot slot)
		{
			_address = slot.Address();
			_offset = 0;
			if (slot.Length() > _buffer.Length)
			{
				_buffer = new byte[slot.Length()];
			}
			_length = slot.Length();
		}

		// FIXME: FB remove
		public void UseSlot(int id, int adress, int length)
		{
			_id = id;
			UseSlot(adress, length);
		}

		public void Write()
		{
			File().WriteBytes(this, _address, _addressOffset);
		}

		public void WriteEncrypt()
		{
			File().WriteEncrypt(this, _address, _addressOffset);
		}

		public ByteArrayBuffer ReadPayloadWriter(int offset, int length)
		{
			Db4objects.Db4o.Internal.StatefulBuffer payLoad = new Db4objects.Db4o.Internal.StatefulBuffer
				(_trans, 0, length);
			System.Array.Copy(_buffer, offset, payLoad._buffer, 0, length);
			TransferPayLoadAddress(payLoad, offset);
			return payLoad;
		}

		private void TransferPayLoadAddress(Db4objects.Db4o.Internal.StatefulBuffer toWriter
			, int offset)
		{
			int blockedOffset = offset / Container().BlockSize();
			toWriter._address = _address + blockedOffset;
			toWriter._id = toWriter._address;
			toWriter._addressOffset = _addressOffset;
		}

		public void MoveForward(int length)
		{
			_addressOffset += length;
		}

		public override string ToString()
		{
			return "id " + _id + " adr " + _address + " len " + _length;
		}

		public Db4objects.Db4o.Internal.Slots.Slot Slot()
		{
			return new Db4objects.Db4o.Internal.Slots.Slot(_address, _length);
		}

		public Pointer4 Pointer()
		{
			return new Pointer4(_id, Slot());
		}

		public int CascadeDeletes()
		{
			return _cascadeDelete;
		}

		public void SetCascadeDeletes(int depth)
		{
			_cascadeDelete = depth;
		}
	}
}
