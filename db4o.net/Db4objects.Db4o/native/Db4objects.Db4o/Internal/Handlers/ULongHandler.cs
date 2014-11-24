/* Copyright (C) 2004 - 2007   Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
    public class ULongHandler : IntegralTypeHandler 
	{

        public override Object DefaultValue()
		{
            return (ulong)0;
        }
      
        public override void Write(object obj, byte[] bytes, int offset)
		{
            ulong ul = (ulong)obj;
            for (int i = 0; i < 8; i++)
			{
                bytes[offset++] = (byte)(int)(ul >> (7 - i) * 8);
            }
        }

        public override int TypeID()
		{
            return 23;
        }

        public override Object Read(byte[] bytes, int offset)
		{
            ulong ul = 0;
            for (int i = 0; i < 8; i++) 
			{
                ul = (ul << 8) + (ulong)(bytes[offset++] & 255);
            }
            return ul;
        }

        public override object Read(IReadContext context)
        {
            byte[] bytes = new byte[8];
            context.ReadBytes(bytes);
            return (ulong)(
                     (ulong)bytes[7] & 255 |
                    (ulong)(bytes[6] & 255) << 8  |
                    (ulong)(bytes[5] & 255) << 16 |
                    (ulong)(bytes[4] & 255) << 24 |
                    (ulong)(bytes[3] & 255) << 32 |
                    (ulong)(bytes[2] & 255) << 40 |
                    (ulong)(bytes[1] & 255) << 48 |
                    (ulong)(bytes[0] & 255) << 56 
                );
        }

        public override void Write(IWriteContext context, object obj)
        {
            ulong ui = (ulong)obj;
            context.WriteBytes(
                new byte[] { 
                    (byte)(ui>>56),
                    (byte)(ui>>48),
                    (byte)(ui>>40),
                    (byte)(ui>>32),
                    (byte)(ui>>24),
                    (byte)(ui>>16),
                    (byte)(ui>>8),
                    (byte)ui,
                });
        }

        public override IPreparedComparison InternalPrepareComparison(object obj)
        {
            return new PreparedComparisonFor<ulong>(((ulong)obj));
        }

		public override object Coerce(Db4o.Reflect.IReflectClass claxx, object value)
		{
			return Coercion4.ToULong(value);
		}
    }
}
