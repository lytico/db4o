/* Copyright (C) 2004 - 2009   Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
    class DateTimeHandler6 : DateTimeHandler
    {
        protected override object ReadKind(IReadContext context, DateTime dateTime)
        {
            return dateTime;
        }

        protected override void WriteKind(IWriteContext context, DateTime dateTime)
        {
            // do nothing
        }

        protected override DateTime ReadKind (DateTime dateTime, byte[] bytes, int offset)
        {
            return dateTime;
        }

        protected override void WriteKind(DateTime dateTime, byte[] bytes, int offset)
        {
            // do nothing
        }
		//public override int LinkLength()
		//{
		//    return base.LinkLength() - Const4.LongLength;
		//}
    }
}
