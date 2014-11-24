/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Diagnostics;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4oTool.Core;
using Mono.Cecil;

namespace Db4oTool.NQ
{
	public class PredicateOptimizer : AbstractOptimizer
	{
		protected override void ProcessType(TypeDefinition type)
		{
			if (IsPredicateClass(type))
			{
				InstrumentPredicateClass(type);
			}
		}

		private void InstrumentPredicateClass(TypeDefinition type)
		{
			++_processedCount;
			
			MethodDefinition match = GetMatchMethod(type);
			IExpression e = GetExpression(match);
			if (null == e) return;

			OptimizePredicate(type, match, e);
		}

		private static MethodDefinition GetMatchMethod(TypeDefinition type)
		{
			return CecilReflector.GetMethod(type, "Match");
		}

		private static bool IsPredicateClass(TypeReference typeRef)
		{
			TypeDefinition type = typeRef as TypeDefinition;
			if (null == type) return false;
			TypeReference baseType = type.BaseType;
			if (null == baseType) return false;
			if (typeof(Db4objects.Db4o.Query.Predicate).FullName == baseType.FullName) return true;
			return IsPredicateClass(baseType);
		}

		protected override string TargetName(int processedCount)
		{
			return string.Format("predicate class{0}", processedCount == 1 ? "" : "es");
		}
	}
}
