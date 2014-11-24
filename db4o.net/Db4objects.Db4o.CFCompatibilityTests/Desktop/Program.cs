using System;
using System.Collections.Generic;
using System.IO;
using Tests.Subject;

namespace Db4objects.Db4o.CFCompatibilityTests
{
	class Program
	{
		public static int Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.Error.WriteLine("Db4objects.Db4o.CFCompatibilityTests.Desktop -generateDatabase|-validateDatabase <database path>");
				return -1;
			}

			string databaseFile = args[1];
			switch(args[0])
			{
				case  "-generateDatabase":
					GenerateDatabase(databaseFile);
					return 0;
				
				case "-validateDatabase":
					return ValidateDatabase(databaseFile);
			}

			Console.Error.WriteLine("Invalid parameter: {0}", args[0]);
			return -2;
		}

		private static int ValidateDatabase(string databaseFile)
		{
			int errorCount = 0;
			using (IObjectContainer db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), databaseFile))
			{
                IList<CFCompatibilityTestSubject<string>> results = db.Query<CFCompatibilityTestSubject<string>>(delegate(CFCompatibilityTestSubject<string> candidate) { return candidate._name == "foo"; });
				if (results.Count > 0)
				{
					Console.Error.WriteLine("Object with name 'foo' should be deleted but still exists in db.");
					errorCount++;
				}

				const string johnDoeName = "john.doe";
				CFCompatibilityTestSubject<string> subject = RetrieveOnlyInstance(db, johnDoeName);
				if (null == subject)
				{
					errorCount++;
				}

				CFCompatibilityTestSubject<string> expected = Subjects.NewJohnDoe();
				if (!expected.Equals(subject))
				{
					Console.Error.WriteLine("Expected {0} but got {1}", expected, subject != null ? subject.ToString() : "null");
					errorCount++;
				}

				subject = RetrieveOnlyInstance(db, "foobar");
				if (null == subject)
				{
					errorCount++;
				}

				if (!Subjects.Item("bar").Equals(subject._child))
				{
					Console.Error.WriteLine("Foo._child expected to be {0} but was {1}", Subjects.Item("bar"), subject._child);
					errorCount++;
				}
			}

			return errorCount;
		}

		private static CFCompatibilityTestSubject<string> RetrieveOnlyInstance(IObjectContainer db, string name)
		{
            IList<CFCompatibilityTestSubject<string>> results = db.Query<CFCompatibilityTestSubject<string>>(delegate(CFCompatibilityTestSubject<string> candidate) { return candidate._name == name; });
			if (results.Count != 1)
			{
				Console.Error.WriteLine("Expected one instance of '{0}' but found '{1}'", name, results.Count);
				return null;
			}

			return results[0];
		}

		private static void GenerateDatabase(string databaseFile)
		{
			if (File.Exists(databaseFile))
			{
				File.Delete(databaseFile);	
			}

			Console.Error.WriteLine("Generating database: {0}", databaseFile);
			using (IObjectContainer db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), databaseFile))
			{
				foreach (CFCompatibilityTestSubject<string> subject in Subjects.Objects)
				{
					db.Store(subject);
				}
			}
		}
	}
}