/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Regression;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Regression
{
	/// <exclude></exclude>
	public class COR234TestCase : ITestCase, IOptOutNoFileSystemData, IOptOutWorkspaceIssue
	{
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
			Db4oFactory.Configure().AllowVersionUpdates(false);
			Db4oFactory.Configure().ReflectWith(Platform4.ReflectorForType(typeof(COR234TestCase
				)));
			Assert.Expect(typeof(OldFormatException), new _ICodeBlock_36(this));
		}

		private sealed class _ICodeBlock_36 : ICodeBlock
		{
			public _ICodeBlock_36(COR234TestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oFactory.OpenFile(this._enclosing.OldDatabaseFilePath());
			}

			private readonly COR234TestCase _enclosing;
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
