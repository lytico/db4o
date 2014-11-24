/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class Atom
	{
		public Db4objects.Db4o.Tests.Common.Assorted.Atom child;

		public string name;

		public Atom()
		{
		}

		public Atom(Db4objects.Db4o.Tests.Common.Assorted.Atom child)
		{
			this.child = child;
		}

		public Atom(string name)
		{
			this.name = name;
		}

		public Atom(Db4objects.Db4o.Tests.Common.Assorted.Atom child, string name) : this
			(child)
		{
			this.name = name;
		}

		public virtual int CompareTo(object obj)
		{
			return 0;
		}

		public override bool Equals(object obj)
		{
			if (obj is Db4objects.Db4o.Tests.Common.Assorted.Atom)
			{
				Db4objects.Db4o.Tests.Common.Assorted.Atom other = (Db4objects.Db4o.Tests.Common.Assorted.Atom
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
