/* Copyright (C) 2004  - 2008  Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Reflect.Emitters;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Internal.Reflect
{
#if !CF
	public class FastNetReflector : NetReflector
	{
		override protected IReflectClass CreateClass(Type forType)
		{
			return new FastNetClass(Parent(), this, forType);
		}

		public override object DeepClone(object obj)
		{
			return new FastNetReflector();
		}
	}

	class FastNetClass : NetClass
	{
		public FastNetClass(IReflector reflector, NetReflector netReflector, Type clazz) : base(reflector, netReflector, clazz)
		{
		}

		protected override IReflectField CreateField(FieldInfo field)
		{
			return new FastNetField(_reflector, field);
		}
	}

	class FastNetField : NetField
	{
		private Getter _getter;
		private Setter _setter;

		public FastNetField(IReflector reflector, FieldInfo field) : base(reflector, field)
		{	
		}

		public override object Get(object onObject)
		{
			if (null == _getter) _getter = AccessorFactory.GetterFor(_field);
			try
			{
				return _getter(onObject);
			}
			catch (FieldAccessException)
			{
				_getter = _field.GetValue;
				return _getter(onObject);
			}
			catch (Exception e)
			{
				throw new Db4oException(e);
			}
		}

		public override void Set(object onObject, object attribute)
		{
			if (null == _setter) _setter = AccessorFactory.SetterFor(_field);
			try
			{
				_setter(onObject, attribute);
			}
			catch (FieldAccessException)
			{
				_setter = _field.SetValue;
				_setter(onObject, attribute);
			}
			catch (Exception e)
			{
				throw new Db4oException(e);
			}
		}
	}
#endif
}
