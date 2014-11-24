/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class LongHandler : PrimitiveHandler
	{
		private static readonly long Defaultvalue = System.Convert.ToInt64(0);

		public override object Coerce(IReflectClass claxx, object obj)
		{
			return Coercion4.ToLong(obj);
		}

		public override object DefaultValue()
		{
			return Defaultvalue;
		}

		public override Type PrimitiveJavaClass()
		{
			return typeof(long);
		}

		public override int LinkLength()
		{
			return Const4.LongLength;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public override object Read(MarshallerFamily mf, StatefulBuffer buffer, bool redirect
			)
		{
			return mf._primitive.ReadLong(buffer);
		}

		internal override object Read1(ByteArrayBuffer a_bytes)
		{
			return a_bytes.ReadLong();
		}

		public override void Write(object obj, ByteArrayBuffer buffer)
		{
			WriteLong(buffer, ((long)obj));
		}

		public static void WriteLong(IWriteBuffer buffer, long val)
		{
			if (Deploy.debug && Deploy.debugLong)
			{
				string l_s = "                                " + val;
				new LatinStringIO().Write(buffer, Sharpen.Runtime.Substring(l_s, l_s.Length - Const4
					.LongBytes));
			}
			else
			{
				for (int i = 0; i < Const4.LongBytes; i++)
				{
					buffer.WriteByte((byte)(val >> ((Const4.LongBytes - 1 - i) * 8)));
				}
			}
		}

		public static long ReadLong(IReadBuffer buffer)
		{
			long ret = 0;
			if (Deploy.debug && Deploy.debugLong)
			{
				ret = long.Parse(new LatinStringIO().Read(buffer, Const4.LongBytes).Trim());
			}
			else
			{
				for (int i = 0; i < Const4.LongBytes; i++)
				{
					ret = (ret << 8) + (buffer.ReadByte() & unchecked((int)(0xff)));
				}
			}
			return ret;
		}

		public override object Read(IReadContext context)
		{
			return context.ReadLong();
		}

		public override void Write(IWriteContext context, object obj)
		{
			context.WriteLong(((long)obj));
		}

		public static int Compare(long first, long second)
		{
			if (first == second)
			{
				return 0;
			}
			return first > second ? 1 : -1;
		}

		public override IPreparedComparison InternalPrepareComparison(object source)
		{
			long sourceLong = ((long)source);
			return new _IPreparedComparison_102(sourceLong);
		}

		private sealed class _IPreparedComparison_102 : IPreparedComparison
		{
			public _IPreparedComparison_102(long sourceLong)
			{
				this.sourceLong = sourceLong;
			}

			public int CompareTo(object target)
			{
				if (target == null)
				{
					return 1;
				}
				long targetLong = ((long)target);
				return LongHandler.Compare(sourceLong, targetLong);
			}

			private readonly long sourceLong;
		}
	}
}
