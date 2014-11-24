/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	public interface ITestableReplicationProviderInside : IReplicationProviderInside, 
		ISimpleObjectContainer
	{
		bool SupportsMultiDimensionalArrays();

		bool SupportsHybridCollection();

		bool SupportsRollback();

		void Commit();

		long ObjectVersion(object storedObject);

		void WaitForPreviousCommits();

		long CreationTime(object obj);
	}
}
