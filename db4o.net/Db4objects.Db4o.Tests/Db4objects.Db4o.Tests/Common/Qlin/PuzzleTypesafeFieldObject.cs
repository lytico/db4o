/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using System.Reflection;
using Db4oUnit;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Tests.Common.Qlin;

namespace Db4objects.Db4o.Tests.Common.Qlin
{
	public class PuzzleTypesafeFieldObject : ITestCase
	{
		private static Prototypes _prototypes = new Prototypes();

		public class Cat
		{
			public string name;

			public Cat(string name)
			{
				this.name = name;
			}
		}

		public virtual void TestTypeSafeFieldAsObject()
		{
			PuzzleTypesafeFieldObject.Cat cat = ((PuzzleTypesafeFieldObject.Cat)Prototype(typeof(
				PuzzleTypesafeFieldObject.Cat)));
			FieldInfo nameField = Field(cat, cat.name);
		}

		private object Prototype(Type clazz)
		{
			return _prototypes.PrototypeForClass(clazz);
		}

		public static FieldInfo Field(object onObject, object expression)
		{
			Type clazz = onObject.GetType();
			IEnumerator path = _prototypes.BackingFieldPath(onObject.GetType(), expression);
			path.MoveNext();
			Sharpen.Runtime.Out.WriteLine(((string)path.Current));
			return null;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_prototypes = new Prototypes(Prototypes.DefaultReflector(), RecursionDepth, IgnoreTransientFields
				);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		private const bool IgnoreTransientFields = true;

		private const int RecursionDepth = 10;
	}
}
#endif // !SILVERLIGHT
