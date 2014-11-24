/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	public partial class DropGuidClassAndFieldIndexes_7_12 : DropClassIndexesConversion
	{
		private readonly FieldReindexer<Guid> reindexer = new FieldReindexer<Guid>();

		protected override bool Accept(ClassMetadata classmetadata)
		{
			var isGuid = NetReflector.ToNative(classmetadata.ClassReflector()) == typeof (Guid);
			if (!isGuid)
			{
				classmetadata.TraverseDeclaredFields(reindexer);
			}
			return isGuid;
		}
	}
}
