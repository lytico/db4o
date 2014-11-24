/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
namespace Db4objects.Drs.Tests.Foundation
{
	public class ObjectSetCollection4FacadeTestCase : Db4oUnit.ITestCase
	{
		public static void Main(string[] args)
		{
			new Db4oUnit.ConsoleTestRunner(typeof(Db4objects.Drs.Tests.Foundation.ObjectSetCollection4FacadeTestCase)
				).Run();
		}

		public virtual void TestEmpty()
		{
			Db4objects.Drs.Foundation.ObjectSetCollection4Facade facade = new Db4objects.Drs.Foundation.ObjectSetCollection4Facade
				(new Db4objects.Db4o.Foundation.Collection4());
			Db4oUnit.Assert.IsFalse(facade.HasNext());
			Db4oUnit.Assert.IsFalse(facade.HasNext());
		}

		public virtual void TestIteration()
		{
			Db4objects.Db4o.Foundation.Collection4 collection = new Db4objects.Db4o.Foundation.Collection4
				();
			collection.Add("bar");
			collection.Add("foo");
			Db4objects.Drs.Foundation.ObjectSetCollection4Facade facade = new Db4objects.Drs.Foundation.ObjectSetCollection4Facade
				(collection);
			Db4oUnit.Assert.IsTrue(facade.HasNext());
			Db4oUnit.Assert.AreEqual("bar", facade.Next());
			Db4oUnit.Assert.IsTrue(facade.HasNext());
			Db4oUnit.Assert.AreEqual("foo", facade.Next());
			Db4oUnit.Assert.IsFalse(facade.HasNext());
			facade.Reset();
			Db4oUnit.Assert.AreEqual("bar", facade.Next());
			Db4oUnit.Assert.AreEqual("foo", facade.Next());
			Db4oUnit.Assert.IsFalse(facade.HasNext());
		}
	}
}
