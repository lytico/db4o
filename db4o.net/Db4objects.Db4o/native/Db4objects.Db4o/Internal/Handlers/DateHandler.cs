/* Copyright (C) 2004 - 2009   Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Internal.Handlers
{
	public class DateHandler : DateHandlerBase
	{	
		public override object DefaultValue()
		{
			return DateTime.MinValue;
		}

		public override object PrimitiveNull()
		{
			return DateTime.MinValue;
		}

		public override object NullRepresentationInUntypedArrays()
		{
			return null;
		}

		public override object CopyValue(object from, object to)
		{
			// nothing to do since we already have a immutable
			// copy
			return from;
		}
	}
}
