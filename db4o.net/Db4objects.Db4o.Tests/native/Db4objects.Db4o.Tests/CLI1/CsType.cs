/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsType : AbstractDb4oTestCase, IOptOutSilverlight
    {
        public Type myType;
		public Type stringType;

        override protected void Store()
        {
            myType = GetType();
            stringType = typeof(String);
            Store(this);
        }

        public void Test()
        {
            Assert.AreEqual(GetType(), myType);
            Assert.AreEqual(typeof(String), stringType);
        }
    }
}
