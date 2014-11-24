/* Copyright (C) 2004  - 2008  Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;
#if !CF
using System.Reflection.Emit;
#endif
using System.Runtime.CompilerServices;

namespace Db4objects.Db4o.Internal.Reflect.Emitters
{
#if !CF
	class GetFieldEmitter : Emitter
	{
		public GetFieldEmitter(FieldInfo field) : base(field, typeof(object), new Type[] { typeof(object) })
		{
		}

		public Getter Emit()
		{
			EmitMethodBody();
			return (Getter)CreateDelegate <Getter>();
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
#endif
}
