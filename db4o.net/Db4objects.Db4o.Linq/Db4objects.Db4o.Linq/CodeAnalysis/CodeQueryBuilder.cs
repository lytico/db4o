/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

#if CF || SILVERLIGHT

using System;
using Cecil.FlowAnalysis.CodeStructure;
using Mono.Cecil;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class CodeQueryBuilder : AbstractCodeStructureVisitor
	{
		private readonly QueryBuilderRecorder _recorder;

		public CodeQueryBuilder(QueryBuilderRecorder recorder)
		{
			_recorder = recorder;
		}

		public override void Visit(ArgumentReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(AssignExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(BinaryExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(CastExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(FieldReferenceExpression node)
		{
            Type descendingEnumType = ResolveDescendingEnumType(node);
            _recorder.Add(
                ctx =>
                    {
                        ctx.Descend(node.Field.Name);
                        ctx.PushDescendigFieldEnumType(descendingEnumType);
                    });
		}

		public override void Visit(LiteralExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(MethodInvocationExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(MethodReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(PropertyReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(ThisReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(UnaryExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(VariableReferenceExpression node)
		{
			CannotOptimize(node);
		}

		private static void CannotOptimize(Expression expression)
		{
			throw new QueryOptimizationException(ExpressionPrinter.ToString(expression));
		}

        private static Type ResolveDescendingEnumType(FieldReferenceExpression node)
        {
			var type = ResolveType(node.Field.FieldType);
			if (type == null) return null;
			if (!type.IsEnum) return null;

			return type;
        }

		private static Type ResolveType(TypeReference type)
		{
			var assemblyName = type.Module.Assembly.Name.FullName;
			var assembly = System.Reflection.Assembly.Load(assemblyName);
			if (assembly == null) return null;

			return assembly.GetType(NormalizeTypeName(type));
		}

		private static string NormalizeTypeName(TypeReference type)
		{
			return type.FullName.Replace('/', '+');
		}
	}
}

#endif
