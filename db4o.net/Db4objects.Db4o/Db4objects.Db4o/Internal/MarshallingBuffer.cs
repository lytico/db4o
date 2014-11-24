/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Sharpen;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class MarshallingBuffer : IWriteBuffer
	{
		private const int SizeNeeded = Const4.LongLength;

		private const int NoParent = -int.MaxValue;

		private ByteArrayBuffer _delegate;

		private int _lastOffSet;

		private int _addressInParent = NoParent;

		private List4 _children;

		private FieldMetadata _indexedField;

		public virtual int Length()
		{
			return Offset();
		}

		public virtual int Offset()
		{
			if (_delegate == null)
			{
				return 0;
			}
			return _delegate.Offset();
		}

		public virtual void WriteByte(byte b)
		{
			PrepareWrite();
			_delegate.WriteByte(b);
		}

		public virtual void WriteBytes(byte[] bytes)
		{
			PrepareWrite(bytes.Length);
			_delegate.WriteBytes(bytes);
		}

		public virtual void WriteInt(int i)
		{
			PrepareWrite();
			_delegate.WriteInt(i);
		}

		public virtual void WriteLong(long l)
		{
			PrepareWrite();
			_delegate.WriteLong(l);
		}

		private void PrepareWrite()
		{
			PrepareWrite(SizeNeeded);
		}

		public virtual void PrepareWrite(int sizeNeeded)
		{
			if (_delegate == null)
			{
				_delegate = new ByteArrayBuffer(sizeNeeded);
			}
			_lastOffSet = _delegate.Offset();
			if (RemainingSize() < sizeNeeded)
			{
				Resize(sizeNeeded);
			}
		}

		private int RemainingSize()
		{
			return _delegate.Length() - _delegate.Offset();
		}

		private void Resize(int sizeNeeded)
		{
			int newSize = _delegate.Length() * 2;
			if (newSize - _lastOffSet < sizeNeeded)
			{
				newSize += sizeNeeded;
			}
			ByteArrayBuffer temp = new ByteArrayBuffer(newSize);
			temp.Seek(_lastOffSet);
			_delegate.CopyTo(temp, 0, 0, _delegate.Length());
			_delegate = temp;
		}

		public virtual void TransferLastWriteTo(MarshallingBuffer other, bool storeLengthInLink
			)
		{
			other.AddressInParent(_lastOffSet, storeLengthInLink);
			int length = _delegate.Offset() - _lastOffSet;
			other.PrepareWrite(length);
			int otherOffset = other._delegate.Offset();
			System.Array.Copy(_delegate._buffer, _lastOffSet, other._delegate._buffer, otherOffset
				, length);
			_delegate.Seek(_lastOffSet);
			other._delegate.Seek(otherOffset + length);
			other._lastOffSet = otherOffset;
		}

		private void AddressInParent(int offset, bool storeLengthInLink)
		{
			_addressInParent = storeLengthInLink ? offset : -offset;
		}

		public virtual void TransferContentTo(ByteArrayBuffer buffer)
		{
			TransferContentTo(buffer, Length());
		}

		public virtual void TransferContentTo(ByteArrayBuffer buffer, int length)
		{
			if (_delegate == null)
			{
				return;
			}
			System.Array.Copy(_delegate._buffer, 0, buffer._buffer, buffer._offset, length);
			buffer._offset += length;
		}

		public virtual ByteArrayBuffer TestDelegate()
		{
			return _delegate;
		}

		public virtual MarshallingBuffer AddChild()
		{
			return AddChild(true, false);
		}

		public virtual MarshallingBuffer AddChild(bool reserveLinkSpace, bool storeLengthInLink
			)
		{
			MarshallingBuffer child = new MarshallingBuffer();
			child.AddressInParent(Offset(), storeLengthInLink);
			_children = new List4(_children, child);
			if (reserveLinkSpace)
			{
				ReserveChildLinkSpace(storeLengthInLink);
			}
			return child;
		}

		public virtual void ReserveChildLinkSpace(bool storeLengthInLink)
		{
			int length = storeLengthInLink ? Const4.IntLength * 2 : Const4.IntLength;
			PrepareWrite(length);
			_delegate.IncrementOffset(length);
		}

		public virtual void MergeChildren(MarshallingContext context, int masterAddress, 
			int linkOffset)
		{
			MergeChildren(context, masterAddress, this, this, linkOffset);
		}

		private static void MergeChildren(MarshallingContext context, int masterAddress, 
			MarshallingBuffer writeBuffer, MarshallingBuffer parentBuffer, int linkOffset)
		{
			if (parentBuffer._children == null)
			{
				return;
			}
			IEnumerator i = new Iterator4Impl(parentBuffer._children);
			while (i.MoveNext())
			{
				Merge(context, masterAddress, writeBuffer, parentBuffer, (MarshallingBuffer)i.Current
					, linkOffset);
			}
		}

		private static void Merge(MarshallingContext context, int masterAddress, MarshallingBuffer
			 writeBuffer, MarshallingBuffer parentBuffer, MarshallingBuffer childBuffer, int
			 linkOffset)
		{
			int childPosition = writeBuffer.Offset();
			writeBuffer.Reserve(childBuffer.BlockedLength());
			MergeChildren(context, masterAddress, writeBuffer, childBuffer, linkOffset);
			int savedWriteBufferOffset = writeBuffer.Offset();
			writeBuffer.Seek(childPosition);
			childBuffer.TransferContentTo(writeBuffer._delegate);
			writeBuffer.Seek(savedWriteBufferOffset);
			parentBuffer.WriteLink(childBuffer, childPosition + linkOffset, childBuffer.UnblockedLength
				());
			childBuffer.WriteIndex(context, masterAddress, childPosition + linkOffset);
		}

		public virtual void Seek(int offset)
		{
			_delegate.Seek(offset);
		}

		public virtual IReservedBuffer Reserve(int length)
		{
			PrepareWrite(length);
			IReservedBuffer reservedBuffer = new _IReservedBuffer_178(this);
			_delegate.Seek(_delegate.Offset() + length);
			return reservedBuffer;
		}

		private sealed class _IReservedBuffer_178 : IReservedBuffer
		{
			public _IReservedBuffer_178(MarshallingBuffer _enclosing)
			{
				this._enclosing = _enclosing;
				this.reservedOffset = this._enclosing._delegate.Offset();
			}

			private readonly int reservedOffset;

			public void WriteBytes(byte[] bytes)
			{
				int currentOffset = this._enclosing._delegate.Offset();
				this._enclosing._delegate.Seek(this.reservedOffset);
				this._enclosing._delegate.WriteBytes(bytes);
				this._enclosing._delegate.Seek(currentOffset);
			}

			private readonly MarshallingBuffer _enclosing;
		}

		private void WriteLink(MarshallingBuffer child, int position, int length)
		{
			int offset = Offset();
			_delegate.Seek(child.AddressInParent());
			_delegate.WriteInt(position);
			if (child.StoreLengthInLink())
			{
				_delegate.WriteInt(length);
			}
			_delegate.Seek(offset);
		}

		private void WriteIndex(MarshallingContext context, int masterAddress, int position
			)
		{
			if (_indexedField != null)
			{
				// for now this is a String index only, it takes the entire slot.
				StatefulBuffer buffer = new StatefulBuffer(context.Transaction(), UnblockedLength
					());
				int blockedPosition = context.Container().BlockConverter().BytesToBlocks(position
					);
				int indexID = masterAddress + blockedPosition;
				buffer.SetID(indexID);
				buffer.Address(indexID);
				TransferContentTo(buffer, UnblockedLength());
				_indexedField.AddIndexEntry(context.Transaction(), context.ObjectID(), buffer);
			}
		}

		private int AddressInParent()
		{
			if (!HasParent())
			{
				throw new InvalidOperationException();
			}
			if (_addressInParent < 0)
			{
				return -_addressInParent;
			}
			return _addressInParent;
		}

		public virtual void DebugDecrementLastOffset(int count)
		{
			_lastOffSet -= count;
		}

		public virtual bool HasParent()
		{
			return _addressInParent != NoParent;
		}

		private bool StoreLengthInLink()
		{
			return _addressInParent > 0;
		}

		public virtual void RequestIndexEntry(FieldMetadata fieldMetadata)
		{
			_indexedField = fieldMetadata;
		}

		public virtual MarshallingBuffer CheckBlockAlignment(MarshallingContext context, 
			MarshallingBuffer precedingBuffer, IntByRef precedingLength)
		{
			_lastOffSet = Offset();
			if (DoBlockAlign())
			{
				precedingBuffer.BlockAlign(context, precedingLength.value);
			}
			if (precedingBuffer != null)
			{
				precedingLength.value += precedingBuffer.Length();
			}
			precedingBuffer = this;
			if (_children != null)
			{
				IEnumerator i = new Iterator4Impl(_children);
				while (i.MoveNext())
				{
					precedingBuffer = ((MarshallingBuffer)i.Current).CheckBlockAlignment(context, precedingBuffer
						, precedingLength);
				}
			}
			return precedingBuffer;
		}

		private void BlockAlign(MarshallingContext context, int precedingLength)
		{
			int totalLength = context.Container().BlockConverter().BlockAlignedBytes(precedingLength
				 + Length());
			int newLength = totalLength - precedingLength;
			BlockAlign(newLength);
		}

		public virtual int MarshalledLength()
		{
			int length = Length();
			if (_children != null)
			{
				IEnumerator i = new Iterator4Impl(_children);
				while (i.MoveNext())
				{
					length += ((MarshallingBuffer)i.Current).MarshalledLength();
				}
			}
			return length;
		}

		private void BlockAlign(int length)
		{
			if (_delegate == null)
			{
				return;
			}
			if (length > _delegate.Length())
			{
				int sizeNeeded = length - _delegate.Offset();
				PrepareWrite(sizeNeeded);
			}
			_delegate.Seek(length);
		}

		private bool DoBlockAlign()
		{
			return HasParent();
		}

		// For now we block align every linked entry. Indexes could be created late.
		private int BlockedLength()
		{
			return Length();
		}

		private int UnblockedLength()
		{
			// This is only valid after checkBlockAlignMent has been called. 
			return _lastOffSet;
		}
	}
}
