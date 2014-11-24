/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericClassBuilder : IReflectClassBuilder
	{
		private GenericReflector _reflector;

		private IReflector _delegate;

		public GenericClassBuilder(GenericReflector reflector, IReflector delegate_) : base
			()
		{
			_reflector = reflector;
			_delegate = delegate_;
		}

		public virtual IReflectClass CreateClass(string name, IReflectClass superClass, int
			 fieldCount)
		{
			IReflectClass nativeClass = _delegate.ForName(name);
			GenericClass clazz = new GenericClass(_reflector, nativeClass, name, (GenericClass
				)superClass);
			clazz.SetDeclaredFieldCount(fieldCount);
			return clazz;
		}

		public virtual IReflectField CreateField(IReflectClass parentType, string fieldName
			, IReflectClass fieldType, bool isVirtual, bool isPrimitive, bool isArray, bool 
			isNArray)
		{
			if (isVirtual)
			{
				return new GenericVirtualField(fieldName);
			}
			return new GenericField(fieldName, fieldType, isPrimitive);
		}

		public virtual void InitFields(IReflectClass clazz, IReflectField[] fields)
		{
			((GenericClass)clazz).InitFields((GenericField[])fields);
		}

		public virtual IReflectField[] FieldArray(int length)
		{
			return new GenericField[length];
		}
	}
}
