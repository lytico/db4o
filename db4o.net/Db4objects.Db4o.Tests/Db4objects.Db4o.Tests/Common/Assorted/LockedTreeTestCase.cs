/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class LockedTreeTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new LockedTreeTestCase().RunSolo();
		}

		public virtual void TestAdd()
		{
			LockedTree lockedTree = new LockedTree();
			lockedTree.Add(new TreeInt(1));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_21(lockedTree));
		}

		private sealed class _ICodeBlock_21 : ICodeBlock
		{
			public _ICodeBlock_21(LockedTree lockedTree)
			{
				this.lockedTree = lockedTree;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				lockedTree.TraverseLocked(new _IVisitor4_23(lockedTree));
			}

			private sealed class _IVisitor4_23 : IVisitor4
			{
				public _IVisitor4_23(LockedTree lockedTree)
				{
					this.lockedTree = lockedTree;
				}

				public void Visit(object obj)
				{
					TreeInt treeInt = (TreeInt)obj;
					if (treeInt._key == 1)
					{
						lockedTree.Add(new TreeInt(2));
					}
				}

				private readonly LockedTree lockedTree;
			}

			private readonly LockedTree lockedTree;
		}

		public virtual void TestClear()
		{
			LockedTree lockedTree = new LockedTree();
			lockedTree.Add(new TreeInt(1));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_38(lockedTree));
		}

		private sealed class _ICodeBlock_38 : ICodeBlock
		{
			public _ICodeBlock_38(LockedTree lockedTree)
			{
				this.lockedTree = lockedTree;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				lockedTree.TraverseLocked(new _IVisitor4_40(lockedTree));
			}

			private sealed class _IVisitor4_40 : IVisitor4
			{
				public _IVisitor4_40(LockedTree lockedTree)
				{
					this.lockedTree = lockedTree;
				}

				public void Visit(object obj)
				{
					lockedTree.Clear();
				}

				private readonly LockedTree lockedTree;
			}

			private readonly LockedTree lockedTree;
		}
	}
}
