/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using OManager.BusinessLayer.Login;

namespace OManager.BusinessLayer.Config
{
	internal class PathContainer
	{
		public PathContainer(ISearchPath searchPath)
		{
			_searchPath = searchPath;
		}

		public ISearchPath SearchPath
		{
			get
			{
				return _searchPath;
			}

			set
			{
				_searchPath = value;	
			}
		}
        public string ConnectionPath
        {
            get
            {
                return _connectionPath ;
            }

            set
            {
                _connectionPath = value;
            }
        }
		private ISearchPath _searchPath;
        private string _connectionPath;
	}
}
