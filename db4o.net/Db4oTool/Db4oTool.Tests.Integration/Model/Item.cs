/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
namespace Db4oTool.Tests.Integration.Model
{
	public class Item
	{
		private string _name;

		public Item(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}

		public override bool Equals(object obj)
		{
			Item other = obj as Item;
			if (other == null) return false;

			return _name.CompareTo(other._name) == 0;
		}

		public override int GetHashCode()
		{
			return _name.GetHashCode();
		}

		public override string ToString()
		{
			return "Item: " + _name;
		}
	}
}
