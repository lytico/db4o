/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public abstract class AbstractFieldMarshaller : IFieldMarshaller
	{
		protected abstract RawFieldSpec ReadSpec(AspectType aspectType, ObjectContainerBase
			 stream, ByteArrayBuffer reader);

		public virtual RawFieldSpec ReadSpec(ObjectContainerBase stream, ByteArrayBuffer 
			reader)
		{
			return ReadSpec(AspectType.Field, stream, reader);
		}

		public abstract void Defrag(ClassMetadata arg1, ClassAspect arg2, LatinStringIO arg3
			, DefragmentContextImpl arg4);

		public abstract int MarshalledLength(ObjectContainerBase arg1, ClassAspect arg2);

		public abstract FieldMetadata Read(ObjectContainerBase arg1, ClassMetadata arg2, 
			ByteArrayBuffer arg3);

		public abstract void Write(Transaction arg1, ClassMetadata arg2, ClassAspect arg3
			, ByteArrayBuffer arg4);
	}
}
