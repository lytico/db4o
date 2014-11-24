/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	public class PrimitiveMarshaller1 : PrimitiveMarshaller
	{
		public override bool UseNormalClassRead()
		{
			return false;
		}

		public override DateTime ReadDate(ByteArrayBuffer bytes)
		{
			return new DateTime(bytes.ReadLong());
		}

		public override object ReadInteger(ByteArrayBuffer bytes)
		{
			return bytes.ReadInt();
		}

		public override object ReadFloat(ByteArrayBuffer bytes)
		{
			return PrimitiveMarshaller0.UnmarshallFloat(bytes);
		}

		public override object ReadDouble(ByteArrayBuffer buffer)
		{
			return PrimitiveMarshaller0.UnmarshalDouble(buffer);
		}

		public override object ReadLong(ByteArrayBuffer buffer)
		{
			return buffer.ReadLong();
		}

		public override object ReadShort(ByteArrayBuffer buffer)
		{
			return PrimitiveMarshaller0.UnmarshallShort(buffer);
		}
	}
}
