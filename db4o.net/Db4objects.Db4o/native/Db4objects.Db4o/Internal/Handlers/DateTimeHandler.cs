/* Copyright (C) 2004 - 2007   Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Marshall;
using Sharpen;

namespace Db4objects.Db4o.Internal.Handlers
{
	public class DateTimeHandler : StructHandler
	{
		public override Object DefaultValue()
		{
			return DateTime.MinValue;
		}

		public override Object Read(byte[] bytes, int offset)
		{
			long ticks = 0;
			for (int i = 0; i < 8; i++)
			{
				ticks = (ticks << 8) + (bytes[offset++] & 255);
			}
		    return ReadKind(new DateTime(ticks), bytes, offset);
		}

	    protected virtual DateTime ReadKind (DateTime dateTime, byte[] bytes, int offset)
	    {
            int kind  = 0;
            for (int i = 0; i < 4; i++)
            {
                kind = (kind << 8) + (bytes[offset++] & 255);
            }
            return DateTime.SpecifyKind(dateTime, (DateTimeKind)kind);
	    }

	    public override int TypeID()
		{
			return 25;
		}

		public override void Write(object obj, byte[] bytes, int offset)
		{
			long ticks = ((DateTime)obj).Ticks;
			for (int i = 0; i < 8; i++)
			{
				bytes[offset++] = (byte)(int)(ticks >> (7 - i) * 8);
			}
		    WriteKind((DateTime)obj, bytes, offset);
		}

	    protected virtual void WriteKind(DateTime dateTime, byte[] bytes, int offset)
	    {
	        int kind = (int) dateTime.Kind;
            for (int i = 0; i < 4; i++)
            {
                bytes[offset++] = (byte)(int)(kind >> (3 - i) * 8);
            }
	    }

	    public override object Read(IReadContext context)
		{	
			long ticks = context.ReadLong();
            DateTime dateTime = new DateTime(ticks);
		    return ReadKind(context, dateTime);
		}

	    protected virtual object ReadKind(IReadContext context, DateTime dateTime)
	    {
	        DateTimeKind kind = (DateTimeKind) context.ReadInt();
	        return DateTime.SpecifyKind(dateTime, kind);
	    }

	    public override void Write(IWriteContext context, object obj)
		{
	        DateTime dateTime = (DateTime)obj;
	        long ticks = dateTime.Ticks;
			context.WriteLong(ticks);
	        WriteKind(context, dateTime);
		}

	    protected virtual void WriteKind(IWriteContext context, DateTime dateTime)
	    {
	        context.WriteInt((int) dateTime.Kind);
	    }

	    public override IPreparedComparison InternalPrepareComparison(object obj)
        {
            return new PreparedComparisonFor<DateTime>(((DateTime)obj));
        }


	}
}
