/* Copyright (C) 2004 - 2007   Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Marshall;


namespace Db4objects.Db4o.Internal.Handlers
{
	public class UShortHandler : IntegralTypeHandler
	{
        public override Object DefaultValue()
		{
            return (ushort)0;
        }
      
        public override Object Read(byte[] bytes, int offset)
		{
            offset += 1;
            return (ushort) (bytes[offset] & 255 | (bytes[--offset] & 255) << 8);
        }
      
        public override int TypeID()
		{
            return 24;
        }
      
        public override void Write(Object obj, byte[] bytes, int offset)
		{
            ushort us = (ushort)obj;
            offset += 2;
            bytes[--offset] = (byte)us;
            bytes[--offset] = (byte)(us >>= 8);
        }

        public override object Read(IReadContext context)
        {
            byte[] bytes = new byte[2];
            context.ReadBytes(bytes);
            return (ushort)(
                     bytes[1] & 255 |
                    (bytes[0] & 255) << 8
                );
        }

        public override void Write(IWriteContext context, object obj)
        {
            ushort us = (ushort)obj;
            context.WriteBytes(
                new byte[] { 
                    (byte)(us>>8),
                    (byte)us,
                });
        }

        public override IPreparedComparison InternalPrepareComparison(object obj)
        {
            return new PreparedComparisonFor<ushort>(((ushort)obj));
        }

		public override object Coerce(Db4o.Reflect.IReflectClass claxx, object value)
		{
			return Coercion4.ToUShort(value);
		}
    }
}
