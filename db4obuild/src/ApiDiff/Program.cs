using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Mono.Cecil;

namespace ApiDiff
{
	class Program
	{
		static void Main(string[] args)
		{
			var baseAssemblyPath = args[0];
			var currentAssemblyPath = args[1];

			var baseAssembly = AssemblyFactory.GetAssembly(baseAssemblyPath);
			var currentAssembly = AssemblyFactory.GetAssembly(currentAssemblyPath);

			Environment.ExitCode = new ApiDiffProcessor(baseAssembly, currentAssembly).Run();
		}

		
	}
}
