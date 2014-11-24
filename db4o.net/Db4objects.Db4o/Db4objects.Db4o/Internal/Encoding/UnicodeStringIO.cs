/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Encoding
{
	/// <exclude></exclude>
	public sealed class UnicodeStringIO : LatinStringIO
	{
		protected override int BytesPerChar()
		{
			return 2;
		}

		public override byte EncodingByte()
		{
			return BuiltInStringEncoding.EncodingByteForEncoding(new UnicodeStringEncoding());
		}

		public override int Length(string str)
		{
			return (str.Length * 2) + Const4.ObjectLength + Const4.IntLength;
		}

		public override string Read(IReadBuffer buffer, int length)
		{
			char[] chars = new char[length];
			for (int ii = 0; ii < length; ii++)
			{
				chars[ii] = (char)((buffer.ReadByte() & unchecked((int)(0xff))) | ((buffer.ReadByte
					() & unchecked((int)(0xff))) << 8));
			}
			return new string(chars, 0, length);
		}

		public override string Read(byte[] bytes)
		{
			int length = bytes.Length / 2;
			char[] chars = new char[length];
			int j = 0;
			for (int ii = 0; ii < length; ii++)
			{
				chars[ii] = (char)((bytes[j++] & unchecked((int)(0xff))) | ((bytes[j++] & unchecked(
					(int)(0xff))) << 8));
			}
			return new string(chars, 0, length);
		}

		public override int ShortLength(string str)
		{
			return (str.Length * 2) + Const4.IntLength;
		}

		public override void Write(IWriteBuffer buffer, string str)
		{
			int length = str.Length;
			char[] chars = new char[length];
			Sharpen.Runtime.GetCharsForString(str, 0, length, chars, 0);
			for (int i = 0; i < length; i++)
			{
				buffer.WriteByte((byte)(chars[i] & unchecked((int)(0xff))));
				buffer.WriteByte((byte)(chars[i] >> 8));
			}
		}

		public override byte[] Write(string str)
		{
			int length = str.Length;
			char[] chars = new char[length];
			Sharpen.Runtime.GetCharsForString(str, 0, length, chars, 0);
			byte[] bytes = new byte[length * 2];
			int j = 0;
			for (int i = 0; i < length; i++)
			{
				bytes[j++] = (byte)(chars[i] & unchecked((int)(0xff)));
				bytes[j++] = (byte)(chars[i] >> 8);
			}
			return bytes;
		}
	}
}
