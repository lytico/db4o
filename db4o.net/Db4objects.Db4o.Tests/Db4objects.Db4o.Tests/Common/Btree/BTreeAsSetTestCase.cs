/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class BTreeAsSetTestCase : BTreeTestCaseBase
	{
		/// <summary>For now this won't work completely easy.</summary>
		/// <remarks>
		/// For now this won't work completely easy.
		/// If multiple transactions add the same value, there may
		/// be multiple add patches in the BTree with the same value.
		/// There could be many of these patches and they could even
		/// be on different nodes, so we may be on the wrong node
		/// when we want to check.
		/// We will have to take a look at this again for unique field
		/// values, so the test can stay here.
		/// </remarks>
		public virtual void _testAddSameValueFromSameTransaction()
		{
			Add(42);
			Add(42);
			AssertSingleElement(42);
		}
	}
}
