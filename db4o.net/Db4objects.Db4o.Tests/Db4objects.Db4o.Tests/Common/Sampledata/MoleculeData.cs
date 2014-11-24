/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Sampledata;

namespace Db4objects.Db4o.Tests.Common.Sampledata
{
	public class MoleculeData : AtomData
	{
		public MoleculeData()
		{
		}

		public MoleculeData(AtomData child) : base(child)
		{
		}

		public MoleculeData(string name) : base(name)
		{
		}

		public MoleculeData(AtomData child, string name) : base(child, name)
		{
		}

		public override bool Equals(object obj)
		{
			if (obj is Db4objects.Db4o.Tests.Common.Sampledata.MoleculeData)
			{
				return base.Equals(obj);
			}
			return false;
		}

		public override string ToString()
		{
			string str = "Molecule(" + name + ")";
			if (child != null)
			{
				return str + "." + child.ToString();
			}
			return str;
		}
	}
}
