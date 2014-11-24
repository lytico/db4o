using System;
using System.IO;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oTestRunner
{
	public abstract class AbstractDb4oTesterBase : MarshalByRefObject, ITestRunner
	{
		void ITestRunner.Run(ILogger logger)
		{
			_logger = logger;
			Run();
		}

		void ITestRunner.TearDown()
		{
			if (File.Exists(_filePath))
			{
				File.Delete(_filePath);
			}

			if (_db != null) _db.Close();
		}

		protected abstract void Run();

		protected void Reopen()
		{
			if (null != _db)
			{
				_db.Close();
				_db = null;
			}
		}

		protected IObjectContainer Db()
		{
			if (_db == null)
			{
				_db = Db4oFactory.OpenFile(Configure(), TempFilePath());
			}

			return _db;
		}

		protected IConfiguration Configure()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			return Configure(config);
		}

		protected virtual IConfiguration Configure(IConfiguration config)
		{
			return config;
		}

		protected string LogText()
		{
			return _log.ToString();
		}

		protected string TempFilePath()
		{
			if (String.IsNullOrEmpty(_filePath))
			{
				_filePath = Path.GetTempFileName();
			}

			return _filePath;
		}

		protected ILogger _logger;
		private string _filePath = string.Empty;
		private readonly StringBuilder _log = new StringBuilder();
		private IObjectContainer _db;
	}
}
