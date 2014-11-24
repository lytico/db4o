/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class SystemInfoTestCase : Db4oTestWithTempFile, IOptOutNoFileSystemData
	{
		private IObjectContainer _db;

		public class Item
		{
		}

		public static void Main(string[] arguments)
		{
			new ConsoleTestRunner(typeof(SystemInfoTestCase)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			_db = Db4oEmbedded.OpenFile(NewConfiguration(), TempFile());
		}

		/// <exception cref="System.Exception"></exception>
		public override void TearDown()
		{
			Close();
			base.TearDown();
		}

		private void Close()
		{
			if (_db != null)
			{
				_db.Close();
				_db = null;
			}
		}

		public virtual void TestDefaultFreespaceInfo()
		{
			AssertFreespaceInfo(FileSession().SystemInfo());
		}

		private LocalObjectContainer FileSession()
		{
			return (LocalObjectContainer)Db();
		}

		private IExtObjectContainer Db()
		{
			return _db.Ext();
		}

		private void AssertFreespaceInfo(ISystemInfo info)
		{
			Assert.IsNotNull(info);
			SystemInfoTestCase.Item item = new SystemInfoTestCase.Item();
			Db().Store(item);
			Db().Commit();
			Db().Delete(item);
			Db().Commit();
			Assert.IsTrue(info.FreespaceEntryCount() > 0);
			Assert.IsTrue(info.FreespaceSize() > 0);
		}

		public virtual void TestTotalSize()
		{
			long actual = Db().SystemInfo().TotalSize();
			Close();
			long expectedSize = File4.Size(TempFile());
			Assert.AreEqual(expectedSize, actual);
		}
	}
}
