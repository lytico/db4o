/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Self;

namespace Db4objects.Db4o.Reflect.Self
{
	public class SelfReflector : IReflector
	{
		private SelfArray _arrayHandler;

		private SelfReflectionRegistry _registry;

		private IReflector _parent;

		public SelfReflector(SelfReflectionRegistry registry)
		{
			_registry = registry;
		}

		public virtual IReflectArray Array()
		{
			if (_arrayHandler == null)
			{
				_arrayHandler = new SelfArray(this, _registry);
			}
			return _arrayHandler;
		}

		public virtual IReflectClass ForClass(Type clazz)
		{
			return new SelfClass(_parent, _registry, clazz);
		}

		public virtual IReflectClass ForName(string className)
		{
			Type clazz = ReflectPlatform.ForName(className);
			return ForClass(clazz);
		}

		public virtual IReflectClass ForObject(object a_object)
		{
			if (a_object == null)
			{
				return null;
			}
			return _parent.ForClass(a_object.GetType());
		}

		public virtual bool IsCollection(IReflectClass claxx)
		{
			return false;
		}

		public virtual void SetParent(IReflector reflector)
		{
			_parent = reflector;
		}

		public virtual object DeepClone(object context)
		{
			return new Db4objects.Db4o.Reflect.Self.SelfReflector(_registry);
		}

		public virtual void Configuration(IReflectorConfiguration config)
		{
		}
	}
}
