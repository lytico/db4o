/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Foundation
{
	public class Coercion4
	{
		public static object ToByte(object obj)
		{
			if (obj is byte) return obj;
			return Coerce(obj, "byte", value => Convert.ToByte(obj));
		}

		public static object ToSByte(object obj)
		{
			if (obj is sbyte) return obj;
			return Coerce(obj, "sbyte", value => Convert.ToSByte(obj));
		}

		public static object ToShort(object obj)
		{
			if (obj is short) return obj;
			return Coerce(obj, "short", value => Convert.ToInt16(obj));
		}

		public static object ToUShort(object obj)
		{
			if (obj is ushort) return obj;
			return Coerce(obj, "ushort", val => Convert.ToUInt16(val));
		}

		public static object ToInt(object obj)
		{
			if (obj is int) return obj;
			return Coerce(obj, "int", value => Convert.ToInt32(obj));
		}
	
		public static object ToUInt(object obj)
		{
			if (obj is uint) return obj;
			return Coerce(obj, "uint", value => Convert.ToUInt32(value));

		}

		public static object ToLong(object obj)
		{
			if (obj is long) return obj;
			return Coerce(obj, "long", value => Convert.ToInt64(obj));
		}

		public static object ToULong(object obj)
		{
			if (obj is ulong) return obj;
			return Coerce(obj, "ulong", val => Convert.ToUInt64(val));
		}

		public static object ToFloat(object obj)
		{
			if (obj is float) return obj;
			return Coerce(obj, "float", value => Convert.ToSingle(obj));
		}

		public static object ToDouble(object obj)
		{
			if (obj is double) return obj; 
			return Coerce(obj, "double", value => Convert.ToDouble(obj));
		}

		public static object ToDecimal(object obj)
		{
			if (obj is decimal) return obj;
			return Coerce(obj, "decimal", value => Convert.ToDecimal(value));
		}

		private static object Coerce(object value, string typeName, Func<object, object> converter)
		{
			Func<object, string> formater = obj => string.Format("Unable to convert '{0}' to {1}.", obj, typeName);
			if (!(value is IConvertible))
			{
				throw new Db4oException(formater(value));
			}

			try
			{
				return converter(value);
			}
			catch (FormatException ex)
			{
				throw new Db4oException(formater(value), ex);
			}
		}

	}
}

