using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DDReflector.Emitters
{
	internal class Emitter
	{
		protected readonly FieldInfo _field;
		protected readonly DynamicMethod _dynamicMethod;
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
	}
}