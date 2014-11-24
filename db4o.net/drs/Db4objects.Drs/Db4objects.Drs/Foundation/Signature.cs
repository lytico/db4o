/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Text;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Drs.Foundation
{
	public class Signature
	{
		public readonly byte[] bytes;

		public Signature(byte[] bytes)
		{
			this.bytes = bytes;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is Db4objects.Drs.Foundation.Signature))
			{
				return false;
			}
			Db4objects.Drs.Foundation.Signature other = (Db4objects.Drs.Foundation.Signature)
				obj;
			return Arrays4.Equals(bytes, other.bytes);
		}

		public override int GetHashCode()
		{
			int hc = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				hc <<= 2;
				hc = hc ^ bytes[i];
			}
			return hc;
		}

		public override string ToString()
		{
			return ToString(bytes);
		}

		public static string ToString(byte[] bytes)
		{
			return BytesToString(bytes);
		}

		private static string BytesToString(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				char c = (char)bytes[i];
				sb.Append(c);
			}
			return sb.ToString();
		}

		public virtual string AsString()
		{
			return BytesToString(bytes);
		}
	}
}
