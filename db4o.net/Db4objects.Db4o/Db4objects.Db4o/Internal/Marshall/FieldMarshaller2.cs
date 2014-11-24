/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class FieldMarshaller2 : FieldMarshaller1
	{
		private const int AspectTypeTagLength = 1;

		public override int MarshalledLength(ObjectContainerBase stream, ClassAspect aspect
			)
		{
			return base.MarshalledLength(stream, aspect) + AspectTypeTagLength;
		}

		protected override RawFieldSpec ReadSpec(AspectType aspectType, ObjectContainerBase
			 stream, ByteArrayBuffer reader)
		{
			return base.ReadSpec(AspectType.ForByte(reader.ReadByte()), stream, reader);
		}

		public override void Write(Transaction trans, ClassMetadata clazz, ClassAspect aspect
			, ByteArrayBuffer writer)
		{
			writer.WriteByte(aspect.AspectType()._id);
			base.Write(trans, clazz, aspect, writer);
		}

		public override void Defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO
			 sio, DefragmentContextImpl context)
		{
			context.ReadByte();
			base.Defrag(classMetadata, aspect, sio, context);
		}
	}
}
