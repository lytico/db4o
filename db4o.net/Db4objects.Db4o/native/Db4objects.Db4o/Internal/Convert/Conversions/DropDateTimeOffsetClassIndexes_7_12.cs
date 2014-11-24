/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	public partial class DropDateTimeOffsetClassIndexes_7_12 : DropClassIndexesConversion
	{
		protected override bool Accept(ClassMetadata classmetadata)
		{
#if CF || SILVERLIGHT
			return false;
#else
			return NetReflector.ToNative(classmetadata.ClassReflector()) == typeof(DateTimeOffset);
#endif
		}
	}
}
