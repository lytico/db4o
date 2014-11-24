/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class SlotFormat0 : SlotFormat
	{
		protected override int HandlerVersion()
		{
			return 0;
		}

		public override bool IsIndirectedWithinSlot(ITypeHandler4 handler)
		{
			return false;
		}

		public override void WriteObjectClassID(ByteArrayBuffer buffer, int id)
		{
			buffer.WriteInt(id);
		}

		public override void SkipMarshallerInfo(ByteArrayBuffer reader)
		{
		}

		public override ObjectHeaderAttributes ReadHeaderAttributes(ByteArrayBuffer reader
			)
		{
			return null;
		}
	}
}
