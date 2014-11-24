/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4oUnit;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.Silverlight.IO
{
	public class IsolatedStorageFileBinTestSuite : FixtureBasedTestSuite
	{
		public static FixtureVariable LOCK_FILE_VARIABLE = FixtureVariable.NewInstance("LockFile");
		public static FixtureVariable READONLY_VARIABLE = FixtureVariable.NewInstance("ReadOnly");

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(IsolatedStorageFileBinTestUnit), };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
			       	{
			       		new SimpleFixtureProvider(LOCK_FILE_VARIABLE, new object[] {true, false}),
						new SimpleFixtureProvider(READONLY_VARIABLE, new object[]  {true, false}),
					};
		}
	}

	public class IsolatedStorageFileBinTestUnit : ITestCase
	{
		public void TestReadOnlyBin()
		{
			IBin bin = OpenBin();
			try
			{
				bin.Write(0, new byte[] {1,2,3}, 3);
			}
			catch (Db4oIOException)
			{
				_exceptionCaught = true;
				if (!IsReadOnly())
				{
					throw;
				}
			}
			finally
			{
				bin.Close();
			}
			Assert.AreEqual(IsReadOnly(), _exceptionCaught, "Exceptions are expected in ReadOnly mode.");
		}

		public void TestLockedBin()
		{
			IBin bin = OpenBin();
			try
			{
				OpenBin().Close();
			}
			catch (DatabaseFileLockedException)
			{
				_exceptionCaught = true;
				if (!IsLocked())
				{
					throw;
				}
			}
			bin.Close();
			Assert.AreEqual(IsLocked(), _exceptionCaught, "Exceptions are expected in locked mode.");
		}

		private IBin OpenBin()
		{
			return _storage.Open(new BinConfiguration("testBin", IsLocked(), 0, IsReadOnly()));
		}

		private static bool IsReadOnly()
		{
			return (bool) IsolatedStorageFileBinTestSuite.READONLY_VARIABLE.Value;
		}

		private static bool IsLocked()
		{
			return (bool) IsolatedStorageFileBinTestSuite.LOCK_FILE_VARIABLE.Value;
		}
		
		private readonly IsolatedStorageStorage _storage = new IsolatedStorageStorage();
		private bool _exceptionCaught;
	}
}

#endif