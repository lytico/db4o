/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public sealed class BooleanHandler : PrimitiveHandler
	{
		internal const int Length = 1 + Const4.AddedLength;

		private const byte True = (byte)'T';

		private const byte False = (byte)'F';

		private const byte Null = (byte)'N';

		private static readonly bool Defaultvalue = false;

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
			return typeof(bool);
		}

		internal override object Read1(ByteArrayBuffer a_bytes)
		{
			byte ret = a_bytes.ReadByte();
			if (ret == True)
			{
				return true;
			}
			if (ret == False)
			{
				return false;
			}
			return null;
		}

		public override void Write(object obj, ByteArrayBuffer buffer)
		{
			buffer.WriteByte(GetEncodedByteValue(obj));
		}

		private byte GetEncodedByteValue(object obj)
		{
			if (obj == null)
			{
				return Null;
			}
			if (((bool)obj))
			{
				return True;
			}
			return False;
		}

		public override object Read(IReadContext context)
		{
			byte ret = context.ReadByte();
			if (ret == True)
			{
				return true;
			}
			if (ret == False)
			{
				return false;
			}
			return null;
		}

		public override void Write(IWriteContext context, object obj)
		{
			context.WriteByte(GetEncodedByteValue(obj));
		}

		public override object NullRepresentationInUntypedArrays()
		{
			return null;
		}

		public override IPreparedComparison InternalPrepareComparison(object source)
		{
			bool sourceBoolean = ((bool)source);
			return new _IPreparedComparison_111(sourceBoolean);
		}

		private sealed class _IPreparedComparison_111 : IPreparedComparison
		{
			public _IPreparedComparison_111(bool sourceBoolean)
			{
				this.sourceBoolean = sourceBoolean;
			}

			public int CompareTo(object target)
			{
				if (target == null)
				{
					return 1;
				}
				bool targetBoolean = ((bool)target);
				return sourceBoolean == targetBoolean ? 0 : (sourceBoolean ? 1 : -1);
			}

			private readonly bool sourceBoolean;
		}
	}
}
