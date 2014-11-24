using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4oTestRunner;

namespace NullableArraysTest
{
	class UpgradeDatabase : AbstractDb4oTesterBase
	{
		protected override void Run()
		{
			if (File.Exists(DATABASE_PATH))
			{
				RunUpdateTest();
			}
			else
			{
				CreateDatabaseInOlderVersion();
			}
		}

		private void CreateDatabaseInOlderVersion()
		{
			_logger.LogMessage("Creating database ({0}) at version {1}.)", DATABASE_PATH, Db4oVersion.Name);
			
			NullableHolder<bool> obj = new NullableHolder<bool>(new bool?[] { true, false, null, false, true });
			using (IObjectContainer container = Db4oFactory.OpenFile(DATABASE_PATH))
			{
				container.Store(obj);
			}

			_logger.LogMessage("Object {0} added to database.", obj.ToString());
		}

		private void RunUpdateTest()
		{
			_logger.LogMessage("Trying to open database {0} with Db4o version {1}", DATABASE_PATH, Db4oVersion.Name);
			try
			{
				using(IObjectContainer container= Db4oFactory.OpenFile(DATABASE_PATH))
				{
					IList<NullableHolder<bool>> results = container.Query<NullableHolder<bool>>();
					foreach (var item in results)
					{
						_logger.LogMessage("Loaded item: {0}", item.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
			}
			finally
			{
				File.Delete(DATABASE_PATH);
			}
		}

		private static readonly string DATABASE_PATH = Path.Combine(Path.GetTempPath(), "NullableArrayUpdateDatabase.odb");
	}
}
