/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public class NewPilot
	{
		internal string name;

		internal int points;

		internal int[] arr;

		public NewPilot()
		{
		}

		public NewPilot(string name, int points, int[] arr)
		{
			this.name = name;
			this.points = points;
			this.arr = arr;
		}

		public virtual int[] GetArr()
		{
			return arr;
		}

		public virtual void SetArr(int[] arr)
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

		public virtual int GetPoints()
		{
			return points;
		}

		public virtual void SetPoints(int points)
		{
			this.points = points;
		}

		public override string ToString()
		{
			return name + "/" + points;
		}
	}
}
