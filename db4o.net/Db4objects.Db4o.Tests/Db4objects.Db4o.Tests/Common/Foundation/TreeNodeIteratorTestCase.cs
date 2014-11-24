/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class TreeNodeIteratorTestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(TreeNodeIteratorTestCase)).Run();
		}

		private static int[] Values = new int[] { 1, 3, 5, 7, 9, 10, 11, 13, 24, 76 };

		public virtual void TestIterate()
		{
			for (int i = 1; i <= Values.Length; i++)
			{
				AssertIterateValues(Values, i);
			}
		}

		public virtual void TestMoveNextAfterCompletion()
		{
			IEnumerator i = new TreeNodeIterator(CreateTree(Values));
			while (i.MoveNext())
			{
			}
			Assert.IsFalse(i.MoveNext());
		}

		private void AssertIterateValues(int[] values, int count)
		{
			int[] testValues = new int[count];
			System.Array.Copy(values, 0, testValues, 0, count);
			AssertIterateValues(testValues);
		}

		private void AssertIterateValues(int[] values)
		{
			Tree tree = CreateTree(Values);
			IEnumerator i = new TreeNodeIterator(tree);
			tree.Traverse(new _IVisitor4_42(i));
		}

		private sealed class _IVisitor4_42 : IVisitor4
		{
			public _IVisitor4_42(IEnumerator i)
			{
				this.i = i;
			}

			public void Visit(object obj)
			{
				i.MoveNext();
				Assert.AreSame(obj, i.Current);
			}

			private readonly IEnumerator i;
		}

		private Tree CreateTree(int[] values)
		{
			Tree tree = new TreeInt(values[0]);
			for (int i = 1; i < values.Length; i++)
			{
				tree = tree.Add(new TreeInt(values[i]));
			}
			return tree;
		}

		public virtual void TestEmpty()
		{
			IEnumerator i = new TreeNodeIterator(null);
			Assert.IsFalse(i.MoveNext());
		}
	}
}
