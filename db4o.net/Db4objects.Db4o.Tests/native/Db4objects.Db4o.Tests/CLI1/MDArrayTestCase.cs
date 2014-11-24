/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class MDArrayTestCase : AbstractDb4oTestCase
	{
        int[,] ints;

        override protected void Store()
        {
            ints = new int[2,2];
            ints[0,0] = 10;
            Store(this);
        }

        public void _Test()
        {
            MDArrayTestCase csa = (MDArrayTestCase) this.RetrieveOnlyInstance(typeof (MDArrayTestCase));
            Assert.AreEqual(10, csa.ints[0,0]);
        }
	}
}
