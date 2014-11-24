/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	internal class ByExample
	{
		public string Name;
		public ByExample Child;

		public ByExample(string name)
		{
			Name = name;
		}

		public ByExample(string name, ByExample child) : this(name)
		{
			Child = child;
		}

		public override string ToString()
		{
			return string.Format("ByExample(Name='{0}', Child=[{1}])", Name, Child);
		}

		public override bool Equals(object obj)
		{
			ByExample other = obj as ByExample;
			if (obj == null) return false;

			if (other.Name != Name)
				return false;

			if (Child == null)
				return other.Child == null;

			return  other.Child.Equals(Child);
		}
	}
}
