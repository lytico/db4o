/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class UUIDFieldMetadata : VirtualFieldMetadata
	{
		internal UUIDFieldMetadata() : base(Handlers4.LongId, new LongHandler())
		{
			SetName(Const4.VirtualFieldPrefix + "uuid");
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public override void AddFieldIndex(ObjectIdContextImpl context)
		{
			LocalTransaction transaction = (LocalTransaction)context.Transaction();
			LocalObjectContainer localContainer = (LocalObjectContainer)transaction.Container
				();
			Slot oldSlot = transaction.IdSystem().CommittedSlot(context.ObjectId());
			int savedOffset = context.Offset();
			int db4oDatabaseIdentityID = context.ReadInt();
			long uuid = context.ReadLong();
			context.Seek(savedOffset);
			bool isnew = (oldSlot.IsNull());
			if ((uuid == 0 || db4oDatabaseIdentityID == 0) && context.ObjectId() > 0 && !isnew)
			{
				UUIDFieldMetadata.DatabaseIdentityIDAndUUID identityAndUUID = ReadDatabaseIdentityIDAndUUID
					(localContainer, context.ClassMetadata(), oldSlot, false);
				db4oDatabaseIdentityID = identityAndUUID.databaseIdentityID;
				uuid = identityAndUUID.uuid;
			}
			if (db4oDatabaseIdentityID == 0)
			{
				db4oDatabaseIdentityID = localContainer.Identity().GetID(transaction);
			}
			if (uuid == 0)
			{
				uuid = localContainer.GenerateTimeStampId();
			}
			StatefulBuffer writer = (StatefulBuffer)context.Buffer();
			writer.WriteInt(db4oDatabaseIdentityID);
			writer.WriteLong(uuid);
			if (isnew)
			{
				AddIndexEntry(writer, uuid);
			}
		}

		internal class DatabaseIdentityIDAndUUID
		{
			public int databaseIdentityID;

			public long uuid;

			public DatabaseIdentityIDAndUUID(int databaseIdentityID_, long uuid_)
			{
				databaseIdentityID = databaseIdentityID_;
				uuid = uuid_;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private UUIDFieldMetadata.DatabaseIdentityIDAndUUID ReadDatabaseIdentityIDAndUUID
			(ObjectContainerBase container, ClassMetadata classMetadata, Slot oldSlot, bool 
			checkClass)
		{
			if (DTrace.enabled)
			{
				DTrace.RereadOldUuid.LogLength(oldSlot.Address(), oldSlot.Length());
			}
			ByteArrayBuffer reader = container.DecryptedBufferByAddress(oldSlot.Address(), oldSlot
				.Length());
			if (checkClass)
			{
				ClassMetadata realClass = ClassMetadata.ReadClass(container, reader);
				if (realClass != classMetadata)
				{
					return null;
				}
			}
			if (classMetadata.SeekToField(container.Transaction, reader, this) == HandlerVersion
				.Invalid)
			{
				return null;
			}
			return new UUIDFieldMetadata.DatabaseIdentityIDAndUUID(reader.ReadInt(), reader.ReadLong
				());
		}

		public override void Delete(DeleteContextImpl context, bool isUpdate)
		{
			if (isUpdate)
			{
				context.Seek(context.Offset() + LinkLength(context));
				return;
			}
			context.Seek(context.Offset() + Const4.IntLength);
			long longPart = context.ReadLong();
			if (longPart > 0)
			{
				if (context.Container().MaintainsIndices())
				{
					RemoveIndexEntry(context.Transaction(), context.ObjectId(), longPart);
				}
			}
		}

		public override bool HasIndex()
		{
			return true;
		}

		public override BTree GetIndex(Transaction transaction)
		{
			EnsureIndex(transaction);
			return base.GetIndex(transaction);
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		protected override void RebuildIndexForObject(LocalObjectContainer container, ClassMetadata
			 classMetadata, int objectId)
		{
			Slot slot = container.SystemTransaction().IdSystem().CurrentSlot(objectId);
			UUIDFieldMetadata.DatabaseIdentityIDAndUUID data = ReadDatabaseIdentityIDAndUUID(
				container, classMetadata, slot, true);
			if (null == data)
			{
				return;
			}
			AddIndexEntry(container.LocalSystemTransaction(), objectId, data.uuid);
		}

		private void EnsureIndex(Transaction transaction)
		{
			if (null == transaction)
			{
				throw new ArgumentNullException();
			}
			if (null != base.GetIndex(transaction))
			{
				return;
			}
			LocalObjectContainer file = ((LocalObjectContainer)transaction.Container());
			SystemData sd = file.SystemData();
			if (sd == null)
			{
				// too early, in new file, try again later.
				return;
			}
			InitIndex(transaction, sd.UuidIndexId());
			if (sd.UuidIndexId() == 0)
			{
				sd.UuidIndexId(base.GetIndex(transaction).GetID());
				file.GetFileHeader().WriteVariablePart(file);
			}
		}

		internal override void Instantiate1(ObjectReferenceContext context)
		{
			int dbID = context.ReadInt();
			Transaction trans = context.Transaction();
			ObjectContainerBase container = trans.Container();
			container.ShowInternalClasses(true);
			try
			{
				Db4oDatabase db = (Db4oDatabase)container.GetByID2(trans, dbID);
				if (db != null && db.i_signature == null)
				{
					container.Activate(trans, db, new FixedActivationDepth(2));
				}
				VirtualAttributes va = context.ObjectReference().VirtualAttributes();
				va.i_database = db;
				va.i_uuid = context.ReadLong();
			}
			finally
			{
				container.ShowInternalClasses(false);
			}
		}

		public override int LinkLength(IHandlerVersionContext context)
		{
			return Const4.LongLength + Const4.IdLength;
		}

		internal override void Marshall(Transaction trans, ObjectReference @ref, IWriteBuffer
			 buffer, bool isMigrating, bool isNew)
		{
			VirtualAttributes attr = @ref.VirtualAttributes();
			ObjectContainerBase container = trans.Container();
			bool doAddIndexEntry = isNew && container.MaintainsIndices();
			int dbID = 0;
			bool linkToDatabase = (attr != null && attr.i_database == null) ? true : !isMigrating;
			if (linkToDatabase)
			{
				Db4oDatabase db = ((IInternalObjectContainer)container).Identity();
				if (db == null)
				{
					// can happen on early classes like Metaxxx, no problem
					attr = null;
				}
				else
				{
					if (attr.i_database == null)
					{
						attr.i_database = db;
						// TODO: Should be check for ! client instead of instanceof
						if (container is LocalObjectContainer)
						{
							attr.i_uuid = container.GenerateTimeStampId();
							doAddIndexEntry = true;
						}
					}
					db = attr.i_database;
					if (db != null)
					{
						dbID = db.GetID(trans);
					}
				}
			}
			else
			{
				if (attr != null)
				{
					dbID = attr.i_database.GetID(trans);
				}
			}
			buffer.WriteInt(dbID);
			if (attr == null)
			{
				buffer.WriteLong(0);
				return;
			}
			buffer.WriteLong(attr.i_uuid);
			if (doAddIndexEntry)
			{
				AddIndexEntry(trans, @ref.GetID(), attr.i_uuid);
			}
		}

		internal override void MarshallIgnore(IWriteBuffer buffer)
		{
			buffer.WriteInt(0);
			buffer.WriteLong(0);
		}

		public HardObjectReference GetHardObjectReferenceBySignature(Transaction transaction
			, long longPart, byte[] signature)
		{
			IBTreeRange range = Search(transaction, longPart);
			IEnumerator keys = range.Keys();
			while (keys.MoveNext())
			{
				IFieldIndexKey current = (IFieldIndexKey)keys.Current;
				HardObjectReference hardRef = GetHardObjectReferenceById(transaction, current.ParentID
					(), signature);
				if (null != hardRef)
				{
					return hardRef;
				}
			}
			return HardObjectReference.Invalid;
		}

		protected HardObjectReference GetHardObjectReferenceById(Transaction transaction, 
			int parentId, byte[] signature)
		{
			HardObjectReference hardRef = transaction.Container().GetHardObjectReferenceById(
				transaction, parentId);
			if (hardRef._reference == null)
			{
				return null;
			}
			VirtualAttributes vad = hardRef._reference.VirtualAttributes(transaction, false);
			if (!Arrays4.Equals(signature, vad.i_database.i_signature))
			{
				return null;
			}
			return hardRef;
		}

		public override void DefragAspect(IDefragmentContext context)
		{
			// database id
			context.CopyID();
			// uuid
			context.IncrementOffset(Const4.LongLength);
		}
	}
}
