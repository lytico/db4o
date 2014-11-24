/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using Db4objects.Db4o.Collections;
using Db4oTool.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.TA
{
	internal class TACollectionsStep : TAInstrumentationStepBase
	{
		public override void Process(MethodDefinition method)
		{
			InstrumentCollectionInstantiation(method);
			InstrumentConcreteCollectionCasts(method);
		}

		private void InstrumentConcreteCollectionCasts(MethodDefinition methodDefinition)
		{
			foreach (Instruction cast in CastsToSupportedCollections(methodDefinition.Body))
			{
				StackAnalysisResult result = StackAnalyzer.IsConsumedBy(MethodCallOnSupportedCollections, cast, methodDefinition.DeclaringType.Module);
				if (!result.Match)
				{
				    throw new InvalidOperationException(string.Format("Error: [{0}] Invalid use of cast result: '{1}'.\r\nCasts to {2} are only allowed for property access/method calls.", methodDefinition, DebugInformation.InstructionInformationFor(result.Consumer, methodDefinition.Body.Instructions), cast.Operand));
				}

				TypeReference castTarget = (TypeReference) cast.Operand;
				ReplaceCastAndCalleeDeclaringType(cast, result.Consumer, _collectionReplacements[castTarget.Resolve().FullName]);
			}
		}

		private void ReplaceCastAndCalleeDeclaringType(Instruction cast, Instruction originalCall, Type replacementType)
		{
			GenericInstanceType originalTypeReference = (GenericInstanceType)cast.Operand;

			GenericInstanceType replacementReferenceType = NewGenericInstanceTypeWithArgumentsFrom(Context.Import(replacementType), originalTypeReference);
			cast.Operand = replacementReferenceType;
			originalCall.Operand = MethodReferenceFor((MethodReference)originalCall.Operand, replacementReferenceType);
		}

		private static MethodReference MethodReferenceFor(MethodReference source, TypeReference declaringType)
		{
			MethodReference newMethod = new MethodReference(source.Name, source.ReturnType);
			newMethod.DeclaringType = declaringType;
			newMethod.HasThis = true;

			foreach (ParameterDefinition param in source.Parameters)
			{
				newMethod.Parameters.Add(param);
			}

			return newMethod;
		}

		private static GenericInstanceType NewGenericInstanceTypeWithArgumentsFrom(TypeReference referenceType, GenericInstanceType argumentSource)
		{
			GenericInstanceType replacementTypeReference = new GenericInstanceType(referenceType);
			foreach (TypeReference argument in argumentSource.GenericArguments)
			{
				replacementTypeReference.GenericArguments.Add(argument);
			}
			return replacementTypeReference;
		}

		private static bool MethodCallOnSupportedCollections(Instruction candidate)
		{
			if (candidate.OpCode != OpCodes.Call && candidate.OpCode != OpCodes.Callvirt) return false;

			MethodDefinition callee = ((MethodReference)candidate.Operand).Resolve();
			return HasReplacement(callee.DeclaringType.Resolve().FullName);
		}

		private static bool HasReplacement(string collectionConcreteType)
		{
			return _collectionReplacements.ContainsKey(collectionConcreteType);
		}

		private IEnumerable<Instruction> CastsToSupportedCollections(MethodBody body)
		{
			return InstrumentationUtil.Where(body, delegate(Instruction candidate)
			{
				if (candidate.OpCode != OpCodes.Castclass) return false;
				GenericInstanceType target = candidate.Operand as GenericInstanceType;

				return target != null && HasReplacement(target.Resolve().FullName);
			});
		}

		private void InstrumentCollectionInstantiation(MethodDefinition methodDefinition)
		{
			foreach (Instruction newObj in TAEnabledCollectionInstantiations(methodDefinition.Body))
			{
				StackAnalysisResult stackAnalysis = StackAnalyzer.IsConsumedBy(delegate { return true; }, newObj, methodDefinition.DeclaringType.Module);
				if (IsAssignmentToConcreteType(stackAnalysis))
				{
					Context.TraceWarning("[{0}] Assignment to concrete collection {1} ignored (offset: 0x{2:X2}).", methodDefinition, InstantiatedType(newObj), newObj.Next.Offset);
					continue;
				}

				ReplaceContructorWithConstructorFrom(newObj);
			}
		}

		private static string InstantiatedType(Instruction newObj)
		{
			MethodReference originalCtor = (MethodReference)newObj.Operand;

			GenericInstanceType originalType = (GenericInstanceType)originalCtor.DeclaringType;

			return originalType.FullName;
		}

		private void ReplaceContructorWithConstructorFrom(Instruction newObj)
		{
			MethodReference originalCtor = (MethodReference)newObj.Operand;

			GenericInstanceType originalList = (GenericInstanceType)originalCtor.DeclaringType;
			GenericInstanceType declaringType = new GenericInstanceType(Context.Import(_collectionReplacements[originalList.Resolve().FullName]));

			foreach (TypeReference argument in originalList.GenericArguments)
			{
				declaringType.GenericArguments.Add(argument);
			}

			MethodReference newCtor = new MethodReference(".ctor", Context.Import(typeof(void)));
			newCtor.DeclaringType = declaringType;
			newCtor.HasThis = true;

			foreach (ParameterDefinition parameter in originalCtor.Parameters)
			{
				newCtor.Parameters.Add(parameter);
			}

			newObj.Operand = newCtor;
		}

		private bool IsAssignmentToConcreteType(StackAnalysisResult analysisResult)
		{
			TypeReference assignmentTargetType;
			if (analysisResult.Consumer.IsNewObj() || analysisResult.Consumer.IsCall())
			{
				assignmentTargetType = analysisResult.AssignedParameter().ParameterType;
			}
			else
			{
				var assignmentTarget = analysisResult.Consumer.Operand as FieldReference;
				if (assignmentTarget != null)
				{
					assignmentTargetType = assignmentTarget.FieldType;
				}
				else
				{
					var variableDefinition = analysisResult.Consumer.Operand as VariableReference;
					if (variableDefinition != null)
					{
						assignmentTargetType = variableDefinition.VariableType;
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
			}
			return HasReplacement(assignmentTargetType.GetElementType().FullName);
		}

		private static IEnumerable<Instruction> TAEnabledCollectionInstantiations(MethodBody methodBody)
		{
			return InstrumentationUtil.Where(methodBody, delegate(Instruction candidate)
			{
				if (candidate.OpCode != OpCodes.Newobj) return false;
				MethodReference ctor = (MethodReference)candidate.Operand;
				TypeDefinition declaringType = ctor.DeclaringType.Resolve();

				return declaringType.HasGenericParameters && _collectionReplacements.ContainsKey(declaringType.FullName);
			});
		}

		static TACollectionsStep()
		{
			_collectionReplacements = new Dictionary<string, Type>();
			
			_collectionReplacements[typeof (List<>).FullName] = typeof (ActivatableList<>);
			_collectionReplacements[typeof (Dictionary<,>).FullName] = typeof (ActivatableDictionary<,>);
		}

		private static readonly IDictionary<string, Type> _collectionReplacements;
	}
}
