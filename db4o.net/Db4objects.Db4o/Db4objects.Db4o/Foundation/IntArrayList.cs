/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class IntArrayList : IEnumerable
	{
		protected int[] i_content;

		private int i_count;

		public IntArrayList() : this(10)
		{
		}

		public IntArrayList(int initialSize)
		{
			i_content = new int[initialSize];
		}

		public virtual void Add(int a_value)
		{
			EnsureCapacity();
			i_content[i_count++] = a_value;
		}

		public virtual void Add(int index, int a_value)
		{
			EnsureCapacity();
			System.Array.Copy(i_content, index, i_content, index + 1, i_count - index);
			i_content[index] = a_value;
			i_count++;
		}

		private void EnsureCapacity()
		{
			if (i_count >= i_content.Length)
			{
				int inc = i_content.Length / 2;
				if (inc < 10)
				{
					inc = 10;
				}
				int[] temp = new int[i_content.Length + inc];
				System.Array.Copy(i_content, 0, temp, 0, i_content.Length);
				i_content = temp;
			}
		}

		public virtual int IndexOf(int a_value)
		{
			for (int i = 0; i < i_count; i++)
			{
				if (i_content[i] == a_value)
				{
					return i;
				}
			}
			return -1;
		}

		public virtual int Size()
		{
			return i_count;
		}

		public virtual long[] AsLong()
		{
			long[] longs = new long[i_count];
			for (int i = 0; i < i_count; i++)
			{
				longs[i] = i_content[i];
			}
			return longs;
		}

		public virtual IIntIterator4 IntIterator()
		{
			return IntIterators.ForInts(i_content, i_count);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return IntIterator();
		}

		public virtual int Get(int index)
		{
			return i_content[index];
		}

		public virtual void Swap(int left, int right)
		{
			if (left != right)
			{
				int swap = i_content[left];
				i_content[left] = i_content[right];
				i_content[right] = swap;
			}
		}
	}
}
