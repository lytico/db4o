/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Internal.Encoding
{
    public class UTF8StringEncoding : BuiltInStringEncoding
    {
        public override byte[] Encode(String str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public override String Decode(byte[] bytes, int start, int length)
        {
            return System.Text.Encoding.UTF8.GetString(bytes, start, length);
        }

    }

}
