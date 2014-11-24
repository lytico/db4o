/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ClassMarshaller1 : ClassMarshaller
	{
		protected override void ReadIndex(ObjectContainerBase stream, ClassMetadata clazz
			, ByteArrayBuffer reader)
		{
			int indexID = reader.ReadInt();
			clazz.Index().Read(stream, -indexID);
		}

		protected override int IndexIDForWriting(int indexID)
		{
			return -indexID;
		}
	}
}
