using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;

namespace EnumHandling
{
	public class Db4oTest : MarshalByRefObject, IRunner
	{
		private readonly string _databasePath;

		public Db4oTest()
		{
			_databasePath = Path.GetTempFileName();
		}

		public string Run(SimpleEnum enumValue, bool runIndexed)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			string objectsMsg = PopulateDatabase(runIndexed);
			string result = CollectResults(enumValue);
			sw.Stop();

			CleanUp();

			return "\r\n" + objectsMsg + "\r\n" + result + "\r\nin " + sw.ElapsedMilliseconds + " ms.";
		}

		private void CleanUp()
		{
			if (File.Exists(_databasePath))
			{
				File.Delete(_databasePath);
			}
		}

		private string CollectResults(SimpleEnum enumValue)
		{
			StringBuilder builder = new StringBuilder(1000);
			using (IObjectContainer db = OpenDatabase())
			{
				builder.AppendFormat("========== Db4o {0} =========={1}{1}", Db4oVersion.Name, Environment.NewLine);
				builder.AppendFormat("SimpleEnum instance count: {0}\r\n\r\n", db.Ext().StoredClass(typeof(SimpleEnum)).InstanceCount());

				AppendQueryResults(builder, enumValue, RunSODAQuery(db, enumValue), "SODA");
				AppendQueryResults(builder, enumValue, RunNativeQuery(db, enumValue), "NQ");
			}

			return builder.ToString();
		}

		private static IEnumerable RunNativeQuery(IObjectContainer db, SimpleEnum value)
		{
			return db.Query((Container c) => c.SimpleEnum == value);
		}

		private static void AppendQueryResults(StringBuilder builder, SimpleEnum enumValue, IEnumerable result, string queryType)
		{
			builder.AppendFormat("[{1}] Quering for '{0}' returned objects: \r\n", enumValue, queryType);
			foreach (Container container in result)
			{
				builder.AppendLine("\t" + container);
			}
		}

		private static IObjectSet RunSODAQuery(IObjectContainer db, SimpleEnum value)
		{
			IQuery query = db.Query();
			query.Constrain(typeof(Container));
			query.Descend("_simpleEnum").Constrain(value);

			return query.Execute();
		}

		private string PopulateDatabase(bool runIndexed)
		{
			using (IObjectContainer db = OpenDatabase(runIndexed))
			{
				return WriteData(db);
			}
		}

		private IObjectContainer OpenDatabase()
		{
			return Db4oFactory.OpenFile(Config(), _databasePath);
		}

		private IObjectContainer OpenDatabase(bool runIndexed)
		{
			return Db4oFactory.OpenFile(Config(runIndexed), _databasePath);
		}

		private static IConfiguration Config()
		{
			IConfiguration configuration = Db4oFactory.NewConfiguration();
			configuration.OptimizeNativeQueries(true);
			configuration.Diagnostic().AddListener(new MessageBoxDiagnosticListener());

			return configuration;
		}

		private static IConfiguration Config(bool runIndexed)
		{
			IConfiguration config = Config();
			config.ObjectClass(typeof(Container)).ObjectField("_simpleEnum").Indexed(runIndexed);

			return config;
		}

		private static string WriteData(IObjectContainer db)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Container c in Objects())
			{
				sb.AppendFormat("Inserted into db: {0}\r\n", c);
				db.Store(c);
			}

			return sb.ToString();
		}

		private static IEnumerable<Container> Objects()
		{
			return new[]
			       	{
			       		new Container(SimpleEnum.Foo, "Foo"),
			       		new Container(SimpleEnum.Bar, "Bar"),
			       		new Container(SimpleEnum.Baz, "Baz")
			       	};
		}

		public enum SimpleEnum
		{
			Foo = 0,
			Bar = 1,
			Baz = 2
		}

		public class Container
		{
			private readonly SimpleEnum _simpleEnum;
			private readonly string _name;

			public SimpleEnum SimpleEnum
			{
				get
				{
					return _simpleEnum;
				}
			}
			public string Name
			{
				get
				{
					return _name;
				}
			}

			public Container(SimpleEnum simpleEnum, string name)
			{
				_simpleEnum = simpleEnum;
				_name = name;
			}

			public override string ToString()
			{
				return "Container('" + _name + "', " + _simpleEnum + "=" + (int) _simpleEnum +")";
			}
		}
	}

	internal class MessageBoxDiagnosticListener : IDiagnosticListener
	{
		public void OnDiagnostic(IDiagnostic d)
		{
			if ( (d is NativeQueryNotOptimized) || (d is NativeQueryOptimizerNotLoaded))
			{
				MessageBox.Show(d.GetType() + "\r\n"+  d, "Info");
			}
		}
	}
}
