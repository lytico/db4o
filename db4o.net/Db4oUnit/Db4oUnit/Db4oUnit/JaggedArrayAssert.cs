/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;

namespace Db4oUnit
{
	public class JaggedArrayAssert
	{
		public static void AreEqual(object[][] expected, object[][] actual)
		{
			if (expected == actual)
			{
				return;
			}
			if (expected == null || actual == null)
			{
				Assert.AreSame(expected, actual);
			}
			Assert.AreEqual(expected.Length, actual.Length);
			Assert.AreSame(expected.GetType(), actual.GetType());
			for (int i = 0; i < expected.Length; i++)
			{
				ArrayAssert.AreEqual(expected[i], actual[i]);
			}
		}

		public static void AreEqual(int[][] expected, int[][] actual)
		{
			if (expected == actual)
			{
				return;
			}
			if (expected == null || actual == null)
			{
				Assert.AreSame(expected, actual);
			}
			Assert.AreEqual(expected.Length, actual.Length);
			Assert.AreSame(expected.GetType(), actual.GetType());
			for (int i = 0; i < expected.Length; i++)
			{
				ArrayAssert.AreEqual(expected[i], actual[i]);
			}
		}
	}
}
