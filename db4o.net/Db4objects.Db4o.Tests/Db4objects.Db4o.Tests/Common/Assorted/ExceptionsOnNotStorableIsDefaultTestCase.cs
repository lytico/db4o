/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <exclude></exclude>
	public class ExceptionsOnNotStorableIsDefaultTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ExceptionsOnNotStorableIsDefaultTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
		}

		public class Item
		{
			public Item(object obj)
			{
				if (obj == null)
				{
					throw new Exception();
				}
			}

			public static ExceptionsOnNotStorableIsDefaultTestCase.Item NewItem()
			{
				return new ExceptionsOnNotStorableIsDefaultTestCase.Item(new object());
			}
		}

		public virtual void TestObjectContainerAliveAfterObjectNotStorableException()
		{
			Assert.Expect(typeof(ObjectNotStorableException), new _ICodeBlock_38(this));
		}

		private sealed class _ICodeBlock_38 : ICodeBlock
		{
			public _ICodeBlock_38(ExceptionsOnNotStorableIsDefaultTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Store(ExceptionsOnNotStorableIsDefaultTestCase.Item.NewItem());
			}

			private readonly ExceptionsOnNotStorableIsDefaultTestCase _enclosing;
		}
	}
}
