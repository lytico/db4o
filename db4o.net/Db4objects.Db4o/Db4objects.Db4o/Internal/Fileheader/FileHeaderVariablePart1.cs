/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fileheader;
using Db4objects.Db4o.Internal.Slots;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public class FileHeaderVariablePart1 : FileHeaderVariablePart
	{
		private const int Length = 1 + (Const4.IntLength * 4) + Const4.LongLength + Const4
			.AddedLength;

		private int _id;

		public FileHeaderVariablePart1(LocalObjectContainer container, int id) : base(container
			)
		{
			// The variable part format is:
			// (int) converter version
			// (byte) freespace system used
			// (int)  freespace address
			// (int) identity ID
			// (long) versionGenerator
			// (int) uuid index ID
			_id = id;
		}

		public FileHeaderVariablePart1(LocalObjectContainer container) : this(container, 
			0)
		{
		}

		public virtual int OwnLength()
		{
			return Length;
		}

		public virtual void ReadThis(ByteArrayBuffer buffer)
		{
			SystemData().ConverterVersion(buffer.ReadInt());
			SystemData().FreespaceSystem(buffer.ReadByte());
			buffer.ReadInt();
			// was BTreeFreespaceId, converted to slot, can no longer be used
			SystemData().IdentityId(buffer.ReadInt());
			SystemData().LastTimeStampID(buffer.ReadLong());
			SystemData().UuidIndexId(buffer.ReadInt());
		}

		public virtual void WriteThis(ByteArrayBuffer buffer)
		{
			throw new InvalidOperationException();
		}

		public override IRunnable Commit(bool shuttingDown)
		{
			throw new InvalidOperationException();
		}

		public virtual int Id()
		{
			return _id;
		}

		public override void Read(int variablePartID, int unused)
		{
			_id = variablePartID;
			Slot slot = _container.ReadPointerSlot(_id);
			ByteArrayBuffer buffer = _container.ReadBufferBySlot(slot);
			ReadThis(buffer);
		}

		public override int MarshalledLength()
		{
			return OwnLength();
		}
	}
}
