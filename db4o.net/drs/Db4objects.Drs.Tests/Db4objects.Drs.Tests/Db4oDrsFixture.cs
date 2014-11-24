/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Sharpen;
using Sharpen.IO;

namespace Db4objects.Drs.Tests
{
	public class Db4oDrsFixture : IDrsProviderFixture
	{
		protected string _name;

		protected IExtObjectContainer _db;

		protected ITestableReplicationProviderInside _provider;

		protected readonly File testFile;

		private IConfiguration _config;

		internal static readonly string RamDriveProperty = "db4o.drs.path";

		private static readonly string Path;

		static Db4oDrsFixture()
		{
			// TODO: No need to maintain the database here. It can be in the provider. 
			string path = Runtime.GetProperty(RamDriveProperty);
			if (path == null)
			{
				path = null;
			}
			if (path == null || path.Length == 0)
			{
				Sharpen.Runtime.Out.WriteLine("You can tune dRS tests by setting the environment variable "
					);
				Sharpen.Runtime.Out.WriteLine(RamDriveProperty);
				Sharpen.Runtime.Out.WriteLine("to your RAM drive.");
				path = ".";
			}
			Path = path;
		}

		public Db4oDrsFixture(string name) : this(name, null)
		{
		}

		public Db4oDrsFixture(string name, IReflector reflector)
		{
			_name = name;
			File folder = new File(Path);
			if (!folder.Exists())
			{
				Sharpen.Runtime.Out.WriteLine("Path " + Path + " does not exist. Using current working folder."
					);
				Sharpen.Runtime.Out.WriteLine("Check the " + RamDriveProperty + " environment variable on your system."
					);
				folder = new File(".");
			}
			testFile = new File(folder.GetPath() + "/drs_cs_" + _name + ".db4o");
			if (reflector != null)
			{
				Config().ReflectWith(reflector);
			}
		}

		public virtual ITestableReplicationProviderInside Provider()
		{
			return _provider;
		}

		public virtual void Clean()
		{
			testFile.Delete();
			_config = null;
		}

		public virtual void Close()
		{
			_provider.Destroy();
			_db.Close();
			_provider = null;
		}

		public virtual void Open()
		{
			_db = Db4oFactory.OpenFile(CloneConfiguration(), testFile.GetPath()).Ext();
			_provider = Db4oProviderFactory.NewInstance(_db, _name);
		}

		private IConfiguration CloneConfiguration()
		{
			return (IConfiguration)((IDeepClone)Config()).DeepClone(null);
		}

		public virtual IConfiguration Config()
		{
			if (_config == null)
			{
				_config = Db4oFactory.NewConfiguration();
			}
			return _config;
		}

		public virtual void Destroy()
		{
		}
	}
}
