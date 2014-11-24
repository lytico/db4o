/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	public abstract class PrimitiveMarshaller
	{
		public MarshallerFamily _family;

		public abstract bool UseNormalClassRead();

		public abstract DateTime ReadDate(ByteArrayBuffer bytes);

		public abstract object ReadShort(ByteArrayBuffer buffer);

		public abstract object ReadInteger(ByteArrayBuffer buffer);

		public abstract object ReadFloat(ByteArrayBuffer buffer);

		public abstract object ReadDouble(ByteArrayBuffer buffer);

		public abstract object ReadLong(ByteArrayBuffer buffer);
	}
}
