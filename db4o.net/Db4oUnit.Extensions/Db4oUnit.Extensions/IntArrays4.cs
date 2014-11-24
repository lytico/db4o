/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4oUnit.Extensions
{
	public class IntArrays4
	{
		public static int[] Fill(int[] array, int value)
		{
			for (int i = 0; i < array.Length; ++i)
			{
				array[i] = value;
			}
			return array;
		}

		public static int[] Concat(int[] a, int[] b)
		{
			int[] array = new int[a.Length + b.Length];
			System.Array.Copy(a, 0, array, 0, a.Length);
			System.Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		public static int Occurences(int[] values, int value)
		{
			int count = 0;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] == value)
				{
					count++;
				}
			}
			return count;
		}

		public static int[] Clone(int[] bars)
		{
			int[] array = new int[bars.Length];
			System.Array.Copy(bars, 0, array, 0, bars.Length);
			return array;
		}

		public static object[] ToObjectArray(int[] values)
		{
			object[] ret = new object[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				ret[i] = values[i];
			}
			return ret;
		}

		public static IEnumerator NewIterator(int[] values)
		{
			return new ArrayIterator4(ToObjectArray(values));
		}
	}
}
