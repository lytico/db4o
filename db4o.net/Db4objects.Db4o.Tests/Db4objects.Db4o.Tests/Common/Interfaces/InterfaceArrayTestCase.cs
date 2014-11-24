/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Interfaces;

namespace Db4objects.Db4o.Tests.Common.Interfaces
{
	public class InterfaceArrayTestCase : AbstractDb4oTestCase
	{
		public interface IFoo
		{
		}

		public class FooImpl : InterfaceArrayTestCase.IFoo
		{
		}

		public class Bar
		{
			public Bar(InterfaceArrayTestCase.IFoo[] foos)
			{
				this.foos = foos;
			}

			public InterfaceArrayTestCase.IFoo[] foos;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new InterfaceArrayTestCase.Bar(new InterfaceArrayTestCase.IFoo[] { new InterfaceArrayTestCase.FooImpl
				() }));
		}

		public virtual void Test()
		{
			Assert.AreEqual(1, ((InterfaceArrayTestCase.Bar)RetrieveOnlyInstance(typeof(InterfaceArrayTestCase.Bar
				))).foos.Length);
		}
	}
}
