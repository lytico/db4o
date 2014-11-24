/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using Db4objects.Db4o.Query;
using Db4objects.Db4o;
using OManager.BusinessLayer.Config;

namespace OManager.DataLayer.Connection
{
    public class Config
    {
        public ISearchPath AssemblySearchPath
        {
            get
            {
                if (_searchPath == null)
                {
                    _searchPath = LoadSearchPath();
                }
                return _searchPath;
            }
            set { _searchPath = value; }
        }

        public static Config Instance
        {
            get { return _config; }
        }

        public string DbPath
        {
            get { return _path; }
            set { _path = value; }

        }

        public void SaveAssemblySearchPath(string path)
        {
            _path = path;
            PathContainer pathContainer = PathContainerFor(Db4oClient.OMNConnection);
            if (pathContainer.SearchPath != null)
                Db4oClient.OMNConnection.Delete(pathContainer.SearchPath);
            pathContainer.SearchPath = AssemblySearchPath;
            pathContainer.ConnectionPath = path;
            Db4oClient.OMNConnection.Store(pathContainer);
            Db4oClient.CloseRecentConnectionFile();
        }

        private ISearchPath LoadSearchPath()
        {
            ISearchPath searchPath = PathContainerFor(Db4oClient.OMNConnection).SearchPath;
            Db4oClient.CloseRecentConnectionFile();
            return searchPath;
        }

        private PathContainer PathContainerFor(IObjectContainer database)
        {

            IQuery query = database.Query();
            query.Constrain(typeof (PathContainer));
            query.Descend("_connectionPath").Constrain(_path).Contains();
            IObjectSet paths = query.Execute();
            return paths.Count == 0 ? new PathContainer(new SearchPathImpl()) : (PathContainer) paths[0];
        }

        private Config()
        {
        }

        private ISearchPath _searchPath;
        private static readonly Config _config = new Config();
        private string _path;

    }



}
