/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Bench.Crud
{
	public class Item
	{
		private static readonly string Load = "LOAD__________________________";

		private static readonly string Update = "ccccc";

		public string _string;

		public Item(string str)
		{
			_string = str;
		}

		public static object NewItem(int i)
		{
			return new Db4objects.Db4o.Bench.Crud.Item(Load + i);
		}

		public virtual void Change()
		{
			_string = _string + Update;
		}
	}
}
