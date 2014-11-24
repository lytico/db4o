/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect.Core
{
	/// <exclude></exclude>
	public class ReflectorUtils
	{
		public static IReflectClass ReflectClassFor(IReflector reflector, object clazz)
		{
			if (clazz is IReflectClass)
			{
				return (IReflectClass)clazz;
			}
			if (clazz is Type)
			{
				return reflector.ForClass((Type)clazz);
			}
			if (clazz is string)
			{
				return reflector.ForName((string)clazz);
			}
			return reflector.ForObject(clazz);
		}

		public static IReflectField Field(IReflectClass claxx, string name)
		{
			while (claxx != null)
			{
				try
				{
					return claxx.GetDeclaredField(name);
				}
				catch (Exception)
				{
				}
				claxx = claxx.GetSuperclass();
			}
			return null;
		}

		public static void ForEachField(IReflectClass claxx, IProcedure4 procedure)
		{
			while (claxx != null)
			{
				IReflectField[] declaredFields = claxx.GetDeclaredFields();
				for (int reflectFieldIndex = 0; reflectFieldIndex < declaredFields.Length; ++reflectFieldIndex)
				{
					IReflectField reflectField = declaredFields[reflectFieldIndex];
					procedure.Apply(reflectField);
				}
				claxx = claxx.GetSuperclass();
			}
		}
	}
}
