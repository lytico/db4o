using System;
using Db4oTool.Core;
using Db4objects.Db4o.TA;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.TA
{
	class BindMethodEmitter : MethodEmitter
	{
		public BindMethodEmitter(InstrumentationContext context, FieldReference field) : base(context, field)
		{
		}

		public MethodDefinition Emit()
		{
			MethodDefinition bind = NewExplicitMethod(typeof(IActivatable).GetMethod("Bind"));
			ILProcessor cil = bind.Body.GetILProcessor ();

			Instruction activatorSetting = cil.Create(OpCodes.Ldarg_0);

			// if (_activator == activator) {
			//   return;
			// }
			LoadActivatorField(cil);
			cil.Emit(OpCodes.Ldarg_1);

			Instruction isParameterNullInstruction = cil.Create(OpCodes.Ldarg_1);

			cil.Emit(OpCodes.Bne_Un, isParameterNullInstruction);
			cil.Emit(OpCodes.Ret);

			// if (activator != null && _activator != null) {
			//   throw new InvalidOperationException();
			// }
			cil.Append(isParameterNullInstruction);
			cil.Emit(OpCodes.Brfalse, activatorSetting);
			LoadActivatorField(cil);
			cil.Emit(OpCodes.Brfalse, activatorSetting);

			cil.Emit(OpCodes.Newobj, _context.Import(typeof(InvalidOperationException).GetConstructor(new Type[0])));
			cil.Emit(OpCodes.Throw);
			
			// _activator = activator;
			cil.Append(activatorSetting);
			cil.Emit(OpCodes.Ldarg_1);
			cil.Emit(OpCodes.Stfld, _activatorField);

			cil.Emit(OpCodes.Ret);

			return bind;
		}

		private void LoadActivatorField(ILProcessor cil)
		{
			cil.Emit(OpCodes.Ldarg_0);
			cil.Emit(OpCodes.Ldfld, _activatorField);
		}
	}
}
