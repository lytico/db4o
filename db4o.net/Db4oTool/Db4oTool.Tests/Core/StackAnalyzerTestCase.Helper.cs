/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Db4oTool.Core;
using Db4oUnit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ICustomAttributeProvider=System.Reflection.ICustomAttributeProvider;

namespace Db4oTool.Tests.Core
{
	partial class StackAnalyzerTestCase
	{
		private static bool IsMethodCallOnList(Instruction candidate)
		{
			if (candidate.OpCode != OpCodes.Call && candidate.OpCode != OpCodes.Callvirt) return false;

			MethodDefinition callee = ((MethodReference)candidate.Operand).Resolve();
			return callee.DeclaringType.Resolve().FullName == callee.DeclaringType.Module.Import(typeof(List<>)).FullName;
		}

		private static TypeReference FindType(AssemblyDefinition assembly, string typeName)
		{
			return assembly.MainModule.GetType(typeName.Replace('+', '/'));
		}

		private TypeReference FindType(Type testsDeclaringType)
		{
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(GetType().Module.Assembly.Location);
			return FindType(assembly, testsDeclaringType.FullName);
		}

		private MethodReference MethodReferenceFor(MethodInfo method, AssemblyDefinition assemblyDefinition)
		{
			return assemblyDefinition.MainModule.Import(method);
		}

		private static Instruction FindCast(MethodDefinition method)
		{
			TypeDefinition castType = method.DeclaringType.Module.Import(typeof(List<int>)).Resolve();
			foreach (Instruction instruction in method.Body.Instructions)
			{
				if (IsCastTo(castType, instruction))
				{
					return instruction;
				}
			}

			Assert.Fail(string.Format("No casts found in method {0}", method));

			return null;
		}

		private static Instruction FindCastOrNewObj(MethodDefinition methodDefinition)
		{
			TypeDefinition type = methodDefinition.DeclaringType.Module.Import(typeof(List<int>)).Resolve();
			foreach (Instruction instruction in methodDefinition.Body.Instructions)
			{
				if (IsCastTo(type, instruction))
				{
					return instruction;
				}

				if (IsInstantiationOf(type, instruction))
				{
					return instruction;
				}
			}

			Assert.Fail(string.Format("No casts / list instantiations found in method {0}", methodDefinition));

			return null;
		}

		private static bool IsInstantiationOf(TypeDefinition definition, Instruction candidate)
		{ 
			if (candidate.OpCode != OpCodes.Newobj) return false;

			MethodReference ctor = (MethodReference) candidate.Operand;
			return ctor.DeclaringType.Resolve() == definition;
		}

		private static bool IsCastTo(TypeDefinition castTarget, Instruction instruction)
		{
			if (instruction.OpCode != OpCodes.Castclass) return false;

			TypeReference typeReference = (TypeReference)instruction.Operand;
			return typeReference.Resolve() == castTarget;
		}

		private void AssertStackIsConsumed(Type testsDeclaringType)
		{
			AssertStack(testsDeclaringType, true);
		}
		
		private void AssertStackIsNotConsumed(Type testsDeclaringType)
		{
			AssertStack(testsDeclaringType, false);
		}

		private void AssertStack(Type testsDeclaringType, bool expectedResult)
		{
			TypeReference typeReference = FindType(testsDeclaringType);
            
			StringBuilder sb = new StringBuilder();
			foreach (MethodReference testMethod in typeReference.Resolve().Methods)
			{
				MethodDefinition methodDefinition = testMethod.Resolve();
				if (!methodDefinition.IsPublic) continue;
				if (methodDefinition.IsConstructor) continue;

				try
				{
					StackAnalysisResult stackAnalysis = StackAnalyzer.IsConsumedBy(IsMethodCallOnList, FindCast(methodDefinition), methodDefinition.DeclaringType.Module);
					if (expectedResult != stackAnalysis.Match)
					{
						sb.AppendFormat("Method {0} contains invalid cast operations.\r\n", testMethod);
					}
				}
				catch (Exception ex)
				{
					sb.AppendFormat("Exception while processing method {0}\r\n{1}", testMethod, ex);
				}
			}

			if (sb.Length > 0)
			{
				Assert.Fail(sb.ToString());
			}
		}

		private static ExpectedStackAnalysisResultAttribute ExpectedStackAnalysisResultFor(ICustomAttributeProvider method)
		{
			foreach(Attribute attribute in method.GetCustomAttributes(false))
			{
				if (attribute.GetType() == typeof(ExpectedStackAnalysisResultAttribute))
				{
					return (ExpectedStackAnalysisResultAttribute) attribute;
				}
			}

			return new ExpectedStackAnalysisResultAttribute();
		}
	}
}