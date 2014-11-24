/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;

namespace Db4oUnit
{
	/// <summary>For test cases that need setUp/tearDown support.</summary>
	/// <remarks>For test cases that need setUp/tearDown support.</remarks>
	public interface ITestLifeCycle : ITestCase
	{
		/// <exception cref="System.Exception"></exception>
		void SetUp();

		/// <exception cref="System.Exception"></exception>
		void TearDown();
	}
}
