/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Convert.Conversions;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ClassMarshaller0 : ClassMarshaller
	{
		protected override void ReadIndex(ObjectContainerBase stream, ClassMetadata clazz
			, ByteArrayBuffer reader)
		{
			int indexID = reader.ReadInt();
			if (!stream.MaintainsIndices() || !(stream is LocalObjectContainer))
			{
				return;
			}
			if (Btree(clazz) != null)
			{
				return;
			}
			clazz.Index().Read(stream, ValidIndexId(indexID));
			if (IsOldClassIndex(indexID))
			{
				new ClassIndexesToBTrees_5_5().Convert((LocalObjectContainer)stream, indexID, Btree
					(clazz));
				stream.SetDirtyInSystemTransaction(clazz);
			}
		}

		private BTree Btree(ClassMetadata clazz)
		{
			return BTreeClassIndexStrategy.Btree(clazz);
		}

		private int ValidIndexId(int indexID)
		{
			return IsOldClassIndex(indexID) ? 0 : -indexID;
		}

		private bool IsOldClassIndex(int indexID)
		{
			return indexID > 0;
		}

		protected override int IndexIDForWriting(int indexID)
		{
			return indexID;
		}
	}
}
