using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DDReflector.Emitters
{
	internal delegate void Setter(object o, object value);

	class SetFieldEmitter : Emitter
	{
		public SetFieldEmitter(FieldInfo field) : base(field, typeof(void), new Type[] { typeof(object), typeof(object) })
		{	
		}

		public Setter Emit()
		{
			EmitMethodBody();
			return (Setter) _dynamicMethod.CreateDelegate(typeof (Setter));
		}

		private void EmitMethodBody()
		{
			if (_field.IsStatic)
			{
				EmitLoadValue();
				_il.Emit(OpCodes.Stsfld, _field);
			}
			else
			{
				EmitLoadTargetObject(_field.DeclaringType);
				EmitLoadValue();
				_il.Emit(OpCodes.Stfld, _field);
			}
			_il.Emit(OpCodes.Ret);
		}

		private void EmitLoadValue()
		{
			_il.Emit(OpCodes.Ldarg_1);
			EmitCastOrUnbox(_field.FieldType);
		}

		protected void EmitCastOrUnbox(Type type)
		{
			if (type.IsValueType)
			{
				_il.Emit(OpCodes.Unbox, type);
				_il.Emit(OpCodes.Ldobj, type);
			}
			else
			{
				_il.Emit(OpCodes.Castclass, type);
			}
		}
	}
}
