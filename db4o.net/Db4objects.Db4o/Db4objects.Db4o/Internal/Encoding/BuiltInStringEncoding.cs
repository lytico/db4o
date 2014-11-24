/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Internal.Encoding
{
	/// <exclude></exclude>
	public abstract class BuiltInStringEncoding : IStringEncoding
	{
		/// <summary>keep the position in the array.</summary>
		/// <remarks>
		/// keep the position in the array.
		/// Information is used to look up encodings.
		/// </remarks>
		private static readonly BuiltInStringEncoding[] AllEncodings = new BuiltInStringEncoding
			[] { null, new LatinStringEncoding(), new UnicodeStringEncoding(), new UTF8StringEncoding
			() };

		public static byte EncodingByteForEncoding(IStringEncoding encoding)
		{
			for (int i = 1; i < AllEncodings.Length; i++)
			{
				if (encoding.GetType() == AllEncodings[i].GetType())
				{
					return (byte)i;
				}
			}
			return 0;
		}

		public static LatinStringIO StringIoForEncoding(byte encodingByte, IStringEncoding
			 encoding)
		{
			if (encodingByte < 0 || encodingByte > AllEncodings.Length)
			{
				throw new ArgumentException();
			}
			if (encodingByte == 0)
			{
				if (encoding is BuiltInStringEncoding)
				{
					Sharpen.Runtime.Out.WriteLine("Warning! Database was created with a custom string encoding but no custom string encoding is configured for this session."
						);
				}
				return new DelegatingStringIO(encoding);
			}
			BuiltInStringEncoding builtInEncoding = AllEncodings[encodingByte];
			return builtInEncoding.CreateStringIo(encoding);
		}

		protected virtual LatinStringIO CreateStringIo(IStringEncoding encoding)
		{
			return new DelegatingStringIO(encoding);
		}

		public abstract string Decode(byte[] arg1, int arg2, int arg3);

		public abstract byte[] Encode(string arg1);
	}
}
