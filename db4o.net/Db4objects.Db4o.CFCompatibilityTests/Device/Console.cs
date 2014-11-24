using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Tests.Subject;
using Db4oUnit;

namespace Db4objects.Db4o.CFCompatibilityTests.Device
{
	public partial class Console : Form
	{
		public Console(string []args)
		{
			InitializeComponent();

			DatabasePath = args[0];

			_result = new ConsoleTestRunner(CFCompatibilityTests()).Run();
			Close();
		}

		private static IEnumerator CFCompatibilityTests()
		{
			return new ReflectionTestSuiteBuilder(
							new Type[] 
							{
								typeof(CFCompatibilityTestCase),
								typeof(CFCompatibilityDefragTestCase),
							}).GetEnumerator();
		}

		public int Result
		{
			get { return _result; }
		}

		internal static string DatabasePath
		{
			get
			{
				return _databaseFile;
			}

			set
			{
				_databaseFile = value;
			}
		}

		private static string _databaseFile;
		private readonly int _result;
	}

	internal class CFCompatibilityDefragTestCase : ITestCase
	{
		public void TestDefragment()
		{
			string backupFile = Console.DatabasePath + "." + Defragment.DefragmentConfig.BackupSuffix;
			if (File.Exists(backupFile))
			{
				File.Delete(backupFile);
			}

			Defragment.Defragment.Defrag(Console.DatabasePath);
		}
	}

	/**
	 * We don't support (and therefore don't test) from Desktop -> CF
	 * 
	 * - Database version updates 
	 * - 
	 * 
	 * 
	 */ 
	internal class CFCompatibilityTestCase : ITestLifeCycle
	{
		public void TestQuery()
		{
			AssertSubject("bar");
		}

		public void TestUpdate()
		{
			CFCompatibilityTestSubject<string> foobar = AssertSubject("foobar");
			Assert.AreEqual(Subjects.Item("baz"), foobar._child);

			foobar.Child(RetrieveOnlyInstance("bar"));
			Db().Store(foobar);
		}

		public void TestInsert()
		{
			Db().Store(Subjects.NewJohnDoe());
		}

		public void TestDelete()
		{
			Db().Delete(RetrieveOnlyInstance("foo"));
		}

		private CFCompatibilityTestSubject<string> AssertSubject(string name)
		{
			CFCompatibilityTestSubject<string> instance = RetrieveOnlyInstance(name);
			Assert.AreEqual(Subjects.Item(name), instance);

			return instance;
		}

		private CFCompatibilityTestSubject<string> RetrieveOnlyInstance(string name)
		{
            IList<CFCompatibilityTestSubject<string>> results = Db().Query<CFCompatibilityTestSubject<string>>(delegate(CFCompatibilityTestSubject<string> candidate) { return candidate._name == name; });
            Assert.AreEqual(1, results.Count);
            
            return results[0];
		}
	
		public void SetUp()
		{
			_db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), Console.DatabasePath);
		}

		public void TearDown()
		{
			_db.Close();
		}

		private IObjectContainer Db()
		{
			return _db;
		}

		private IObjectContainer _db;
	}
}