/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ClassMarshaller2 : ClassMarshaller
	{
		protected override void ReadIndex(ObjectContainerBase stream, ClassMetadata clazz
			, ByteArrayBuffer reader)
		{
			int indexID = reader.ReadInt();
			if (indexID == 0)
			{
				return;
			}
			clazz.Index().Read(stream, indexID);
		}

		protected override int IndexIDForWriting(int indexID)
		{
			return indexID;
		}
	}
}
