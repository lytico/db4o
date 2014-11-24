/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;
using Db4objects.Db4o.NativeQueries.Instrumentation;
using Db4objects.Db4o.NativeQueries.Optimization;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.NativeQueries.Instrumentation
{
	public class SODAMethodBuilder
	{
		private const bool LogBytecode = false;

		private IMethodRef descendRef;

		private IMethodRef constrainRef;

		private IMethodRef greaterRef;

		private IMethodRef smallerRef;

		private IMethodRef containsRef;

		private IMethodRef startsWithRef;

		private IMethodRef endsWithRef;

		private IMethodRef notRef;

		private IMethodRef andRef;

		private IMethodRef orRef;

		private IMethodRef identityRef;

		private readonly ITypeEditor _editor;

		private IMethodBuilder _builder;

		public static readonly string OptimizeQueryMethodName = "optimizeQuery";

		private class SODAExpressionBuilder : IExpressionVisitor
		{
			private ITypeRef predicateClass;

			public SODAExpressionBuilder(SODAMethodBuilder _enclosing, ITypeRef predicateClass
				)
			{
				this._enclosing = _enclosing;
				this.predicateClass = predicateClass;
			}

			public virtual void Visit(AndExpression expression)
			{
				expression.Left().Accept(this);
				expression.Right().Accept(this);
				this._enclosing.Invoke(this._enclosing.andRef);
			}

			public virtual void Visit(BoolConstExpression expression)
			{
				this.LoadQuery();
			}

			//throw new RuntimeException("No boolean constants expected in parsed expression tree");
			private void LoadQuery()
			{
				this._enclosing.LoadArgument(1);
			}

			public virtual void Visit(OrExpression expression)
			{
				expression.Left().Accept(this);
				expression.Right().Accept(this);
				this._enclosing.Invoke(this._enclosing.orRef);
			}

			public virtual void Visit(ComparisonExpression expression)
			{
				this.LoadQuery();
				this.Descend(this.FieldNames(expression.Left()));
				expression.Right().Accept(this.ComparisonEmitter());
				this.Constrain(expression.Op());
			}

			private void Descend(IEnumerator fieldNames)
			{
				while (fieldNames.MoveNext())
				{
					this.Descend(fieldNames.Current);
				}
			}

			private ComparisonBytecodeGeneratingVisitor ComparisonEmitter()
			{
				return new ComparisonBytecodeGeneratingVisitor(this._enclosing._builder, this.predicateClass
					);
			}

			private void Constrain(ComparisonOperator op)
			{
				this._enclosing.Invoke(this._enclosing.constrainRef);
				if (op.Equals(ComparisonOperator.ValueEquality))
				{
					return;
				}
				if (op.Equals(ComparisonOperator.ReferenceEquality))
				{
					this._enclosing.Invoke(this._enclosing.identityRef);
					return;
				}
				if (op.Equals(ComparisonOperator.Greater))
				{
					this._enclosing.Invoke(this._enclosing.greaterRef);
					return;
				}
				if (op.Equals(ComparisonOperator.Smaller))
				{
					this._enclosing.Invoke(this._enclosing.smallerRef);
					return;
				}
				if (op.Equals(ComparisonOperator.Contains))
				{
					this._enclosing.Invoke(this._enclosing.containsRef);
					return;
				}
				if (op.Equals(ComparisonOperator.StartsWith))
				{
					this._enclosing.Ldc(1);
					this._enclosing.Invoke(this._enclosing.startsWithRef);
					return;
				}
				if (op.Equals(ComparisonOperator.EndsWith))
				{
					this._enclosing.Ldc(1);
					this._enclosing.Invoke(this._enclosing.endsWithRef);
					return;
				}
				throw new Exception("Cannot interpret constraint: " + op);
			}

			private void Descend(object fieldName)
			{
				this._enclosing.Ldc(fieldName);
				this._enclosing.Invoke(this._enclosing.descendRef);
			}

			public virtual void Visit(NotExpression expression)
			{
				expression.Expr().Accept(this);
				this._enclosing.Invoke(this._enclosing.notRef);
			}

			private IEnumerator FieldNames(FieldValue fieldValue)
			{
				Collection4 coll = new Collection4();
				IComparisonOperand curOp = fieldValue;
				while (curOp is FieldValue)
				{
					FieldValue curField = (FieldValue)curOp;
					coll.Prepend(curField.FieldName());
					curOp = curField.Parent();
				}
				return coll.GetEnumerator();
			}

			private readonly SODAMethodBuilder _enclosing;
		}

		public SODAMethodBuilder(ITypeEditor editor)
		{
			_editor = editor;
			BuildMethodReferences();
		}

		public virtual void InjectOptimization(IExpression expr)
		{
			_editor.AddInterface(TypeRef(typeof(IDb4oEnhancedFilter)));
			_builder = _editor.NewPublicMethod(PlatformName(OptimizeQueryMethodName), TypeRef
				(typeof(void)), new ITypeRef[] { TypeRef(typeof(IQuery)) });
			ITypeRef predicateClass = _editor.Type;
			expr.Accept(new SODAMethodBuilder.SODAExpressionBuilder(this, predicateClass));
			_builder.Pop();
			_builder.EndMethod();
		}

		private ITypeRef TypeRef(Type type)
		{
			return _editor.References.ForType(type);
		}

		private string PlatformName(string name)
		{
			return NativeQueriesPlatform.ToPlatformName(name);
		}

		private void LoadArgument(int index)
		{
			_builder.LoadArgument(index);
		}

		private void Invoke(IMethodRef method)
		{
			_builder.Invoke(method, CallingConvention.Interface);
		}

		private void Ldc(object value)
		{
			_builder.Ldc(value);
		}

		private void BuildMethodReferences()
		{
			descendRef = MethodRef(typeof(IQuery), "descend", new Type[] { typeof(string) });
			constrainRef = MethodRef(typeof(IQuery), "constrain", new Type[] { typeof(object)
				 });
			greaterRef = MethodRef(typeof(IConstraint), "greater", new Type[] {  });
			smallerRef = MethodRef(typeof(IConstraint), "smaller", new Type[] {  });
			containsRef = MethodRef(typeof(IConstraint), "contains", new Type[] {  });
			startsWithRef = MethodRef(typeof(IConstraint), "startsWith", new Type[] { typeof(
				bool) });
			endsWithRef = MethodRef(typeof(IConstraint), "endsWith", new Type[] { typeof(bool
				) });
			notRef = MethodRef(typeof(IConstraint), "not", new Type[] {  });
			andRef = MethodRef(typeof(IConstraint), "and", new Type[] { typeof(IConstraint) }
				);
			orRef = MethodRef(typeof(IConstraint), "or", new Type[] { typeof(IConstraint) });
			identityRef = MethodRef(typeof(IConstraint), "identity", new Type[] {  });
		}

		private IMethodRef MethodRef(Type parent, string name, Type[] args)
		{
			try
			{
				return _editor.References.ForMethod(parent.GetMethod(PlatformName(name), args));
			}
			catch (Exception e)
			{
				throw new InstrumentationException(e);
			}
		}
	}
}
