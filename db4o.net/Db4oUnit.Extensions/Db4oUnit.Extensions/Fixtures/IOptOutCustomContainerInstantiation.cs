/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	/// <summary>
	/// Opts out from fixtures that require customized ways of creating/opening ObjectContainer
	/// instances and won't work with test cases explicitly invoking the common factory methods.
	/// </summary>
	/// <remarks>
	/// Opts out from fixtures that require customized ways of creating/opening ObjectContainer
	/// instances and won't work with test cases explicitly invoking the common factory methods.
	/// </remarks>
	public interface IOptOutCustomContainerInstantiation : IOptOutFromTestFixture
	{
	}
}
