/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.CLI1.Reflect.Net
{
	public class UnavailableClassPrimitiveRepresentationTestCase : UnavailableClassTestCaseBase
	{
		protected override void Store()
		{
			foreach (Type type in Platform4.PrimitiveTypes())
			{
				Store(type);
			}
		}

		private void Store(Type type)
		{
			Type genericItemType = GenericItemTypeFor(type);
			object instance = Activator.CreateInstance(genericItemType);

			Store(instance);
		}

		public void TestPrimitiveTypeRepresentation()
		{
			
			ReopenHidingClasses(ExcludedTypes());

			IReflector reflector = Db().Ext().Reflector();

			foreach (Type type in Platform4.PrimitiveTypes())
			{
				IReflectClass klass = reflector.ForName(TypeReference.FromType(GenericItemTypeFor(type)).GetUnversionedName());
				AssertType(reflector, type, FieldType(klass, "simpleField"));
				AssertType(reflector, NullableTypeFor(type), FieldType(klass, "nullableField"));
			}
		}

		private static Type[] ExcludedTypes()
		{
			Type[] excludedTypes = new Type[Platform4.PrimitiveTypes().Length];
			int i = 0;
			foreach (Type type in Platform4.PrimitiveTypes())
			{
				excludedTypes[i++] = GenericItemTypeFor(type);
			}

			return excludedTypes;
		}

		private static Type NullableTypeFor(Type type)
		{
			return typeof (Nullable<>).MakeGenericType(type);
		}

		private static IReflectClass FieldType(IReflectClass klass, string fieldName)
		{
			return Field(klass, fieldName).GetFieldType();
		}

		private static void AssertType(IReflector reflector, Type expected, IReflectClass actual)
		{
			Assert.AreEqual(reflector.ForClass(expected), actual);
		}

		private static IReflectField Field(IReflectClass klass, string fieldName)
		{
			return klass.GetDeclaredField(fieldName);
		}

		private static Type GenericItemTypeFor(Type type)
		{
			return typeof(Item<>).MakeGenericType(type);
		}
	}

	public class Item<T> where T : struct
	{
		public T simpleField;
		public T? nullableField;
	}
}
