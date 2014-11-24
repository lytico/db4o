/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class StringHandler0 : StringHandler
	{
		public override object Read(IReadContext context)
		{
			ByteArrayBuffer buffer = (ByteArrayBuffer)((IInternalReadContext)context).ReadIndirectedBuffer
				();
			if (buffer == null)
			{
				return null;
			}
			return ReadString(context, buffer);
		}

		public override void Delete(IDeleteContext context)
		{
			context.DefragmentRecommended();
		}

		public override void Defragment(IDefragmentContext context)
		{
			int sourceAddress = context.SourceBuffer().ReadInt();
			int length = context.SourceBuffer().ReadInt();
			if (sourceAddress == 0 && length == 0)
			{
				context.TargetBuffer().WriteInt(0);
				context.TargetBuffer().WriteInt(0);
				return;
			}
			int targetAddress = 0;
			try
			{
				targetAddress = context.CopySlotToNewMapped(sourceAddress, length);
			}
			catch (IOException exc)
			{
				throw new Db4oIOException(exc);
			}
			context.TargetBuffer().WriteInt(targetAddress);
			context.TargetBuffer().WriteInt(length);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override object ReadIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer
			 buffer)
		{
			return buffer.Container().ReadWriterByAddress(buffer.Transaction(), buffer.ReadInt
				(), buffer.ReadInt());
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override object ReadIndexEntry(IObjectIdContext context)
		{
			return context.Transaction().Container().ReadWriterByAddress(context.Transaction(
				), context.ReadInt(), context.ReadInt());
		}
	}
}
