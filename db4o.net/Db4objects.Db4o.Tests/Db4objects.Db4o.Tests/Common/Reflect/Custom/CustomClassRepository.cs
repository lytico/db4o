/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class CustomClassRepository
	{
		public Hashtable4 _classes;

		[System.NonSerialized]
		public CustomReflector _reflector;

		public CustomClassRepository()
		{
			// fields must be public so test works on less capable runtimes
			_classes = new Hashtable4();
		}

		public virtual CustomClass ForName(string className)
		{
			return (CustomClass)_classes.Get(className);
		}

		public virtual CustomClass DefineClass(string className, string[] fieldNames, string
			[] fieldTypes)
		{
			AssertNotDefined(className);
			CustomClass klass = CreateClass(className, fieldNames, fieldTypes);
			return DefineClass(klass);
		}

		private CustomClass CreateClass(string className, string[] fieldNames, string[] fieldTypes
			)
		{
			return new CustomClass(this, className, fieldNames, ResolveTypes(fieldTypes));
		}

		private Type[] ResolveTypes(string[] typeNames)
		{
			Type[] types = new Type[typeNames.Length];
			for (int i = 0; i < types.Length; ++i)
			{
				types[i] = ResolveType(typeNames[i]);
			}
			return types;
		}

		private Type ResolveType(string typeName)
		{
			if (typeName.Equals("string"))
			{
				return typeof(string);
			}
			if (typeName.Equals("int"))
			{
				return typeof(int);
			}
			throw new ArgumentException("Invalid type '" + typeName + "'");
		}

		private CustomClass DefineClass(CustomClass klass)
		{
			_classes.Put(klass.GetName(), klass);
			return klass;
		}

		private void AssertNotDefined(string className)
		{
			if (_classes.ContainsKey(className))
			{
				throw new ArgumentException("Class '" + className + "' already defined.");
			}
		}

		public virtual void Initialize(CustomReflector reflector)
		{
			_reflector = reflector;
		}

		public virtual IReflectClass ForFieldType(Type type)
		{
			return _reflector.ForFieldType(type);
		}

		public override string ToString()
		{
			return "CustomClassRepository(classes: " + _classes.Size() + ")";
		}

		public virtual IEnumerator Iterator()
		{
			return _classes.ValuesIterator();
		}
	}
}
