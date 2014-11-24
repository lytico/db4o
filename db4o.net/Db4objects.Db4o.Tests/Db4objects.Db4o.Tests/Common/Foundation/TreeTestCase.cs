/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class TreeTestCase : ITestCase
	{
		private const int Count = 21;

		public virtual void TestTraversalWithStartingPointEmpty()
		{
			Tree.Traverse(null, new TreeInt(5), new _ICancellableVisitor4_18());
		}

		private sealed class _ICancellableVisitor4_18 : ICancellableVisitor4
		{
			public _ICancellableVisitor4_18()
			{
			}

			public bool Visit(object node)
			{
				return true;
			}
		}

		public virtual void TestCancelledTraversalWithStartingPointNotInTheTree()
		{
			IntByRef visits = new IntByRef();
			TreeInt tree = CreateTree();
			Tree.Traverse(tree, new TreeInt(5), new _ICancellableVisitor4_28(visits));
			Assert.AreEqual(1, visits.value);
		}

		private sealed class _ICancellableVisitor4_28 : ICancellableVisitor4
		{
			public _ICancellableVisitor4_28(IntByRef visits)
			{
				this.visits = visits;
			}

			public bool Visit(object node)
			{
				visits.value++;
				Assert.AreEqual(new TreeInt(6), ((TreeInt)node));
				return false;
			}

			private readonly IntByRef visits;
		}

		public virtual void TestCancelledTraversalWithStartingPointInTheTree()
		{
			IntByRef visits = new IntByRef();
			TreeInt tree = CreateTree();
			Tree.Traverse(tree, new TreeInt(6), new _ICancellableVisitor4_41(visits));
			Assert.AreEqual(1, visits.value);
		}

		private sealed class _ICancellableVisitor4_41 : ICancellableVisitor4
		{
			public _ICancellableVisitor4_41(IntByRef visits)
			{
				this.visits = visits;
			}

			public bool Visit(object node)
			{
				visits.value++;
				Assert.AreEqual(new TreeInt(6), ((TreeInt)node));
				return false;
			}

			private readonly IntByRef visits;
		}

		public virtual void TestUnCancelledTraversalWithStartingPointNotInTheTree()
		{
			IList actual = new ArrayList();
			TreeInt tree = CreateTree();
			Tree.Traverse(tree, new TreeInt(5), new _ICancellableVisitor4_54(actual));
			IteratorAssert.AreEqual(CreateList(6).GetEnumerator(), actual.GetEnumerator());
		}

		private sealed class _ICancellableVisitor4_54 : ICancellableVisitor4
		{
			public _ICancellableVisitor4_54(IList actual)
			{
				this.actual = actual;
			}

			public bool Visit(object node)
			{
				actual.Add(((TreeInt)node));
				return true;
			}

			private readonly IList actual;
		}

		public virtual void TestUnCancelledTraversalWithStartingPointInTheTree()
		{
			IList actual = new ArrayList();
			TreeInt tree = CreateTree();
			Tree.Traverse(tree, new TreeInt(6), new _ICancellableVisitor4_66(actual));
			IteratorAssert.AreEqual(CreateList(6).GetEnumerator(), actual.GetEnumerator());
		}

		private sealed class _ICancellableVisitor4_66 : ICancellableVisitor4
		{
			public _ICancellableVisitor4_66(IList actual)
			{
				this.actual = actual;
			}

			public bool Visit(object node)
			{
				actual.Add(((TreeInt)node));
				return true;
			}

			private readonly IList actual;
		}

		private IList CreateList(int start)
		{
			IList expected = new ArrayList();
			TreeInt expectedTree = CreateTree(start);
			Tree.Traverse(expectedTree, new _IVisitor4_79(expected));
			return expected;
		}

		private sealed class _IVisitor4_79 : IVisitor4
		{
			public _IVisitor4_79(IList expected)
			{
				this.expected = expected;
			}

			public void Visit(object node)
			{
				expected.Add(((TreeInt)node));
			}

			private readonly IList expected;
		}

		private TreeInt CreateTree()
		{
			return CreateTree(0);
		}

		private TreeInt CreateTree(int start)
		{
			TreeInt tree = null;
			for (int i = start; i < Count; i += 3)
			{
				tree = ((TreeInt)Tree.Add(tree, new TreeInt(i)));
			}
			return tree;
		}
	}
}
