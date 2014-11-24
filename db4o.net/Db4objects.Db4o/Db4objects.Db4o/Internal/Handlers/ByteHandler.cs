/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers
{
	public sealed class ByteHandler : PrimitiveHandler
	{
		internal const int Length = 1 + Const4.AddedLength;

		private static readonly byte Defaultvalue = (byte)0;

		public override object Coerce(IReflectClass claxx, object obj)
		{
			return Coercion4.ToByte(obj);
		}

		public override object DefaultValue()
		{
			return Defaultvalue;
		}

		public override int LinkLength()
		{
			return Length;
		}

		public override Type PrimitiveJavaClass()
		{
			return typeof(byte);
		}

		internal override object Read1(ByteArrayBuffer a_bytes)
		{
			byte ret = a_bytes.ReadByte();
			return ret;
		}

		public override void Write(object a_object, ByteArrayBuffer a_bytes)
		{
			a_bytes.WriteByte(((byte)a_object));
		}

		public override object Read(IReadContext context)
		{
			byte byteValue = context.ReadByte();
			return byteValue;
		}

		public override void Write(IWriteContext context, object obj)
		{
			context.WriteByte(((byte)obj));
		}

		public override IPreparedComparison InternalPrepareComparison(object source)
		{
			byte sourceByte = ((byte)source);
			return new _IPreparedComparison_82(sourceByte);
		}

		private sealed class _IPreparedComparison_82 : IPreparedComparison
		{
			public _IPreparedComparison_82(byte sourceByte)
			{
				this.sourceByte = sourceByte;
			}

			public int CompareTo(object target)
			{
				if (target == null)
				{
					return 1;
				}
				byte targetByte = ((byte)target);
				return sourceByte == targetByte ? 0 : (sourceByte < targetByte ? -1 : 1);
			}

			private readonly byte sourceByte;
		}
	}
}
