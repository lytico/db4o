/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class CustomStringEncodingTestCase : StringEncodingTestCaseBase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.StringEncoding(new _IStringEncoding_12());
		}

		private sealed class _IStringEncoding_12 : IStringEncoding
		{
			public _IStringEncoding_12()
			{
			}

			public byte[] Encode(string str)
			{
				int length = str.Length;
				char[] chars = new char[length];
				Sharpen.Runtime.GetCharsForString(str, 0, length, chars, 0);
				byte[] bytes = new byte[length * 4];
				int count = 0;
				for (int i = 0; i < length; i++)
				{
					bytes[count++] = (byte)(chars[i] & unchecked((int)(0xff)));
					bytes[count++] = (byte)(chars[i] >> 8);
					bytes[count++] = (byte)i;
					// bogus bytes, just for testing
					bytes[count++] = (byte)i;
				}
				return bytes;
			}

			public string Decode(byte[] bytes, int start, int length)
			{
				int stringLength = length / 4;
				char[] chars = new char[stringLength];
				int j = start;
				for (int ii = 0; ii < stringLength; ii++)
				{
					chars[ii] = (char)((bytes[j++] & unchecked((int)(0xff))) | ((bytes[j++] & unchecked(
						(int)(0xff))) << 8));
					j += 2;
				}
				return new string(chars, 0, stringLength);
			}
		}

		protected override Type StringIoClass()
		{
			return typeof(DelegatingStringIO);
		}
	}
}
