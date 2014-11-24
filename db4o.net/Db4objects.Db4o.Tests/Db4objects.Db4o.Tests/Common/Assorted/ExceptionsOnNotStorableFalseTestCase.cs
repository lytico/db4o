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
	public class ExceptionsOnNotStorableFalseTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ExceptionsOnNotStorableFalseTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ExceptionsOnNotStorable(false);
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

			public static ExceptionsOnNotStorableFalseTestCase.Item NewItem()
			{
				return new ExceptionsOnNotStorableFalseTestCase.Item(new object());
			}
		}

		public virtual void TestObjectContainerAliveAfterObjectNotStorableException()
		{
			ExceptionsOnNotStorableFalseTestCase.Item item = ExceptionsOnNotStorableFalseTestCase.Item
				.NewItem();
			bool exceptionOccurred = false;
			try
			{
				Store(item);
			}
			catch (ObjectNotStorableException)
			{
				exceptionOccurred = true;
			}
			Assert.IsFalse(exceptionOccurred);
		}
	}
}
