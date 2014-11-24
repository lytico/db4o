/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class StorageTestUnitBase : TestWithTempFile
	{
		protected IBin _bin;

		public StorageTestUnitBase() : base()
		{
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			base.SetUp();
			Open(false);
		}

		protected virtual void Open(bool readOnly)
		{
			if (null != _bin)
			{
				throw new InvalidOperationException();
			}
			_bin = Storage().Open(new BinConfiguration(TempFile(), false, 0, readOnly));
		}

		/// <exception cref="System.Exception"></exception>
		public override void TearDown()
		{
			Close();
			base.TearDown();
		}

		protected virtual void Close()
		{
			if (null != _bin)
			{
				_bin.Sync();
				_bin.Close();
				_bin = null;
			}
		}

		private IStorage Storage()
		{
			return ((IStorage)SubjectFixtureProvider.Value());
		}
	}
}
