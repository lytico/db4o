/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;

namespace OManager.BusinessLayer.Config
{
	[Serializable]
	class SearchPathImpl : ISearchPath
	{
		public bool Add(string path)
		{
			if (_paths.Contains(path))
			{
				return false;
			}

			_paths.Add(path);
			return true;
		}

		public void Remove(string path)
		{
			_paths.Remove(path);
		}

		public IEnumerable<string> Paths
		{
			get
			{
				return _paths;
			}
		}

		private readonly IList<string> _paths = new List<string>();
	}
}
