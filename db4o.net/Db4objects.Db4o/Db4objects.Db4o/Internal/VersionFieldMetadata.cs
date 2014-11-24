/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class VersionFieldMetadata : VirtualFieldMetadata
	{
		internal VersionFieldMetadata() : base(Handlers4.LongId, new LongHandler())
		{
			SetName(VirtualField.Version);
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public override void AddFieldIndex(ObjectIdContextImpl context)
		{
			StatefulBuffer buffer = (StatefulBuffer)context.Buffer();
			buffer.WriteLong(context.Transaction().Container().GenerateTimeStampId());
		}

		public override void Delete(DeleteContextImpl context, bool isUpdate)
		{
			context.Seek(context.Offset() + LinkLength(context));
		}

		internal override void Instantiate1(ObjectReferenceContext context)
		{
			context.ObjectReference().VirtualAttributes().i_version = context.ReadLong();
		}

		internal override void Marshall(Transaction trans, ObjectReference @ref, IWriteBuffer
			 buffer, bool isMigrating, bool isNew)
		{
			VirtualAttributes attr = @ref.VirtualAttributes();
			if (!isMigrating)
			{
				attr.i_version = trans.Container().GenerateTimeStampId();
			}
			if (attr == null)
			{
				buffer.WriteLong(0);
			}
			else
			{
				buffer.WriteLong(attr.i_version);
			}
		}

		public override int LinkLength(IHandlerVersionContext context)
		{
			return Const4.LongLength;
		}

		internal override void MarshallIgnore(IWriteBuffer buffer)
		{
			buffer.WriteLong(0);
		}
	}
}
