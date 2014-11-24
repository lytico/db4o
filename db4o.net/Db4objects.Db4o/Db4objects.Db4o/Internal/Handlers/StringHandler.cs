/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class StringHandler : IValueTypeHandler, IIndexableTypeHandler, IBuiltinTypeHandler
		, IVariableLengthTypeHandler, IQueryableTypeHandler
	{
		private IReflectClass _classReflector;

		public virtual IReflectClass ClassReflector()
		{
			return _classReflector;
		}

		public virtual void Delete(IDeleteContext context)
		{
		}

		// do nothing, we are in a slot indirection anyway, the 
		// buffer position does not need to be changed.
		internal virtual byte GetIdentifier()
		{
			return Const4.Yapstring;
		}

		public virtual bool DescendsIntoMembers()
		{
			return false;
		}

		public object IndexEntryToObject(IContext context, object indexEntry)
		{
			if (indexEntry is Slot)
			{
				Slot slot = (Slot)indexEntry;
				indexEntry = context.Transaction().Container().DecryptedBufferByAddress(slot.Address
					(), slot.Length());
			}
			return ReadStringNoDebug(context, (IReadBuffer)indexEntry);
		}

		/// <summary>This readIndexEntry method reads from the parent slot.</summary>
		/// <remarks>This readIndexEntry method reads from the parent slot.</remarks>
		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual object ReadIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer
			 buffer)
		{
			int payLoadOffSet = buffer.ReadInt();
			int length = buffer.ReadInt();
			if (payLoadOffSet == 0)
			{
				return null;
			}
			return buffer.ReadPayloadWriter(payLoadOffSet, length);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual object ReadIndexEntry(IObjectIdContext context)
		{
			int payLoadOffSet = context.ReadInt();
			int length = context.ReadInt();
			if (payLoadOffSet == 0)
			{
				return null;
			}
			return ((StatefulBuffer)context.Buffer()).ReadPayloadWriter(payLoadOffSet, length
				);
		}

		/// <summary>This readIndexEntry method reads from the actual index in the file.</summary>
		/// <remarks>This readIndexEntry method reads from the actual index in the file.</remarks>
		public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer reader)
		{
			Slot s = new Slot(reader.ReadInt(), reader.ReadInt());
			if (IsInvalidSlot(s))
			{
				return null;
			}
			return s;
		}

		private bool IsInvalidSlot(Slot slot)
		{
			return slot.IsNull();
		}

		public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer writer, object
			 entry)
		{
			if (entry == null)
			{
				writer.WriteInt(0);
				writer.WriteInt(0);
				return;
			}
			if (entry is StatefulBuffer)
			{
				StatefulBuffer entryAsWriter = (StatefulBuffer)entry;
				writer.WriteInt(entryAsWriter.GetAddress());
				writer.WriteInt(entryAsWriter.Length());
				return;
			}
			if (entry is Slot)
			{
				Slot s = (Slot)entry;
				writer.WriteInt(s.Address());
				writer.WriteInt(s.Length());
				return;
			}
			throw new ArgumentException();
		}

		public void WriteShort(Transaction trans, string str, ByteArrayBuffer buffer)
		{
			StringIo(trans.Container()).WriteLengthAndString(buffer, str);
		}

		internal virtual ByteArrayBuffer Val(object obj, IContext context)
		{
			if (obj is ByteArrayBuffer)
			{
				return (ByteArrayBuffer)obj;
			}
			ObjectContainerBase oc = context.Transaction().Container();
			if (obj is string)
			{
				return WriteToBuffer((IInternalObjectContainer)oc, (string)obj);
			}
			if (obj is Slot)
			{
				Slot s = (Slot)obj;
				return oc.DecryptedBufferByAddress(s.Address(), s.Length());
			}
			return null;
		}

		/// <summary>
		/// returns: -x for left is greater and +x for right is greater
		/// FIXME: The returned value is the wrong way around.
		/// </summary>
		/// <remarks>
		/// returns: -x for left is greater and +x for right is greater
		/// FIXME: The returned value is the wrong way around.
		/// TODO: You will need collators here for different languages.
		/// </remarks>
		internal int Compare(ByteArrayBuffer a_compare, ByteArrayBuffer a_with)
		{
			if (a_compare == null)
			{
				if (a_with == null)
				{
					return 0;
				}
				return -1;
			}
			if (a_with == null)
			{
				return 1;
			}
			return Compare(a_compare._buffer, a_with._buffer);
		}

		public static int Compare(byte[] compare, byte[] with)
		{
			int min = compare.Length < with.Length ? compare.Length : with.Length;
			int start = Const4.IntLength;
			for (int i = start; i < min; i++)
			{
				if (compare[i] != with[i])
				{
					return compare[i] - with[i];
				}
			}
			return compare.Length - with.Length;
		}

		public virtual void DefragIndexEntry(DefragmentContextImpl context)
		{
			context.CopyAddress();
			// length
			context.IncrementIntSize();
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			InternalWrite((IInternalObjectContainer)context.ObjectContainer(), context, (string
				)obj);
		}

		protected static void InternalWrite(IInternalObjectContainer objectContainer, IWriteBuffer
			 buffer, string str)
		{
			StringIo(objectContainer).WriteLengthAndString(buffer, str);
		}

		public static ByteArrayBuffer WriteToBuffer(IInternalObjectContainer container, string
			 str)
		{
			ByteArrayBuffer buffer = new ByteArrayBuffer(StringIo(container).Length(str));
			InternalWrite(container, buffer, str);
			return buffer;
		}

		protected static LatinStringIO StringIo(IContext context)
		{
			return StringIo((IInternalObjectContainer)context.ObjectContainer());
		}

		protected static LatinStringIO StringIo(IInternalObjectContainer objectContainer)
		{
			return objectContainer.Container.StringIO();
		}

		public static string ReadString(IContext context, IReadBuffer buffer)
		{
			string str = ReadStringNoDebug(context, buffer);
			return str;
		}

		public static string ReadStringNoDebug(IContext context, IReadBuffer buffer)
		{
			return Intern(context, StringIo(context).ReadLengthAndString(buffer));
		}

		protected static string Intern(IContext context, string str)
		{
			if (context.ObjectContainer().Ext().Configure().InternStrings())
			{
				return string.Intern(str);
			}
			return str;
		}

		public virtual object Read(IReadContext context)
		{
			return ReadString(context, context);
		}

		public virtual void Defragment(IDefragmentContext context)
		{
			context.IncrementOffset(LinkLength());
		}

		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			ByteArrayBuffer sourceBuffer = Val(obj, context);
			return new _IPreparedComparison_229(this, context, sourceBuffer);
		}

		private sealed class _IPreparedComparison_229 : IPreparedComparison
		{
			public _IPreparedComparison_229(StringHandler _enclosing, IContext context, ByteArrayBuffer
				 sourceBuffer)
			{
				this._enclosing = _enclosing;
				this.context = context;
				this.sourceBuffer = sourceBuffer;
			}

			public int CompareTo(object target)
			{
				ByteArrayBuffer targetBuffer = this._enclosing.Val(target, context);
				return this._enclosing.Compare(sourceBuffer, targetBuffer);
			}

			private readonly StringHandler _enclosing;

			private readonly IContext context;

			private readonly ByteArrayBuffer sourceBuffer;
		}

		public virtual int LinkLength()
		{
			return Const4.IndirectionLength;
		}

		public virtual void RegisterReflector(IReflector reflector)
		{
			_classReflector = reflector.ForClass(typeof(string));
		}
	}
}
