/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Util
{
	public class Db4oUnitTestUtil
	{
		public static Type[] MergeClasses(Type[] classesLeft, Type[] classesRight)
		{
			if (classesLeft == null || classesLeft.Length == 0)
			{
				return classesRight;
			}
			if (classesRight == null || classesRight.Length == 0)
			{
				return classesLeft;
			}
			Type[] merged = new Type[classesLeft.Length + classesRight.Length];
			System.Array.Copy(classesLeft, 0, merged, 0, classesLeft.Length);
			System.Array.Copy(classesRight, 0, merged, classesLeft.Length, classesRight.Length
				);
			return merged;
		}

		private Db4oUnitTestUtil()
		{
		}
	}
}
