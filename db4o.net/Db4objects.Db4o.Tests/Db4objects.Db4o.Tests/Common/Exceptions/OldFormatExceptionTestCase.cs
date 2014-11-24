/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Exceptions;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	/// <exclude></exclude>
	public class OldFormatExceptionTestCase : ITestCase, IOptOutNoFileSystemData, IOptOutWorkspaceIssue
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(OldFormatExceptionTestCase)).Run();
		}

		// It is also regression test for COR-634.
		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			if (WorkspaceServices.WorkspaceRoot == null)
			{
				Sharpen.Runtime.Err.WriteLine("Build environment not available. Skipping test case..."
					);
				return;
			}
			if (!System.IO.File.Exists(SourceFile()))
			{
				Sharpen.Runtime.Err.WriteLine("Test source file " + SourceFile() + " not available. Skipping test case..."
					);
				return;
			}
			string oldDatabaseFilePath = OldDatabaseFilePath();
			Assert.Expect(typeof(OldFormatException), new _ICodeBlock_43(this, oldDatabaseFilePath
				));
			IObjectContainer container = null;
			try
			{
				container = Db4oFactory.OpenFile(NewConfiguration(true), oldDatabaseFilePath);
			}
			finally
			{
				if (container != null)
				{
					container.Close();
				}
			}
		}

		private sealed class _ICodeBlock_43 : ICodeBlock
		{
			public _ICodeBlock_43(OldFormatExceptionTestCase _enclosing, string oldDatabaseFilePath
				)
			{
				this._enclosing = _enclosing;
				this.oldDatabaseFilePath = oldDatabaseFilePath;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oFactory.OpenFile(this._enclosing.NewConfiguration(false), oldDatabaseFilePath
					);
			}

			private readonly OldFormatExceptionTestCase _enclosing;

			private readonly string oldDatabaseFilePath;
		}

		private IConfiguration NewConfiguration(bool allowVersionUpdates)
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.ReflectWith(Platform4.ReflectorForType(typeof(OldFormatExceptionTestCase))
				);
			config.AllowVersionUpdates(allowVersionUpdates);
			return config;
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected virtual string OldDatabaseFilePath()
		{
			string oldFile = IOServices.BuildTempPath("old_db.db4o");
			File4.Copy(SourceFile(), oldFile);
			return oldFile;
		}

		private string SourceFile()
		{
			return WorkspaceServices.WorkspaceTestFilePath("db4oVersions/db4o_3.0.3");
		}
	}
}
