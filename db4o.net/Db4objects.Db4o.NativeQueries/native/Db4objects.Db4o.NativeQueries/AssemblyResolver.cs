/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using System.Reflection;
using Db4objects.Db4o.Internal.Query;
using Mono.Cecil;

namespace Db4objects.Db4o.NativeQueries
{
	internal class AssemblyResolver
	{
		public AssemblyResolver(ICachingStrategy<string, AssemblyDefinition> assemblyCache)
		{
			_assemblyCachingStrategy = assemblyCache;
		}

		public AssemblyDefinition ForTypeReference(TypeReference type)
		{
			AssemblyNameReference scope = (AssemblyNameReference)type.Scope;
			string assemblyName = scope.FullName;
			AssemblyDefinition definition = LookupAssembly(assemblyName);
			if (null == definition)
			{
				Assembly assembly = Assembly.Load(assemblyName);
				string location = assembly.GetType(type.FullName, true).Module.FullyQualifiedName;
				definition = _assemblyCachingStrategy.Get(location);
				RegisterAssembly(definition);
			}
			return definition;
		}
		
		private AssemblyDefinition LookupAssembly(string fullName)
		{
			return _assemblies.ContainsKey(fullName) ? _assemblies[fullName] : null;
		}

		/// <summary>
		/// Registers an assembly so it can be looked up by its assembly name
		/// string later.
		/// </summary>
		/// <param name="assembly"></param>
		private void RegisterAssembly(AssemblyDefinition assembly)
		{
			_assemblies.Add(assembly.Name.FullName, assembly);
		}

		public AssemblyDefinition ForType(TypeDefinition type)
		{
			AssemblyDefinition assembly = type.Module.Assembly;
			RegisterAssembly(assembly);

			return assembly;
		}

		private readonly ICachingStrategy<string, AssemblyDefinition> _assemblyCachingStrategy;
		readonly IDictionary<string, AssemblyDefinition> _assemblies = new Dictionary<string, AssemblyDefinition>();
	}
}
