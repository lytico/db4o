/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

#if CF || SILVERLIGHT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Linq.Caching;

using Mono.Cecil;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class MetadataResolver
	{
		public static MetadataResolver Instance = new MetadataResolver();

		private ICache4<Assembly, AssemblyDefinition> _assemblyCache;
		private ICache4<MethodInfo, MethodDefinition> _methodCache;

		private MetadataResolver()
		{
			_assemblyCache = CacheFactory<Assembly, AssemblyDefinition>.For(CacheFactory.New2QXCache(5));
			_methodCache = CacheFactory<MethodInfo, MethodDefinition>.For(CacheFactory.New2QXCache(5));
		}

		private AssemblyDefinition GetAssembly(Assembly assembly)
		{
			return _assemblyCache.Produce(assembly, ReadAssembly);
		}

		private static AssemblyDefinition ReadAssembly(Assembly assembly)
		{
#if CF
			return AssemblyDefinition.ReadAssembly(assembly.ManifestModule.FullyQualifiedName);
#elif SILVERLIGHT
			var reference = AssemblyNameReference.Parse(assembly.FullName);
			var streamInfo = System.Windows.Application.GetResourceStream(new Uri(reference.Name + ".dll", UriKind.Relative));
			return AssemblyDefinition.ReadAssembly(streamInfo.Stream);
#endif
		}

#if CF
		private static string GetFullName(Type type)
		{
			if (type.DeclaringType != null) return type.FullName.Replace('+', '/');
			return type.FullName;
		}

		private TypeDefinition GetType(Type type)
		{
			var assembly = GetAssembly(type.Assembly);
			return assembly.MainModule.GetType(GetFullName(type));
		}

		private static bool ParameterMatch(ParameterDefinition parameter, ParameterInfo info)
		{
			return parameter.ParameterType.FullName == GetFullName(info.ParameterType);
		}

		private static bool ParametersMatch(IList<ParameterDefinition> parameters, ParameterInfo[] infos)
		{
			if (parameters.Count != infos.Length) return false;

			for (int i = 0; i < parameters.Count; i++)
			{
				if (!ParameterMatch(parameters[i], infos[i])) return false;
			}

			return true;
		}

		private static bool MethodMatch(MethodDefinition method, MethodInfo info)
		{
			if (method.Name != info.Name) return false;
			if (method.ReturnType.Name != info.ReturnType.Name) return false;

			return ParametersMatch(method.Parameters, info.GetParameters());
		}

		private MethodDefinition GetMethod(MethodInfo method)
		{
			TypeDefinition type = GetType(method.DeclaringType);

			var matches = from MethodDefinition candidate in type.Methods
						  where MethodMatch(candidate, method)
						  select candidate;

			return matches.FirstOrDefault();
		}

#elif SILVERLIGHT
		private MethodDefinition GetMethod(MethodInfo method)
		{
			var assembly = GetAssembly(method.DeclaringType.Assembly);
			return (MethodDefinition)assembly.MainModule.LookupToken(method.MetadataToken);
		}
#endif

		public MethodDefinition ResolveMethod(MethodInfo method)
		{
			if (method == null) throw new ArgumentNullException("method");

			return _methodCache.Produce(method, GetMethod);
		}
	}
}

#endif
