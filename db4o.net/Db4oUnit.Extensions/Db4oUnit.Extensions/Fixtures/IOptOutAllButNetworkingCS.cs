/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	/// <summary>
	/// Marker interface to denote that implementing test cases should be excluded
	/// from running with any fixture but a networking C/S one.
	/// </summary>
	/// <remarks>
	/// Marker interface to denote that implementing test cases should be excluded
	/// from running with any fixture but a networking C/S one.
	/// </remarks>
	public interface IOptOutAllButNetworkingCS : IOptOutSolo
	{
	}
}
