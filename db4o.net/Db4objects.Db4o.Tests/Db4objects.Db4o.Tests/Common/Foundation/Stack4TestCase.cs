/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Stack4TestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Stack4TestCase)).Run();
		}

		public virtual void TestPushPop()
		{
			Stack4 stack = new Stack4();
			AssertEmpty(stack);
			stack.Push("a");
			stack.Push("b");
			stack.Push("c");
			Assert.IsFalse(stack.IsEmpty());
			Assert.AreEqual("c", stack.Peek());
			Assert.AreEqual("c", stack.Peek());
			Assert.AreEqual("c", stack.Pop());
			Assert.AreEqual("b", stack.Pop());
			Assert.AreEqual("a", stack.Peek());
			Assert.AreEqual("a", stack.Pop());
			AssertEmpty(stack);
		}

		private void AssertEmpty(Stack4 stack)
		{
			Assert.IsTrue(stack.IsEmpty());
			Assert.IsNull(stack.Peek());
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_35(stack));
		}

		private sealed class _ICodeBlock_35 : ICodeBlock
		{
			public _ICodeBlock_35(Stack4 stack)
			{
				this.stack = stack;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				stack.Pop();
			}

			private readonly Stack4 stack;
		}
	}
}
