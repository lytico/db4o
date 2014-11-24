/* Copyright (C) 2004  - 2008  Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;
#if !CF
using System.Reflection.Emit;
#endif

namespace Db4objects.Db4o.Internal.Reflect.Emitters
{
#if !CF
	internal class Emitter
	{
		private readonly DynamicMethod _dynamicMethod;
		protected readonly FieldInfo _field;
		protected readonly ILGenerator _il;

		public Emitter(FieldInfo field, Type returnType, Type[] paramTypes)
		{
			_field = field;
			_dynamicMethod = new DynamicMethod(_field.DeclaringType.Name + "$" + _field.Name, returnType, paramTypes, _field.DeclaringType);
			_il = _dynamicMethod.GetILGenerator();
		}

		protected void BoxIfNeeded(Type type)
		{
			if (!type.IsValueType) return;
			_il.Emit(OpCodes.Box, type);
		}

		protected void EmitLoadTargetObject(Type expectedType)
		{
			_il.Emit(OpCodes.Ldarg_0); // target object is the first argument
			if (expectedType.IsValueType)
			{
				_il.Emit(OpCodes.Unbox, expectedType);
			}
			else
			{
				_il.Emit(OpCodes.Castclass, expectedType);
			}
		}

		protected Delegate CreateDelegate<T>()
		{
			return  _dynamicMethod.CreateDelegate(typeof(T));
		}
	}
#endif
}