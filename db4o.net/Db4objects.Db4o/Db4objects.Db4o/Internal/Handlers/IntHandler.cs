/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class IntHandler : PrimitiveHandler
	{
		private static readonly int Defaultvalue = 0;

		public override object Coerce(IReflectClass claxx, object obj)
		{
			return Coercion4.ToInt(obj);
		}

		public override object DefaultValue()
		{
			return Defaultvalue;
		}

		public override Type PrimitiveJavaClass()
		{
			return typeof(int);
		}

		public override int LinkLength()
		{
			return Const4.IntLength;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public override object Read(MarshallerFamily mf, StatefulBuffer writer, bool redirect
			)
		{
			return mf._primitive.ReadInteger(writer);
		}

		internal override object Read1(ByteArrayBuffer a_bytes)
		{
			return a_bytes.ReadInt();
		}

		public override void Write(object obj, ByteArrayBuffer writer)
		{
			Write(((int)obj), writer);
		}

		public virtual void Write(int intValue, ByteArrayBuffer writer)
		{
			WriteInt(intValue, writer);
		}

		public static void WriteInt(int a_int, ByteArrayBuffer a_bytes)
		{
			a_bytes.WriteInt(a_int);
		}

		public override void DefragIndexEntry(DefragmentContextImpl context)
		{
			context.IncrementIntSize();
		}

		public override object Read(IReadContext context)
		{
			return context.ReadInt();
		}

		public override void Write(IWriteContext context, object obj)
		{
			context.WriteInt(((int)obj));
		}

		public override IPreparedComparison InternalPrepareComparison(object source)
		{
			return NewPrepareCompare(((int)source));
		}

		public virtual IPreparedComparison NewPrepareCompare(int i)
		{
			return new IntHandler.PreparedIntComparison(this, i);
		}

		public static int Compare(int first, int second)
		{
			if (first == second)
			{
				return 0;
			}
			return first > second ? 1 : -1;
		}

		public sealed class PreparedIntComparison : IPreparedComparison
		{
			private readonly int _sourceInt;

			public PreparedIntComparison(IntHandler _enclosing, int sourceInt)
			{
				this._enclosing = _enclosing;
				this._sourceInt = sourceInt;
			}

			public int CompareTo(object target)
			{
				if (target == null)
				{
					return 1;
				}
				int targetInt = ((int)target);
				return IntHandler.Compare(this._sourceInt, targetInt);
			}

			private readonly IntHandler _enclosing;
		}
	}
}
