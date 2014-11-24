/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;

#if !SILVERLIGHT
using System;
using System.Reflection;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.CLI1.NativeQueries;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;
using Db4objects.Db4o.NativeQueries;
using Db4objects.Db4o.Tests.NativeQueries.Mocks;
#endif

namespace Db4objects.Db4o.Tests.CLI1.Inside.Query
{	
	public class QueryExpressionBuilderTestCase : ITestCase
	{
#if !SILVERLIGHT
		public class ActivatorUtil
		{
			public static void ActivateForRead(IActivatable tba)
			{
				tba.Activate(ActivationPurpose.Read);
			}
		}

		public class Item
		{
			public string name;
		}

		public class ActivatableItem : Item, IActivatable
		{
			public ActivatableItem parent;

			public void Bind(IActivator activator) {}
			public void Activate(ActivationPurpose purpose){}
			public void Activate(string str) {}

			public string NameProperty
			{
				get { Activate(ActivationPurpose.Read); return name;}
			}

			public string IndirectActivationNameProperty
			{
				get { ActivateForRead(); return name; }
			}

			public void ActivateForRead() { Activate(ActivationPurpose.Read); }
			
			public void ActivateParentForRead() { parent.Activate(ActivationPurpose.Read); }

			public void ActivateAndChangeState()
			{
				Activate(ActivationPurpose.Read);
				parent = null;
			}
		}

		public class BaseActivatableItem : Item, IActivatable
		{
			public void Bind(IActivator activator) {}
			public void Activate(ActivationPurpose purpose) {}

			protected void ActivateForRead()
			{
				Activate(ActivationPurpose.Read);
			}
		}

		public class DerivedActivatableItem : BaseActivatableItem
		{
			public string GetName()
			{
				ActivateForRead();
				return name;
			}
		}

		bool MatchWithActivate(Item item)
		{
			((IActivatable) item).Activate(ActivationPurpose.Read);
			return item.name.StartsWith("foo");
		}

		bool MatchNoSideEffectsIndirectedActivate(ActivatableItem item)
		{
			item.ActivateForRead();
			return item.name.StartsWith("foo");
		}

		bool MatchNoSideEffectsIndirectedActivateOnTypeHierarchy(DerivedActivatableItem item)
		{
			return item.GetName().StartsWith("foo");
		}

		bool MatchNoSideEffectsIndirectedStaticActivate(ActivatableItem item)
		{
			ActivatorUtil.ActivateForRead(item);
			return item.name.StartsWith("foo");
		}

		bool MatchNoSideEffectsIndirectedActivateToAnotherInstance(ActivatableItem item)
		{
			item.ActivateParentForRead();
			return item.name.StartsWith("foo");
		}

		bool MatchWithSideEffectsIndirectedActivate(ActivatableItem item)
		{
			item.ActivateAndChangeState();
			return item.name.StartsWith("foo");
		}

		bool MatchActivateableCall(ActivatableItem item)
		{
			item.Activate(ActivationPurpose.Read);
			return item.name.StartsWith("foo");
		}

		bool MatchWithActivateOnProperty(ActivatableItem item)
		{
			return item.NameProperty.StartsWith("foo");
		}

		bool MatchNoSideEffectIndirectActivationNameProperty(ActivatableItem item)
		{
			return item.IndirectActivationNameProperty.StartsWith("foo");
		}
		
		bool NotMatchWrongActivateCall(ActivatableItem item)
		{
			item.Activate("foo");
			return item.name.StartsWith("foo");
		}

		public void TestMatchNoSideEffectsIndirectedActivateAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchNoSideEffectsIndirectedActivate");
		}

		public void TestStaticHelperActivateAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchNoSideEffectsIndirectedStaticActivate");
		}

		public void TestMatchNoSideEffectsIndirectedActivateOnTypeHierarchyAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchNoSideEffectsIndirectedActivateOnTypeHierarchy");
		}

		public void TestMatchWithSideEffectsIndirectedActivateAreNotIgnored()
		{
			AssertActivatableCallsAreNotIgnored("MatchWithSideEffectsIndirectedActivate");
		}

		public void TestActivationCallOnAnotherInstanceAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchNoSideEffectsIndirectedActivateToAnotherInstance");
		}

		public void TestActivateCallsInsidePropertiesAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchWithActivateOnProperty");
		}

		public void TestNoSideEffectIndirectActivateInsidePropertiesAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchNoSideEffectIndirectActivationNameProperty");
		}

		public void TestActivateCallsAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchWithActivate");
		}

		public void TestActivateCallsOnOverridenActivateMethodsAreIgnored()
		{
			AssertActivatableCallsAreIgnored("MatchActivateableCall");
		}
		
		public void TestWrongActivateCallAreNotIngnored()
		{
			AssertActivatableCallsAreNotIgnored("NotMatchWrongActivateCall");
		}

