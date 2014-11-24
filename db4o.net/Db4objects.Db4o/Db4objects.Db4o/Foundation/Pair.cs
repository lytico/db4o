/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	public class Pair
	{
		public static Db4objects.Db4o.Foundation.Pair Of(object first, object second)
		{
			return new Db4objects.Db4o.Foundation.Pair(first, second);
		}

		public object first;

		public object second;

		public Pair(object first, object second)
		{
			this.first = first;
			this.second = second;
		}

		public override string ToString()
		{
			return "Pair.of(" + first + ", " + second + ")";
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			result = prime * result + ((first == null) ? 0 : first.GetHashCode());
			result = prime * result + ((second == null) ? 0 : second.GetHashCode());
			return result;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			Db4objects.Db4o.Foundation.Pair other = (Db4objects.Db4o.Foundation.Pair)obj;
			if (first == null)
			{
				if (other.first != null)
				{
					return false;
				}
			}
			else
			{
				if (!first.Equals(other.first))
				{
					return false;
				}
			}
			if (second == null)
			{
				if (other.second != null)
				{
					return false;
				}
			}
			else
			{
				if (!second.Equals(other.second))
				{
					return false;
				}
			}
			return true;
		}
	}
}
