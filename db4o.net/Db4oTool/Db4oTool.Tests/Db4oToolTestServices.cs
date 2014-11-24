/* Copyright (C) 2004 - 2010 Versant Inc.  http://www.db4o.com */
using System;
using System.Reflection;
using Db4oTool.Tests.Core;
using Mono.Cecil;

namespace Db4oTool.Tests
{
	class Db4oToolTestServices
	{
		public static AssemblyDefinition AssemblyFromResource(string resourceName, Type simblingType, bool loadSymbols, Action<string> sourceHandler, params Assembly[] references)
		{
			string assemblyPath = CompilationServices.EmitAssemblyFromResource(
										ResourceServices.CompleteResourceName(simblingType, resourceName),
										sourceHandler,
										references);

			ReaderParameters parameters = new ReaderParameters();
			parameters.ReadSymbols = loadSymbols;
			return AssemblyDefinition.ReadAssembly(assemblyPath, parameters);
		}
	}
}
