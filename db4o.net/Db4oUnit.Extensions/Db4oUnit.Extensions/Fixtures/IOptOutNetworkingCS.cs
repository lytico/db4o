/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	/// <summary>
	/// Marker interface to denote that implementing test cases should be excluded
	/// from running with the networking Client/Server fixture.
	/// </summary>
	/// <remarks>
	/// Marker interface to denote that implementing test cases should be excluded
	/// from running with the networking Client/Server fixture.
	/// </remarks>
	public interface IOptOutNetworkingCS : IOptOutFromTestFixture
	{
	}
}
