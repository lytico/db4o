/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
using System.Collections.Generic;

namespace Db4oTool.Tests.Integration.Model
{
	public class CollectionHolder<T>
	{
		private string _name;
		private IList<T> _list;
		private IDictionary<string, T> _dictionary;

		public CollectionHolder()
		{
			// db4o creation constructor
		}

		public CollectionHolder(string name, params T[] items)
		{
			_name = name;
			_list = new List<T>(items);
			_dictionary = NewDictionary(items);
		}

		public IList<T> List
		{
			get { return _list; }
		}

		public IDictionary<string, T> Dictionary
		{
			get { return _dictionary; }
		}

		public override string ToString()
		{
			return _name + ": " + _list + "";
		}

		private static IDictionary<string, T> NewDictionary(T[] items)
		{
			IDictionary<string, T> dictionary = new Dictionary<string, T>();
			foreach (T item in items)
			{
				dictionary[item.ToString()] = item;
			}
			return dictionary;
		}

	}
}
