/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Header
{
	public class OldHeaderTest : ITestLifeCycle, IOptOutNoFileSystemData, IOptOutWorkspaceIssue
	{
		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Test()
		{
			string originalFilePath = OriginalFilePath();
			string dbFilePath = DbFilePath();
			if (!System.IO.File.Exists(originalFilePath))
			{
				TestPlatform.EmitWarning(originalFilePath + " does not exist. Can not run " + GetType
					().FullName);
				return;
			}
			File4.Copy(originalFilePath, dbFilePath);
			Db4oFactory.Configure().AllowVersionUpdates(true);
			Db4oFactory.Configure().ExceptionsOnNotStorable(false);
			IObjectContainer oc = Db4oFactory.OpenFile(dbFilePath);
			try
			{
				Assert.IsNotNull(oc);
			}
			finally
			{
				oc.Close();
				Db4oFactory.Configure().ExceptionsOnNotStorable(true);
				Db4oFactory.Configure().AllowVersionUpdates(false);
			}
		}

		private static string OriginalFilePath()
		{
			return WorkspaceServices.WorkspaceTestFilePath("db4oVersions/db4o_5.5.2");
		}

		private static string DbFilePath()
		{
			return WorkspaceServices.WorkspaceTestFilePath("db4oVersions/db4o_5.5.2.db4o");
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			string tempTestFilePath = DbFilePath();
			if (System.IO.File.Exists(tempTestFilePath))
			{
				File4.Delete(tempTestFilePath);
			}
		}
	}
}
