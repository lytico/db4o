/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public interface IReflectClassBuilder
	{
		IReflectClass CreateClass(string name, IReflectClass superClass, int fieldCount);

		IReflectField CreateField(IReflectClass parentType, string fieldName, IReflectClass
			 fieldType, bool isVirtual, bool isPrimitive, bool isArray, bool isNArray);

		void InitFields(IReflectClass clazz, IReflectField[] fields);

		IReflectField[] FieldArray(int length);
	}
}
