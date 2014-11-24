/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class FieldMarshaller0 : AbstractFieldMarshaller
	{
		public override int MarshalledLength(ObjectContainerBase stream, ClassAspect aspect
			)
		{
			int len = stream.StringIO().ShortLength(aspect.GetName());
			if (aspect is FieldMetadata)
			{
				FieldMetadata field = (FieldMetadata)aspect;
				if (field.NeedsArrayAndPrimitiveInfo())
				{
					len += 1;
				}
				if (!(field is VirtualFieldMetadata))
				{
					len += Const4.IdLength;
				}
			}
			return len;
		}

		protected override RawFieldSpec ReadSpec(AspectType aspectType, ObjectContainerBase
			 stream, ByteArrayBuffer reader)
		{
			string name = StringHandler.ReadStringNoDebug(stream.Transaction.Context(), reader
				);
			if (!aspectType.IsFieldMetadata())
			{
				return new RawFieldSpec(aspectType, name);
			}
			if (name.IndexOf(Const4.VirtualFieldPrefix) == 0)
			{
				if (stream._handlers.VirtualFieldByName(name) != null)
				{
					return new RawFieldSpec(aspectType, name);
				}
			}
			int fieldTypeID = reader.ReadInt();
			byte attribs = reader.ReadByte();
			return new RawFieldSpec(aspectType, name, fieldTypeID, attribs);
		}

		public sealed override FieldMetadata Read(ObjectContainerBase stream, ClassMetadata
			 containingClass, ByteArrayBuffer reader)
		{
			RawFieldSpec spec = ReadSpec(stream, reader);
			return FromSpec(spec, stream, containingClass);
		}

		protected virtual FieldMetadata FromSpec(RawFieldSpec spec, ObjectContainerBase stream
			, ClassMetadata containingClass)
		{
			if (spec == null)
			{
				return null;
			}
			string name = spec.Name();
			if (spec.IsVirtualField())
			{
				return stream._handlers.VirtualFieldByName(name);
			}
			if (spec.IsTranslator())
			{
				return new TranslatedAspect(containingClass, name);
			}
			if (spec.IsField())
			{
				return new FieldMetadata(containingClass, name, spec.FieldTypeID(), spec.IsPrimitive
					(), spec.IsArray(), spec.IsNArray());
			}
			return new UnknownTypeHandlerAspect(containingClass, name);
		}

		public override void Write(Transaction trans, ClassMetadata clazz, ClassAspect aspect
			, ByteArrayBuffer writer)
		{
			writer.WriteShortString(trans, aspect.GetName());
			if (!(aspect is FieldMetadata))
			{
				return;
			}
			FieldMetadata field = (FieldMetadata)aspect;
			field.Alive();
			if (field.IsVirtual())
			{
				return;
			}
			ITypeHandler4 handler = field.GetHandler();
			if (handler is StandardReferenceTypeHandler)
			{
				// TODO: ensure there is a test case, to make this happen 
				if (((StandardReferenceTypeHandler)handler).ClassMetadata().GetID() == 0)
				{
					trans.Container().NeedsUpdate(clazz);
				}
			}
			writer.WriteInt(field.FieldTypeID());
			BitMap4 bitmap = new BitMap4(3);
			bitmap.Set(0, field.IsPrimitive());
			bitmap.Set(1, Handlers4.HandlesArray(handler));
			bitmap.Set(2, Handlers4.HandlesMultidimensionalArray(handler));
			// keep the order
			writer.WriteByte(bitmap.GetByte(0));
		}

		public override void Defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO
			 sio, DefragmentContextImpl context)
		{
			context.IncrementStringOffset(sio);
			if (!(aspect is FieldMetadata))
			{
				return;
			}
			if (((FieldMetadata)aspect).IsVirtual())
			{
				return;
			}
			// handler ID
			context.CopyID();
			// skip primitive/array/narray attributes
			context.IncrementOffset(1);
		}
	}
}
