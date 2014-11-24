using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Migration
{
#if !CF && !SILVERLIGHT
	internal class LegacyAdapterEmitter
	{
		private string _legacyAssembly;
		private string _version;

		public LegacyAdapterEmitter(string legacyAssembly, string version)
		{
			_legacyAssembly = legacyAssembly;
			_version = version;
		}

		public void Emit(string fname)
		{	
			CompilationServices.EmitAssembly(fname, new string[] {_legacyAssembly}, GetCode());
		}	

		public string GetCode()
		{
			if (_version.StartsWith("5")) return PascalCaseAdapter;
			return CamelCaseAdapter;
		}

		#region PascalCaseAdapter
		string PascalCaseAdapter
		{
			get
			{
				return CommonCode + @"

namespace Db4objects.Db4o.Config
{
   	public interface IConfiguration
	{
    }   
}

namespace Db4objects.Db4o.Query
{
    public interface IQuery
    {
    }   
}

namespace Db4objects.Db4o
{
	using Db4objects.Db4o.Ext;
    using Db4objects.Db4o.Config;
    using Db4objects.Db4o.Query;
    
	public class Db4oFactory
	{
		public static IObjectContainer OpenFile(string fname)
		{
			return new ObjectContainerAdapter(com.db4o.Db4o.OpenFile(fname));
		}

        public static IConfiguration Configure()
        {
            return null;
        }

		public static string Version()
		{
			return com.db4o.Db4o.Version();
		}
	}

	class ObjectContainerAdapter : IExtObjectContainer
	{
		private readonly ObjectContainer _container;

		public ObjectContainerAdapter(ObjectContainer container)
		{
			_container = container;
		}

		public void Set(object o)
		{
			_container.Set(o);
		}
		
        public void Delete(object obj)
        {
            _container.Delete(obj);
        }

		public bool Close()
		{
			return _container.Close();
		}

		public IExtObjectContainer Ext()
		{
			return this;
		}

        public void Commit()
        {
            _container.Commit();
        }

        public IQuery Query()
        {
            return new QueryAdapter(_container.Query());
        }
	}

    class QueryAdapter : IQuery
    {
        private com.db4o.query.Query _query;        
        public QueryAdapter(com.db4o.query.Query query)
        {
            _query = query;
        }
    }
}
";
			}
		}
		#endregion

		#region CamelCaseAdapter
		string CamelCaseAdapter
		{
			get
			{
				return CommonCode + @"

namespace Db4objects.Db4o
{
	using Db4objects.Db4o.Ext;

	public class Db4oFactory
	{
		public static IObjectContainer OpenFile(string fname)
		{
			return new ObjectContainerAdapter(com.db4o.Db4o.openFile(fname));
		}

		public static string Version()
		{
			return com.db4o.Db4o.version();
		}
	}

	class ObjectContainerAdapter : IExtObjectContainer
	{
		private readonly ObjectContainer _container;

		public ObjectContainerAdapter(ObjectContainer container)
		{
			_container = container;
		}

		public void Set(object o)
		{
			_container.set(o);
		}

		public bool Close()
		{
			return _container.close();
		}

		public IExtObjectContainer Ext()
		{
			return this;
		}
	}
}
";
			}
		}
		#endregion

		#region CommonCode
		string CommonCode
		{
			get
			{
				return @"
using System;
using com.db4o;

namespace Db4objects.Db4o.Ext
{
	public interface IExtObjectContainer : IObjectContainer
	{
	}
}

namespace Db4objects.Db4o.Foundation
{
	public interface IFunction4
	{
		object Apply(object value);
	}
}

namespace Db4objects.Db4o
{
	using Db4objects.Db4o.Ext;
    using Db4objects.Db4o.Query;
	
	public interface IObjectContainer
	{
		IQuery Query();
		void Set(object o);
		void Delete(object obj);
		IExtObjectContainer Ext();
		bool Close();
        void Commit();
	}
}

namespace Db4objects.Db4o.Foundation.IO
{
	using System.IO;

	public class File4
	{
		public static void Delete(string file)
		{
			if (File.Exists(file))
			{
				File.Delete(file);
			}
		}
	}
}

namespace Sharpen
{
	public class Runtime 
    {
		public static string Substring(string s, int startIndex, int endIndex)
		{
			return s.Substring(startIndex, endIndex - startIndex);
		}

		public static string Substring(string s, int startIndex)
		{
			return s.Substring(startIndex);
		}
    }
}

";
			}
		}
		#endregion
	}
#endif
}