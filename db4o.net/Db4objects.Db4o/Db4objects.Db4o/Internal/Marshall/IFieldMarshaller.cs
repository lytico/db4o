/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public interface IFieldMarshaller
	{
		void Write(Transaction trans, ClassMetadata clazz, ClassAspect aspect, ByteArrayBuffer
			 writer);

		RawFieldSpec ReadSpec(ObjectContainerBase stream, ByteArrayBuffer reader);

		FieldMetadata Read(ObjectContainerBase stream, ClassMetadata clazz, ByteArrayBuffer
			 reader);

		int MarshalledLength(ObjectContainerBase stream, ClassAspect aspect);

		void Defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO sio, DefragmentContextImpl
			 context);
	}
}
