/* Copyright (C) 2012  Versant Inc.  http://www.db4o.com */
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	public class FieldReindexer<T> : IProcedure4 where T : struct
	{
		public void Apply(object field)
		{
			if (!((FieldMetadata)field).HasIndex())
			{
				return;
			}
			ReindexField(((FieldMetadata)field));
		}

		private static void ReindexField(IStoredField field)
		{
			var claxx = field.GetStoredType();
			if (claxx == null)
			{
				return;
			}

			var t = NetReflector.ToNative(claxx);
			if (t == typeof(T) || t == typeof(T?))
			{
				field.DropIndex();
				field.CreateIndex();
			}
		}
	}
}
