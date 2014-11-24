/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public abstract class ClassMarshaller
	{
		public MarshallerFamily _family;

		public virtual RawClassSpec ReadSpec(Transaction trans, ByteArrayBuffer reader)
		{
			byte[] nameBytes = ReadName(trans, reader);
			string className = trans.Container().StringIO().Read(nameBytes);
			ReadMetaClassID(reader);
			// skip
			int ancestorID = reader.ReadInt();
			reader.IncrementOffset(Const4.IntLength);
			// index ID
			int numFields = reader.ReadInt();
			return new RawClassSpec(className, ancestorID, numFields);
		}

		public virtual void Write(Transaction trans, ClassMetadata clazz, ByteArrayBuffer
			 writer)
		{
			writer.WriteShortString(trans, clazz.NameToWrite());
			int intFormerlyKnownAsMetaClassID = 0;
			writer.WriteInt(intFormerlyKnownAsMetaClassID);
			writer.WriteIDOf(trans, clazz._ancestor);
			WriteIndex(trans, clazz, writer);
			writer.WriteInt(clazz.DeclaredAspectCount());
			clazz.TraverseDeclaredAspects(new _IProcedure4_39(this, trans, clazz, writer));
		}

		private sealed class _IProcedure4_39 : IProcedure4
		{
			public _IProcedure4_39(ClassMarshaller _enclosing, Transaction trans, ClassMetadata
				 clazz, ByteArrayBuffer writer)
			{
				this._enclosing = _enclosing;
				this.trans = trans;
				this.clazz = clazz;
				this.writer = writer;
			}

			public void Apply(object arg)
			{
				this._enclosing._family._field.Write(trans, clazz, (ClassAspect)arg, writer);
			}

			private readonly ClassMarshaller _enclosing;

			private readonly Transaction trans;

			private readonly ClassMetadata clazz;

			private readonly ByteArrayBuffer writer;
		}

		protected virtual void WriteIndex(Transaction trans, ClassMetadata clazz, ByteArrayBuffer
			 writer)
		{
			int indexID = clazz.Index().Write(trans);
			writer.WriteInt(IndexIDForWriting(indexID));
		}

		protected abstract int IndexIDForWriting(int indexID);

		public byte[] ReadName(Transaction trans, ByteArrayBuffer reader)
		{
			return ReadName(trans.Container().StringIO(), reader);
		}

		public int ReadMetaClassID(ByteArrayBuffer reader)
		{
			return reader.ReadInt();
		}

		private byte[] ReadName(LatinStringIO sio, ByteArrayBuffer reader)
		{
			byte[] nameBytes = sio.Bytes(reader);
			reader.IncrementOffset(nameBytes.Length);
			nameBytes = Platform4.UpdateClassName(nameBytes);
			return nameBytes;
		}

		public void Read(ObjectContainerBase stream, ClassMetadata clazz, ByteArrayBuffer
			 reader)
		{
			clazz.SetAncestor(stream.ClassMetadataForID(reader.ReadInt()));
			//        if(clazz.callConstructor()){
			//            // The logic further down checks the ancestor YapClass, whether
			//            // or not it is allowed, not to call constructors. The ancestor
			//            // YapClass may possibly have not been loaded yet.
			//            clazz.createConstructor(true);
			//        }
			clazz.CheckType();
			ReadIndex(stream, clazz, reader);
			clazz._aspects = ReadAspects(stream, reader, clazz);
		}

		protected abstract void ReadIndex(ObjectContainerBase stream, ClassMetadata clazz
			, ByteArrayBuffer reader);

		private ClassAspect[] ReadAspects(ObjectContainerBase stream, ByteArrayBuffer reader
			, ClassMetadata clazz)
		{
			ClassAspect[] aspects = new ClassAspect[reader.ReadInt()];
			for (int i = 0; i < aspects.Length; i++)
			{
				aspects[i] = _family._field.Read(stream, clazz, reader);
				aspects[i].SetHandle(i);
			}
			return aspects;
		}

		public virtual int MarshalledLength(ObjectContainerBase stream, ClassMetadata clazz
			)
		{
			IntByRef len = new IntByRef(stream.StringIO().ShortLength(clazz.NameToWrite()) + 
				Const4.ObjectLength + (Const4.IntLength * 2) + (Const4.IdLength));
			len.value += clazz.Index().OwnLength();
			clazz.TraverseDeclaredAspects(new _IProcedure4_108(this, len, stream));
			return len.value;
		}

		private sealed class _IProcedure4_108 : IProcedure4
		{
			public _IProcedure4_108(ClassMarshaller _enclosing, IntByRef len, ObjectContainerBase
				 stream)
			{
				this._enclosing = _enclosing;
				this.len = len;
				this.stream = stream;
			}

			public void Apply(object arg)
			{
				len.value += this._enclosing._family._field.MarshalledLength(stream, (ClassAspect
					)arg);
			}

			private readonly ClassMarshaller _enclosing;

			private readonly IntByRef len;

			private readonly ObjectContainerBase stream;
		}

		public virtual void Defrag(ClassMetadata classMetadata, LatinStringIO sio, DefragmentContextImpl
			 context, int classIndexID)
		{
			ReadName(sio, context.SourceBuffer());
			ReadName(sio, context.TargetBuffer());
			int metaClassID = 0;
			context.WriteInt(metaClassID);
			// ancestor ID
			context.CopyID();
			context.WriteInt((classMetadata.HasClassIndex() ? IndexIDForWriting(classIndexID)
				 : 0));
			int aspectCount = context.ReadInt();
			if (aspectCount > classMetadata.DeclaredAspectCount())
			{
				throw new InvalidOperationException();
			}
			IntByRef processedAspectCount = new IntByRef(0);
			classMetadata.TraverseDeclaredAspects(new _IProcedure4_136(this, processedAspectCount
				, aspectCount, classMetadata, sio, context));
		}

		private sealed class _IProcedure4_136 : IProcedure4
		{
			public _IProcedure4_136(ClassMarshaller _enclosing, IntByRef processedAspectCount
				, int aspectCount, ClassMetadata classMetadata, LatinStringIO sio, DefragmentContextImpl
				 context)
			{
				this._enclosing = _enclosing;
				this.processedAspectCount = processedAspectCount;
				this.aspectCount = aspectCount;
				this.classMetadata = classMetadata;
				this.sio = sio;
				this.context = context;
			}

			public void Apply(object arg)
			{
				if (processedAspectCount.value >= aspectCount)
				{
					return;
				}
				ClassAspect aspect = (ClassAspect)arg;
				this._enclosing._family._field.Defrag(classMetadata, aspect, sio, context);
				processedAspectCount.value++;
			}

			private readonly ClassMarshaller _enclosing;

			private readonly IntByRef processedAspectCount;

			private readonly int aspectCount;

			private readonly ClassMetadata classMetadata;

			private readonly LatinStringIO sio;

			private readonly DefragmentContextImpl context;
		}
	}
}
