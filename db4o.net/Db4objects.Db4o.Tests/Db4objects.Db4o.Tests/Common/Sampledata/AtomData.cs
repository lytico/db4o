/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Sampledata
{
	public class AtomData
	{
		public Db4objects.Db4o.Tests.Common.Sampledata.AtomData child;

		public string name;

		public AtomData()
		{
		}

		public AtomData(Db4objects.Db4o.Tests.Common.Sampledata.AtomData child)
		{
			this.child = child;
		}

		public AtomData(string name)
		{
			this.name = name;
		}

		public AtomData(Db4objects.Db4o.Tests.Common.Sampledata.AtomData child, string name
			) : this(child)
		{
			this.name = name;
		}

		public override int GetHashCode()
		{
			return this.name != null ? this.name.GetHashCode() : 0;
		}

		public override bool Equals(object obj)
		{
			if (obj is Db4objects.Db4o.Tests.Common.Sampledata.AtomData)
			{
				Db4objects.Db4o.Tests.Common.Sampledata.AtomData other = (Db4objects.Db4o.Tests.Common.Sampledata.AtomData
					)obj;
				if (name == null)
				{
					if (other.name != null)
					{
						return false;
					}
				}
				else
				{
					if (!name.Equals(other.name))
					{
						return false;
					}
				}
				if (child != null)
				{
					return child.Equals(other.child);
				}
				return other.child == null;
			}
			return false;
		}

		public override string ToString()
		{
			string str = "Atom(" + name + ")";
			if (child != null)
			{
				return str + "." + child.ToString();
			}
			return str;
		}
	}
}
