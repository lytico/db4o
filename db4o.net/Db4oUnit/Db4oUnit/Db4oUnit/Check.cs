/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4oUnit
{
	/// <summary>
	/// Utility class to enable the reuse of object comparison and checking
	/// methods without asserting.
	/// </summary>
	/// <remarks>
	/// Utility class to enable the reuse of object comparison and checking
	/// methods without asserting.
	/// </remarks>
	public class Check
	{
		public static bool ObjectsAreEqual(object expected, object actual)
		{
			return expected == actual || (expected != null && actual != null && expected.Equals
				(actual));
		}

		public static bool ArraysAreEqual(object[] expected, object[] actual)
		{
			if (expected == actual)
			{
				return true;
			}
			if (expected == null || actual == null)
			{
				return false;
			}
			if (expected.Length != actual.Length)
			{
				return false;
			}
			for (int i = 0; i < expected.Length; i++)
			{
				if (!ObjectsAreEqual(expected[i], actual[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
