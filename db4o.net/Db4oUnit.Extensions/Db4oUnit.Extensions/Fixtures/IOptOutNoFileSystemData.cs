/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	/// <summary>
	/// Marker interface to denote that implementing test cases should be excluded
	/// from running within a fixture that may not provide access to required data
	/// on the file system.
	/// </summary>
	/// <remarks>
	/// Marker interface to denote that implementing test cases should be excluded
	/// from running within a fixture that may not provide access to required data
	/// on the file system. (This opt-out probably should be replaced by a less
	/// file system dependent access mechanism to this data.)
	/// </remarks>
	public interface IOptOutNoFileSystemData : IOptOutFromTestFixture
	{
	}
}
