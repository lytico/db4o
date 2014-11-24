/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Sharpen.IO;

namespace Db4objects.Db4o.Foundation
{
	public sealed class PrimitiveCodec
	{
		public const int IntLength = 4;

		public const int LongLength = 8;

		public static int ReadInt(byte[] buffer, int offset)
		{
			offset += 3;
			return (buffer[offset] & 255) | (buffer[--offset] & 255) << 8 | (buffer[--offset]
				 & 255) << 16 | buffer[--offset] << 24;
		}

		public static int ReadInt(ByteArrayInputStream @in)
		{
			return (@in.Read() << 24) | ((@in.Read() & 255) << 16) | ((@in.Read() & 255) << 8
				) | (@in.Read() & 255);
		}

		public static void WriteInt(byte[] buffer, int offset, int val)
		{
			offset += 3;
			buffer[offset] = (byte)val;
			buffer[--offset] = (byte)(val >>= 8);
			buffer[--offset] = (byte)(val >>= 8);
			buffer[--offset] = (byte)(val >> 8);
		}

		public static void WriteInt(ByteArrayOutputStream @out, int val)
		{
			@out.Write((byte)(val >> 24));
			@out.Write((byte)(val >> 16));
			@out.Write((byte)(val >> 8));
			@out.Write((byte)val);
		}

		public static void WriteLong(byte[] buffer, long val)
		{
			WriteLong(buffer, 0, val);
		}

		public static void WriteLong(byte[] buffer, int offset, long val)
		{
			for (int i = 0; i < LongLength; i++)
			{
				buffer[offset++] = (byte)(val >> ((7 - i) * 8));
			}
		}

		public static void WriteLong(ByteArrayOutputStream @out, long val)
		{
			for (int i = 0; i < LongLength; i++)
			{
				@out.Write((byte)(val >> ((7 - i) * 8)));
			}
		}

		public static long ReadLong(byte[] buffer, int offset)
		{
			long ret = 0;
			for (int i = 0; i < LongLength; i++)
			{
				ret = (ret << 8) + (buffer[offset++] & unchecked((int)(0xff)));
			}
			return ret;
		}

		public static long ReadLong(ByteArrayInputStream @in)
		{
			long ret = 0;
			for (int i = 0; i < LongLength; i++)
			{
				ret = (ret << 8) + ((byte)@in.Read() & unchecked((int)(0xff)));
			}
			return ret;
		}
	}
}
