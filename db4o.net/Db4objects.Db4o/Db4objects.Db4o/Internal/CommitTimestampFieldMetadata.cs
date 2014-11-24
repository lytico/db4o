/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class CommitTimestampFieldMetadata : VirtualFieldMetadata
	{
		internal CommitTimestampFieldMetadata() : base(Handlers4.LongId, new LongHandler(
			))
		{
			SetName(VirtualField.CommitTimestamp);
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public override void AddFieldIndex(ObjectIdContextImpl context)
		{
		}

		public override void AddIndexEntry(Transaction trans, int parentID, object indexEntry
			)
		{
		}

		public override void RemoveIndexEntry(Transaction trans, int parentID, object indexEntry
			)
		{
		}

		public override void Delete(DeleteContextImpl context, bool isUpdate)
		{
		}

		internal override void Instantiate1(ObjectReferenceContext context)
		{
		}

		internal override void Marshall(Transaction trans, ObjectReference @ref, IWriteBuffer
			 buffer, bool isMigrating, bool isNew)
		{
		}

		public override int LinkLength(IHandlerVersionContext context)
		{
			return 0;
		}

		public override void DefragAspect(IDefragmentContext context)
		{
		}

		internal override void MarshallIgnore(IWriteBuffer buffer)
		{
		}

		public override void Activate(UnmarshallingContext context)
		{
		}

		// do nothing.
		public override BTree GetIndex(Transaction trans)
		{
			return ((LocalTransaction)trans.SystemTransaction()).CommitTimestampSupport().TimestampToId
				();
		}

		public override bool HasIndex()
		{
			return true;
		}

		protected override IFieldIndexKey CreateFieldIndexKey(int parentID, object indexEntry
			)
		{
			return new CommitTimestampSupport.TimestampEntry(parentID, ((long)indexEntry));
		}

		internal int counter = 0;

		public override object Read(IObjectIdContext context)
		{
			int objectId = context.ObjectId();
			long version = context.Transaction().SystemTransaction().VersionForId(objectId);
			return version;
		}
	}
}
