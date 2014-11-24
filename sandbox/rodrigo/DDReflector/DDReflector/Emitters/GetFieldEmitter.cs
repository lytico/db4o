using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace DDReflector.Emitters
{
	delegate object Getter(object o);

	class GetFieldEmitter : Emitter
	{
		public GetFieldEmitter(FieldInfo field) : base(field, typeof(object), new Type[] { typeof(object) })
		{
		}

		public Getter Emit()
		{
			EmitMethodBody();
			return (Getter)_dynamicMethod.CreateDelegate(typeof(Getter));
		}

		private void EmitMethodBody()
		{
			if (_field.IsStatic)
			{
				// make sure type is initialized before
				// accessing any static fields
				RuntimeHelpers.RunClassConstructor(_field.DeclaringType.TypeHandle);
				_il.Emit(OpCodes.Ldsfld, _field);
			}
			else
			{
				EmitLoadTargetObject(_field.DeclaringType);
				_il.Emit(OpCodes.Ldfld, _field);
			}

			EmitReturn();
		}

		protected void EmitReturn()
		{
			BoxIfNeeded(_field.FieldType);
			_il.Emit(OpCodes.Ret);
		}
	}
}
