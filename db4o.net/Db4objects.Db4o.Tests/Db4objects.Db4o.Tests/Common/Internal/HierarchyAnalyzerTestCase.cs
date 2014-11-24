/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class HierarchyAnalyzerTestCase : AbstractDb4oTestCase
	{
		public class A
		{
		}

		public class BA : HierarchyAnalyzerTestCase.A
		{
		}

		public class CBA : HierarchyAnalyzerTestCase.BA
		{
		}

		public class DA : HierarchyAnalyzerTestCase.A
		{
		}

		public class E
		{
		}

		public virtual void TestRemovedImmediateSuperclass()
		{
			AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.DA), typeof(HierarchyAnalyzerTestCase.CBA
				), new HierarchyAnalyzer.Diff[] { new HierarchyAnalyzer.Removed(ProduceClassMetadata
				(typeof(HierarchyAnalyzerTestCase.BA))), new HierarchyAnalyzer.Same(ProduceClassMetadata
				(typeof(HierarchyAnalyzerTestCase.A))) });
		}

		public virtual void TestRemoveTopLevelSuperclass()
		{
			AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.E), typeof(HierarchyAnalyzerTestCase.BA
				), new HierarchyAnalyzer.Diff[] { new HierarchyAnalyzer.Removed(ProduceClassMetadata
				(typeof(HierarchyAnalyzerTestCase.A))) });
		}

		public virtual void TestAddedImmediateSuperClass()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_50(this));
		}

		private sealed class _ICodeBlock_50 : ICodeBlock
		{
			public _ICodeBlock_50(HierarchyAnalyzerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.CBA), typeof(HierarchyAnalyzerTestCase.DA
					), new HierarchyAnalyzer.Diff[] {  });
			}

			private readonly HierarchyAnalyzerTestCase _enclosing;
		}

		public virtual void TestAddedTopLevelSuperClass()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_58(this));
		}

		private sealed class _ICodeBlock_58 : ICodeBlock
		{
			public _ICodeBlock_58(HierarchyAnalyzerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.BA), typeof(HierarchyAnalyzerTestCase.E
					), new HierarchyAnalyzer.Diff[] {  });
			}

			private readonly HierarchyAnalyzerTestCase _enclosing;
		}

		private void AssertDiffBetween(Type runtimeClass, Type storedClass, HierarchyAnalyzer.Diff
			[] expectedDiff)
		{
			ClassMetadata classMetadata = ProduceClassMetadata(storedClass);
			IReflectClass reflectClass = ReflectClass(runtimeClass);
			IList ancestors = new HierarchyAnalyzer(classMetadata, reflectClass).Analyze();
			AssertDiff(ancestors, expectedDiff);
		}

		private ClassMetadata ProduceClassMetadata(Type storedClass)
		{
			return Container().ProduceClassMetadata(ReflectClass(storedClass));
		}

		private void AssertDiff(IList actual, HierarchyAnalyzer.Diff[] expected)
		{
			Iterator4Assert.AreEqual(Iterators.Iterate(expected), Iterators.Iterator(actual));
		}
	}
}
