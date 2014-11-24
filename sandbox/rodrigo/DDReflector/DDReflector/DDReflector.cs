using System;
using System.Reflection;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;
using DDReflector.Emitters;

namespace DDReflector
{
	public class DDReflector : NetReflector
	{
		override protected IReflectClass CreateClass(System.Type forType)
		{
			return new DDClass(_parent, forType);
		}
	}

	class DDClass : NetClass
	{
		public DDClass(IReflector reflector, Type clazz) : base(reflector, clazz)
		{
		}

		protected override IReflectField CreateField(FieldInfo field)
		{
			return new DDField(_reflector, field);
		}
	}

	class DDField : NetField
	{
		private Getter _getter;
		private Setter _setter;

		public DDField(IReflector reflector, FieldInfo field) : base(reflector, field)
		{	
		}

		public override object Get(object onObject)
		{
			if (null == _getter) _getter = new GetFieldEmitter(field).Emit();
			return _getter(onObject);
		}

		public override void Set(object onObject, object attribute)
		{
			if (null == _setter) _setter = new SetFieldEmitter(field).Emit();
			_setter(onObject, attribute);
		}
	}
}
