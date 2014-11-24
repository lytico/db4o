/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	/// <exclude></exclude>
	public class VersionNumberToCommitTimestamp_8_0 : Conversion
	{
		public const int Version = 12;

		private VersionFieldMetadata versionFieldMetadata;

		public override void Convert(ConversionStage.SystemUpStage stage)
		{
			LocalObjectContainer container = stage.File();
			if (!container.Config().GenerateCommitTimestamps().DefiniteYes())
			{
				return;
			}
			container.ClassCollection().WriteAllClasses();
			BuildCommitTimestampIndex(container);
			container.SystemTransaction().Commit();
		}

		private void BuildCommitTimestampIndex(LocalObjectContainer container)
		{
			versionFieldMetadata = container.Handlers.Indexes()._version;
			ClassMetadataIterator i = container.ClassCollection().Iterator();
			while (i.MoveNext())
			{
				ClassMetadata clazz = i.CurrentClass();
				if (clazz.HasVersionField() && !clazz.IsStruct())
				{
					RebuildIndexForClass(container, clazz);
				}
			}
		}

		public virtual bool RebuildIndexForClass(LocalObjectContainer container, ClassMetadata
			 classMetadata)
		{
			long[] ids = classMetadata.GetIDs();
			for (int i = 0; i < ids.Length; i++)
			{
				RebuildIndexForObject(container, (int)ids[i]);
			}
			return ids.Length > 0;
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		protected virtual void RebuildIndexForObject(LocalObjectContainer container, int 
			objectId)
		{
			StatefulBuffer writer = container.ReadStatefulBufferById(container.SystemTransaction
				(), objectId);
			if (writer != null)
			{
				RebuildIndexForWriter(container, writer, objectId);
			}
		}

		protected virtual void RebuildIndexForWriter(LocalObjectContainer container, StatefulBuffer
			 buffer, int objectId)
		{
			ObjectHeader objectHeader = new ObjectHeader(container, buffer);
			ObjectIdContextImpl context = new ObjectIdContextImpl(container.SystemTransaction
				(), buffer, objectHeader, objectId);
			ClassMetadata classMetadata = context.ClassMetadata();
			if (classMetadata.IsStruct())
			{
				// We don't keep version information for structs.
				return;
			}
			if (classMetadata.SeekToField(container.SystemTransaction(), buffer, versionFieldMetadata
				) != HandlerVersion.Invalid)
			{
				long version = ((long)versionFieldMetadata.Read(context));
				if (version != 0)
				{
					LocalTransaction t = (LocalTransaction)container.SystemTransaction();
					t.CommitTimestampSupport().Put(container.SystemTransaction(), objectId, version);
				}
			}
		}
	}
}
