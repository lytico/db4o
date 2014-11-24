/* Copyright (C) 2004 - 2008 Versant Inc.   http://www.db4o.com */

#if !CF
using Db4objects.Db4o.Internal.Reflect;
#endif

using System.Collections.Generic;
using Db4objects.Db4o.Reflect;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Reflector
{
	public class FastNetReflectorTestCase : ITestCase, IOptOutSilverlight
	{
#if !CF
		public void TestNullAssignmentToValueTypeField()
		{
			FastNetReflector reflector = new FastNetReflector();
			IReflectField field = reflector.ForClass(typeof (ValueTypeContainer)).GetDeclaredField("_value");
			ValueTypeContainer subject = new ValueTypeContainer(0xDb40);
			
			field.Set(subject, null);
			Assert.AreEqual(0, subject.Value);

			field.Set(subject, 42);
			Assert.AreEqual(42, subject.Value);
		}

		public void TestNonAccessibleGenericTypeParamenterBugInReflectionEmit()
		{
			FastNetReflector reflector = new FastNetReflector();
			IReflectField sizeField = reflector.ForClass(typeof(GenericClass<NotAccessible>)).GetDeclaredField("_size");

			GenericClass<NotAccessible> obj = new GenericClass<NotAccessible>();
			sizeField.Set(obj, 42);
			Assert.AreEqual(42, sizeField.Get(obj));
		}

#if !NET_4_0 //TODO: Investigate why this is failing on .Net 4.0
		public void TestDynamicMethodsOnSecurityCriticalTypes()
		{
			FastNetReflector reflector = new FastNetReflector();
			IReflectField sizeField = reflector.ForClass(typeof(List<NotAccessible>)).GetDeclaredField("_size");

			List<NotAccessible> obj = new List<NotAccessible>();
			sizeField.Set(obj, 42);
			Assert.AreEqual(42, sizeField.Get(obj));
		}
#endif

		internal class ValueTypeContainer
		{
			private int _value;

			public ValueTypeContainer(int initialValue)
			{
				_value = initialValue;
			}

			public int Value
			{
				get { return _value;}
			}
		}

		private class NotAccessible
		{
		}

		class GenericClass<T>
		{
			private int _size;
		}
#endif
	}
}
