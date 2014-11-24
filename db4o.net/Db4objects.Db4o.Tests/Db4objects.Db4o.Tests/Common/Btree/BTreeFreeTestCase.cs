/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class BTreeFreeTestCase : BTreeTestCaseBase
	{
		private static readonly int[] Values = new int[] { 1, 2, 5, 7, 8, 9, 12 };

		public static void Main(string[] args)
		{
			new BTreeFreeTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Add(Values);
			Trans().Commit();
			BTreeAssert.AssertAllSlotsFreed(FileTransaction(), _btree, new _ICodeBlock_22(this
				));
		}

		private sealed class _ICodeBlock_22 : ICodeBlock
		{
			public _ICodeBlock_22(BTreeFreeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing._btree.Free((LocalTransaction)this._enclosing.SystemTrans());
				this._enclosing.SystemTrans().Commit();
			}

			private readonly BTreeFreeTestCase _enclosing;
		}

		private LocalTransaction FileTransaction()
		{
			return ((LocalTransaction)Trans());
		}
	}
}
