/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	/// <summary>
	/// Marker interface to denote that implementing test cases using JavaServices
	/// should be excluded from fixtures that run in an environment where the classpath
	/// is not captured in the java.class.path system property.
	/// </summary>
	/// <remarks>
	/// Marker interface to denote that implementing test cases using JavaServices
	/// should be excluded from fixtures that run in an environment where the classpath
	/// is not captured in the java.class.path system property.
	/// </remarks>
	public interface IOptOutNoInheritedClassPath : IOptOutFromTestFixture
	{
	}
}
