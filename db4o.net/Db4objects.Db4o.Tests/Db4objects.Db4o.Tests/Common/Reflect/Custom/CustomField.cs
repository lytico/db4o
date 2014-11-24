/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	/// <summary>
	/// One important thing to remember when implementing ReflectField
	/// is that getFieldType and getIndexType must always return ReflectClass
	/// instances given by the parent reflector.
	/// </summary>
	/// <remarks>
	/// One important thing to remember when implementing ReflectField
	/// is that getFieldType and getIndexType must always return ReflectClass
	/// instances given by the parent reflector.
	/// </remarks>
	public class CustomField : IReflectField
	{
		public CustomClassRepository _repository;

		public string _name;

		public Type _type;

		public int _index;

		public bool _indexed;

		public CustomField()
		{
		}

		public CustomField(CustomClassRepository repository, int index, string name, Type
			 type)
		{
			// fields must be public so test works on less capable runtimes
			_repository = repository;
			_index = index;
			_name = name;
			_type = type;
		}

		public virtual object Get(object onObject)
		{
			LogMethodCall("get", onObject);
			return FieldValues(onObject)[_index];
		}

		private object[] FieldValues(object onObject)
		{
			return ((PersistentEntry)onObject).fieldValues;
		}

		public virtual IReflectClass GetFieldType()
		{
			LogMethodCall("getFieldType");
			return _repository.ForFieldType(_type);
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual object IndexEntry(object orig)
		{
			LogMethodCall("indexEntry", orig);
			return orig;
		}

		public virtual IReflectClass IndexType()
		{
			LogMethodCall("indexType");
			return GetFieldType();
		}

		public virtual bool IsPublic()
		{
			return true;
		}

		public virtual bool IsStatic()
		{
			return false;
		}

		public virtual bool IsTransient()
		{
			return false;
		}

		public virtual void Set(object onObject, object value)
		{
			LogMethodCall("set", onObject, value);
			FieldValues(onObject)[_index] = value;
		}

		public virtual void Indexed(bool value)
		{
			_indexed = value;
		}

		public virtual bool Indexed()
		{
			return _indexed;
		}

		public override string ToString()
		{
			return "CustomField(" + _index + ", " + _name + ", " + _type.FullName + ")";
		}

		private void LogMethodCall(string methodName)
		{
			Logger.LogMethodCall(this, methodName);
		}

		private void LogMethodCall(string methodName, object arg)
		{
			Logger.LogMethodCall(this, methodName, arg);
		}

		private void LogMethodCall(string methodName, object arg1, object arg2)
		{
			Logger.LogMethodCall(this, methodName, arg1, arg2);
		}
	}
}
