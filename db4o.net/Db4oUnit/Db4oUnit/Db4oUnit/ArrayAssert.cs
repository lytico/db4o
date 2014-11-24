/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4oUnit
{
	public partial class ArrayAssert
	{
		public static void Contains(long[] array, long expected)
		{
			if (-1 != IndexOf(array, expected))
			{
				return;
			}
			Assert.Fail("Expecting '" + expected + "'.");
		}

		public static void ContainsByIdentity(object[] array, object[] expected)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				if (-1 == Arrays4.IndexOfIdentity(array, expected[i]))
				{
					Assert.Fail("Expecting contains '" + expected[i] + "'.");
				}
			}
		}

		public static void ContainsByEquality(object[] array, object[] expected)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				if (-1 == Arrays4.IndexOfEquals(array, expected[i]))
				{
					Assert.Fail("Expecting contains '" + expected[i] + "'.");
				}
			}
		}

		public static void AreEqual(object[] expected, object[] actual)
		{
			AreEqualImpl(expected, actual);
		}

		public static void AreEqual(string[] expected, string[] actual)
		{
			// JDK 1.1 needs the conversion
			AreEqualImpl(StringArrayToObjectArray(expected), StringArrayToObjectArray(actual)
				);
		}

		private static object[] StringArrayToObjectArray(string[] expected)
		{
			object[] expectedAsObject = new object[expected.Length];
			System.Array.Copy(expected, 0, expectedAsObject, 0, expected.Length);
			return expectedAsObject;
		}

		private static string IndexMessage(int i)
		{
			return "expected[" + i + "]";
		}

		public static void AreEqual(byte[] expected, byte[] actual)
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
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}

		public static void AreNotEqual(byte[] expected, byte[] actual)
		{
			Assert.AreNotSame(expected, actual);
			for (int i = 0; i < expected.Length; i++)
			{
				if (expected[i] != actual[i])
				{
					return;
				}
			}
			Assert.IsTrue(false);
		}

		public static void AreEqual(int[] expected, int[] actual)
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
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}

		public static void AreEqual(long[] expected, long[] actual)
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
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}

		public static void AreEqual(float[] expected, float[] actual)
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
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}

		public static void AreEqual(double[] expected, double[] actual)
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
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}

		public static void AreEqual(char[] expected, char[] actual)
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
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i], IndexMessage(i));
			}
		}

		private static int IndexOf(long[] array, long expected)
		{
			for (int i = 0; i < array.Length; ++i)
			{
				if (expected == array[i])
				{
					return i;
				}
			}
			return -1;
		}
	}
}
