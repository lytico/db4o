namespace CustomInstrumentations
{
	using System;
	using Mono.Cecil;
	using Mono.Cecil.Cil;
	using Db4oTool.Core;

	/// <summary>
	/// Prepends Console.WriteLine("TRACE: " + method.Name) to every method
	/// in the assembly.
	/// </summary>
	public class Trace : AbstractAssemblyInstrumentation
	{
		override protected void ProcessMethod(MethodDefinition method)
		{
			if (!method.HasBody) return;

			MethodBody body = method.Body;
			Instruction firstInstruction = body.Instructions[0];
			CilWorker worker = body.CilWorker;

			// ldstr "TRACE: " + method
			worker.InsertBefore(firstInstruction,
								worker.Create(OpCodes.Ldstr, "TRACE: " + method));

			// call Console.WriteLine(string)
			MethodReference Console_WriteLine =
				Import(typeof(Console).GetMethod(
					"WriteLine", new Type[] { typeof(string) })
				);
			worker.InsertBefore(firstInstruction,
								worker.Create(OpCodes.Call, Console_WriteLine));
		}
	}
}
