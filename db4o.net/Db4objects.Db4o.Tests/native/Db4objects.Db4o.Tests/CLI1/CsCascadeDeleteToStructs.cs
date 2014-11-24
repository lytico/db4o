/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsCascadeDeleteToStructs : AbstractDb4oTestCase
    {
        public CDSStruct myStruct;

        protected override void Store()
        {
            myStruct = new CDSStruct(3, "hi");
            Store(this);
        }

        public void TestOne()
        {
            EnsureOccurrences(1, myStruct);
            myStruct.foo = 44;
            myStruct.bar = "cool";
            Db().Store(this);
            EnsureOccurrences(1, myStruct);

            Db().Delete(this);
            Db().Commit();
            EnsureOccurrences(0, myStruct);
        }

        private void EnsureOccurrences(int expected, object template)
        {
			Assert.AreEqual(expected, Db().QueryByExample(template).Count);
        }
    }

    public struct CDSStruct
    {
        public int foo;
        public string bar;

        public CDSStruct(int foo, string bar)
        {
            this.foo = foo;
            this.bar = bar;
        }
    }
}
