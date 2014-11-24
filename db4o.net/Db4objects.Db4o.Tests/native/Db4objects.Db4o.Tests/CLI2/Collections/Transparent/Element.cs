/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	[Serializable]
	public class Element : AbstractCollectionElement
	{
		public Element(string name) : base(name)
		{
		}
	
		public override bool Equals(object obj) 
		{
			Element other = obj as Element;
			if (other == null || (other.GetType() != GetType()))
			{
				return false;
			}

			return _name.Equals(other._name);
		}
	
	
		public override string ToString() 
		{
			return "Element " + Name;
		}
	
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