#if !SILVERLIGHT && !CF
		public void TestCrossAssemblyActivateCallIsIgnored()
		{
			const string helperAssemblyCode = @"
								using Db4objects.Db4o.TA;
								using Db4objects.Db4o.Activation;

								public class ActivatorUtil
								{
									public static void ActivateForRead(IActivatable item)
									{
										item.Activate(ActivationPurpose.Read);
									}
								}";

			const string assemblyCode = @"
								using Db4objects.Db4o.TA;
								using Db4objects.Db4o.Activation;

								public class ActivatableItem : IActivatable
								{
									public void Bind(IActivator activator) {}
									public void Activate(ActivationPurpose purpose) {}

									public string name;
								}

								public class Predicates
								{
									public bool ByName(ActivatableItem item)
									{
										ActivatorUtil.ActivateForRead(item);
										return item.name.StartsWith(""foo"");
									}
								}";


			CompilationServices.EmitAssembly(AssemblyPathFor("ActivatorHelper.dll"), new string[] { Db4oAssemblyPath() }, helperAssemblyCode);

			string crossNQAssemblyPath = AssemblyPathFor("CrossNQ.dll");
			CompilationServices.EmitAssembly(crossNQAssemblyPath, new string[] { Db4oAssemblyPath(), AssemblyPathFor("ActivatorHelper.dll") }, assemblyCode);

			Assembly assembly = Assembly.LoadFrom(crossNQAssemblyPath);

			Type type = assembly.GetType("Predicates");
			IExpression expression = new QueryExpressionBuilder().FromMethod(type.GetMethod("ByName", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public));
			
			AssertSimpleComparisionAgainstConstant(expression, "foo");
		}

		private string Db4oAssemblyPath()
		{
			return typeof(IActivatable).Module.FullyQualifiedName;
		}

		private static string AssemblyPathFor(string assemblyName)
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyName);
		}
#endif

		private void AssertActivatableCallsAreNotIgnored(string methodName)
		{
			bool exceptionRaised = false;
			try
			{
				ExpressionFromMethod(methodName);
			}
			catch(UnsupportedPredicateException upe)
			{
				exceptionRaised = true;
			}

			Assert.IsTrue(exceptionRaised, "[" + methodName + "] : Unoptimized predicate exception not raised");
		}

		private void AssertActivatableCallsAreIgnored(string methodName)
		{
			IExpression expression = ExpressionFromMethod(methodName);
			AssertSimpleComparisionAgainstConstant(expression, "foo");
		}

		private void AssertSimpleComparisionAgainstConstant(IExpression expression, string value)
		{
			Assert.AreEqual(
				new ComparisonExpression(
					NewFieldValue(CandidateFieldRoot.Instance, "name", typeof(string)),
					new ConstValue(value),
					ComparisonOperator.StartsWith),
				expression);
		}

		public void TestNameEqualsTo()
		{
			IExpression expression = ExpressionFromPredicate(typeof(NameEqualsTo));
			Assert.AreEqual(
				new ComparisonExpression(
				NewFieldValue(CandidateFieldRoot.Instance, "name", typeof(string)),
				NewFieldValue(PredicateFieldRoot.Instance, "_name", typeof(string)),
				ComparisonOperator.ValueEquality),
				expression);
		}

		public void TestHasPreviousWithPrevious()
		{
			// candidate.HasPrevious && candidate.Previous.HasPrevious
			IExpression expression = ExpressionFromPredicate(typeof(HasPreviousWithPrevious));
			IExpression expected = 
				new AndExpression(
				new NotExpression(
				new ComparisonExpression(
				NewFieldValue(CandidateFieldRoot.Instance, "previous", typeof(Data)), 
				new ConstValue(null),
				ComparisonOperator.ValueEquality)),
				new NotExpression(
				new ComparisonExpression(
				NewFieldValue(
					NewFieldValue(CandidateFieldRoot.Instance, "previous", typeof(Data)),
					"previous",
					typeof(Data)),
				new ConstValue(null),
				ComparisonOperator.ValueEquality)));

			Assert.AreEqual(expected, expression);
		}
		
		enum MessagePriority
		{
			None,
			Low,
			Normal,
			High
		}
		
		class Message
		{
			private MessagePriority _priority;

			public MessagePriority Priority
			{
				get { return _priority;  }
				set { _priority = value;  }
			}
		}
		
		private bool MatchEnumConstrain(Message message)
		{
			return message.Priority == MessagePriority.High;
		}
		
		public void TestQueryWithEnumConstrain()
		{
			IExpression expression = ExpressionFromMethod("MatchEnumConstrain");
			IExpression expected = new ComparisonExpression(
				NewFieldValue(CandidateFieldRoot.Instance, "_priority", typeof(MessagePriority)),
				new ConstValue(MessagePriority.High),
				ComparisonOperator.ValueEquality);
			Assert.AreEqual(expected, expression);
		}
		
		private IExpression ExpressionFromMethod(string methodName)
		{
			return new QueryExpressionBuilder().FromMethod(GetType().GetMethod(methodName, BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static));
		}

		private IExpression ExpressionFromPredicate(Type type)
		{
			// XXX: move knowledge about IMethodDefinition to QueryExpressionBuilder
			return (new QueryExpressionBuilder()).FromMethod(type.GetMethod("Match"));
		}

		private FieldValue NewFieldValue(IComparisonOperandAnchor anchor, string name, Type type)
		{
			return new FieldValue(anchor,
				new MockFieldRef(name,
					new MockTypeRef(type)));
		}
#endif
	}
}
