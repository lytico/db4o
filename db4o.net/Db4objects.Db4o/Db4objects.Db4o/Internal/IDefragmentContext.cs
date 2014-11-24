/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	public interface IDefragmentContext : IBufferContext, IMarshallingInfo, IHandlerVersionContext
	{
		ITypeHandler4 TypeHandlerForId(int id);

		int CopyID();

		int CopyIDReturnOriginalID();

		int CopySlotlessID();

		int CopyUnindexedID();

		void Defragment(ITypeHandler4 handler);

		int HandlerVersion();

		void IncrementOffset(int length);

		bool IsLegacyHandlerVersion();

		int MappedID(int origID);

		ByteArrayBuffer SourceBuffer();

		ByteArrayBuffer TargetBuffer();

		Slot AllocateTargetSlot(int length);

		Slot AllocateMappedTargetSlot(int sourceAddress, int length);

		/// <exception cref="System.IO.IOException"></exception>
		int CopySlotToNewMapped(int sourceAddress, int length);

		/// <exception cref="System.IO.IOException"></exception>
		ByteArrayBuffer SourceBufferByAddress(int sourceAddress, int length);

		/// <exception cref="System.IO.IOException"></exception>
		ByteArrayBuffer SourceBufferById(int sourceId);

		void TargetWriteBytes(int address, ByteArrayBuffer buffer);

		IDefragmentServices Services();

		ObjectContainerBase Container();
	}
}
