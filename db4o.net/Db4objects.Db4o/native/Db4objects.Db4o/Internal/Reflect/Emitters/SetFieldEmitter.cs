/* Copyright (C) 2004  - 2008  Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;
#if !CF
using System.Reflection.Emit;
#endif

namespace Db4objects.Db4o.Internal.Reflect.Emitters
{
#if !CF
	class SetFieldEmitter : Emitter
	{
		public SetFieldEmitter(FieldInfo field) : base(field, typeof(void), new Type[] { typeof(object), typeof(object) })
		{	
		}

		public Setter Emit()
		{
			EmitMethodBody();
			return (Setter) CreateDelegate<Setter>();
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
			if (_field.FieldType.IsValueType)
			{
				EmitLoadValueType();
			}
			else
			{
				EmitLoadReferenceType();
			}
			
		}

		private void EmitLoadReferenceType()
		{
			_il.Emit(OpCodes.Ldarg_1);
			_il.Emit(OpCodes.Castclass, _field.FieldType);
		}

		private void EmitLoadValueType()
		{
			Type fieldType = _field.FieldType;

			_il.Emit(OpCodes.Ldarg_1);
			Label nonNull = _il.DefineLabel();
			_il.Emit(OpCodes.Brtrue_S, nonNull);

			_il.DeclareLocal(fieldType);
			_il.Emit(OpCodes.Ldloc_0);
			Label end = _il.DefineLabel();
			_il.Emit(OpCodes.Br_S, end);
			_il.MarkLabel(nonNull);
			_il.Emit(OpCodes.Ldarg_1);
			_il.Emit(OpCodes.Unbox, fieldType);
			_il.Emit(OpCodes.Ldobj, fieldType);
			_il.MarkLabel(end);
		}
	}
#endif
}
