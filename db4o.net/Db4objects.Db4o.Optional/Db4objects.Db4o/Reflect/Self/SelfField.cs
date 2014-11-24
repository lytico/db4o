/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Self;

namespace Db4objects.Db4o.Reflect.Self
{
	public class SelfField : IReflectField
	{
		private string _name;

		private IReflectClass _type;

		private SelfClass _selfclass;

		private SelfReflectionRegistry _registry;

		public SelfField(string name, IReflectClass type, SelfClass selfclass, SelfReflectionRegistry
			 registry)
		{
			_name = name;
			_type = type;
			_selfclass = selfclass;
			_registry = registry;
		}

		public virtual object Get(object onObject)
		{
			if (onObject is ISelfReflectable)
			{
				return ((ISelfReflectable)onObject).Self_get(_name);
			}
			return null;
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual IReflectClass GetFieldType()
		{
			return _type;
		}

		public virtual bool IsPublic()
		{
			return _registry.InfoFor(_selfclass.GetJavaClass()).FieldByName(_name).IsPublic();
		}

		public virtual bool IsStatic()
		{
			return _registry.InfoFor(_selfclass.GetJavaClass()).FieldByName(_name).IsStatic();
		}

		public virtual bool IsTransient()
		{
			return _registry.InfoFor(_selfclass.GetJavaClass()).FieldByName(_name).IsTransient
				();
		}

		public virtual void Set(object onObject, object value)
		{
			if (onObject is ISelfReflectable)
			{
				((ISelfReflectable)onObject).Self_set(_name, value);
			}
		}

		public virtual object IndexEntry(object orig)
		{
			return orig;
		}

		public virtual IReflectClass IndexType()
		{
			return GetFieldType();
		}
	}
}
