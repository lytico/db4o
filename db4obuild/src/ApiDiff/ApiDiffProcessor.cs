using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace ApiDiff
{
	public class ApiDiffProcessor
	{
		private AssemblyDefinition _base;
		private AssemblyDefinition _current;
		private int _differences;

		public ApiDiffProcessor(AssemblyDefinition baseAssembly, AssemblyDefinition currentAssembly)
		{
			_base = baseAssembly;
			_current = currentAssembly;
		}

		public int Run()
		{
			CheckTypes();
			return _differences;
		}

		private void CheckTypes()
		{
			var baseTypes = PublicNameSetFor(_base);
			var currentTypes = PublicNameSetFor(_current);

			CompareSets("TypeDefinition", currentTypes, baseTypes);

			CheckTypes(baseTypes.Intersect(currentTypes));
		}

		private void CompareSets<T>(string what, ICollection<T> currentTypes, ICollection<T> baseTypes)
		{
			LogDifferences("Unexpected " + what  + ": ", Difference(currentTypes, baseTypes));
			LogDifferences("Missing " + what + ": ", Difference(baseTypes, currentTypes));
		}

		private void CheckTypes(IEnumerable<string> typeNames)
		{
			foreach (var typeName in typeNames)
			{
				var baseType = GetType(_base, typeName);
				var currentType = GetType(_current, typeName);
				CheckBaseType(baseType, currentType);
				CheckInterfaces(baseType, currentType);
				CheckMembers(baseType, currentType);
			}
		}

		private TypeDefinition GetType(AssemblyDefinition assembly, string typeName)
		{
			return assembly.MainModule.Types[typeName];
		}

		private void CheckMembers(TypeDefinition baseType, TypeDefinition currentType)
		{
			CompareSets("method on '" + baseType + "'", MethodSignaturesFor(baseType), MethodSignaturesFor(currentType));
		}

		private List<string> MethodSignaturesFor(TypeDefinition type)
		{
			var signatures = (from MethodDefinition m in type.Methods where m.IsPublic select m.ToString()).ToList();
			signatures.Sort();
			return signatures;
		}

		private void CheckInterfaces(TypeDefinition baseType, TypeDefinition currentType)
		{
			CompareSets("interface on TypeDefinition '" + baseType + "'", InterfaceNamesFor(baseType), InterfaceNamesFor(currentType));
		}

		private List<string> InterfaceNamesFor(TypeDefinition baseType)
		{
			return (from TypeReference typeRef in baseType.Interfaces select typeRef.FullName).ToList();
		}

		private void CheckBaseType(TypeDefinition baseType, TypeDefinition currentType)
		{
			var baseBaseType = baseType.BaseType;
			var currentBaseType = currentType.BaseType;
			if (baseBaseType == null || currentBaseType == null)
			{
				if (baseBaseType != currentBaseType)
				{
					UnexpectedBaseType(baseType, baseBaseType, currentBaseType);
				}
				return;
			}

			if (baseBaseType.FullName != currentBaseType.FullName)
			{
				UnexpectedBaseType(baseType, baseBaseType, currentBaseType);
			}
		}

		private void UnexpectedBaseType(TypeDefinition baseType, TypeReference baseBaseType, TypeReference currentBaseType)
		{
			LogDifference("{0}: Expecting base TypeDefinition '{1}' got '{2}'", baseType, baseBaseType, currentBaseType);
		}

		private void LogDifferences<T>(string prefix, IEnumerable<T> difference)
		{
			foreach (var item in difference)
			{
				LogDifference("{0}{1}", prefix, item);
			}
		}

		private void LogDifference(string format, params object[] args)
		{
			++_differences;
			Console.Error.WriteLine(format, args);
		}

		private IEnumerable<T> Difference<T>(IEnumerable<T> a, ICollection<T> b)
		{
			foreach (var item in a)
			{
				if (!b.Contains(item))
				{
					yield return item;
				}
			}
		}

		private HashSet<string> PublicNameSetFor(AssemblyDefinition assembly)
		{
			return new HashSet<string>(PublicTypeNamesFor(assembly));
		}

		private static IEnumerable<string> PublicTypeNamesFor(AssemblyDefinition assembly)
		{
			foreach (ModuleDefinition m in assembly.Modules)
			{
				foreach (TypeDefinition type in m.Types)
				{
					if (type.IsPublic)
					{
						yield return type.FullName;
					}
				}
			}
		}
	}
}
