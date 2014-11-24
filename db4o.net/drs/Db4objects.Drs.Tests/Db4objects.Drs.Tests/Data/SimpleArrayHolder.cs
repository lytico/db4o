/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;
using Sharpen;

namespace Db4objects.Drs.Tests.Data
{
	public class SimpleArrayHolder
	{
		private string name;

		private SimpleArrayContent[] arr;

		public SimpleArrayHolder()
		{
		}

		public SimpleArrayHolder(string name)
		{
			this.name = name;
		}

		public virtual SimpleArrayContent[] GetArr()
		{
			return arr;
		}

		public virtual void SetArr(SimpleArrayContent[] arr)
		{
			this.arr = arr;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual void Add(SimpleArrayContent sac)
		{
			if (arr == null)
			{
				arr = new SimpleArrayContent[] { sac };
				return;
			}
			SimpleArrayContent[] temp = arr;
			arr = new SimpleArrayContent[temp.Length + 1];
			System.Array.Copy(temp, 0, arr, 0, temp.Length);
			arr[temp.Length] = sac;
		}
	}
}
