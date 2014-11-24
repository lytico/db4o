/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Sharpen;

namespace Db4oUnit.Extensions
{
	public abstract class ComposibleTestSuite : Db4oTestSuite
	{
		protected Type[] ComposeTests(Type[] testCases)
		{
			return Concat(testCases, ComposeWith());
		}

		protected virtual Type[] ComposeWith()
		{
			return new Type[0];
		}

		public static Type[] Concat(Type[] testCases, Type[] otherTests)
		{
			Type[] result = new Type[otherTests.Length + testCases.Length];
			System.Array.Copy(testCases, 0, result, 0, testCases.Length);
			System.Array.Copy(otherTests, 0, result, testCases.Length, otherTests.Length);
			return result;
		}
	}
}
